using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Gradebook.Foundation.Common;
using Gradebook.Foundation.Common.Extensions;
using Gradebook.Foundation.Common.Identity.Logic.Interfaces;
using Gradebook.Foundation.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Gradebook.Foundation.Identity.Logic;

public class IdentityLogic : IIdentityLogic
{
    private readonly ServiceResolver<IConfiguration> _configuration;
    private readonly ServiceResolver<ApplicationIdentityDatabaseContext> _identityContext;
    private readonly ServiceResolver<UserManager<ApplicationUser>> _userManager;
    private readonly ServiceResolver<RoleManager<IdentityRole>> _roleManager;
    private readonly ServiceResolver<IHttpContextAccessor> _httpContextAccessor;
    public IdentityLogic(IServiceProvider serviceProvider)
    {
        _configuration = serviceProvider.GetResolver<IConfiguration>();
        _userManager = serviceProvider.GetResolver<UserManager<ApplicationUser>>();
        _roleManager = serviceProvider.GetResolver<RoleManager<IdentityRole>>();
        _identityContext = serviceProvider.GetResolver<ApplicationIdentityDatabaseContext>();
        _httpContextAccessor = serviceProvider.GetResolver<IHttpContextAccessor>();
    }
    public JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Service["JWT:Secret"]));
        _ = int.TryParse(_configuration.Service["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

        var token = new JwtSecurityToken(
            issuer: _configuration.Service["JWT:ValidIssuer"],
            audience: _configuration.Service["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }
    public async Task EditUserRoles(string[] roles, string? userGuid = null)
    {
        if (userGuid is null) userGuid = (await CurrentUserId()).Response;
        var user = await _userManager.Service.FindByIdAsync(userGuid);
        if (user is null) return;
        var rolesToRemove = _identityContext.Service.Roles
            .Join(_identityContext.Service.UserRoles, r => r.Id, ur => ur.RoleId, (r, ur) => new { r, ur })
            .Where(e => e.ur.UserId == userGuid)
            .Where(e => !roles.Select(o => o.Normalize()).Contains(e.r.NormalizedName))
            .Select(e => e.r.Name);
        if (rolesToRemove.Any())
            await _userManager.Service.RemoveFromRolesAsync(user, rolesToRemove);
        if (roles.Any())
        {
            foreach (var role in roles)
            {
                if (!await _roleManager.Service.RoleExistsAsync(role))
                    await _roleManager.Service.CreateAsync(new IdentityRole(role));
                await _userManager.Service.AddToRoleAsync(user, role);
            }
        }
    }
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Service["JWT:Secret"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
    public async Task<ResponseWithStatus<string, bool>> CurrentUserId()
    {
        var user = await _userManager.Service.FindByNameAsync(_httpContextAccessor.Service.HttpContext.User.Identity!.Name);
        return user is null ?
            new ResponseWithStatus<string, bool>(null, false) :
            new ResponseWithStatus<string, bool>(user.Id, true);
    }
    public async Task<ResponseWithStatus<string[], bool>> GetUserRoles(string? userGuid = null)
    {
        if (userGuid is null) userGuid = (await this.CurrentUserId()).Response;
        var response = _identityContext.Service.Roles
            .Join(_identityContext.Service.UserRoles, r => r.Id, ur => ur.RoleId, (r, ur) => new { r, ur })
            .Where(e => e.ur.UserId == userGuid)
            .Select(e => e.r.Name);
        return new ResponseWithStatus<string[], bool>(response.ToArray(), true);
    }
    public async Task<StatusResponse<bool>> AddUserRole(string role, string? userGuid = null)
    {
        var r = (await GetUserRoles(userGuid)).Response!.Any(e => e.Normalize() == role.Normalize());
        if (r) return new StatusResponse<bool>(true);
        await EditUserRoles((await GetUserRoles(userGuid)).Response!.Append(role).ToArray(), userGuid);
        return new StatusResponse<bool>(true);
    }
    public async Task<StatusResponse<bool>> RemoveUserRole(string role, string? userGuid = null)
    {
        await EditUserRoles((await GetUserRoles(userGuid)).Response!.Where(e => e.Normalize() != role.Normalize()).ToArray(), userGuid);
        return new StatusResponse<bool>(true);
    }
}
