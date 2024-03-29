using AutoMapper;
using Gradebook.Foundation.Common;
using Gradebook.Foundation.Common.Extensions;
using Gradebook.Foundation.Common.Foundation.Commands;
using Gradebook.Foundation.Common.Foundation.Commands.Definitions;
using Gradebook.Foundation.Common.Foundation.Models;
using Gradebook.Foundation.Common.Foundation.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Schools;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SchoolsController : ControllerBase
{
    private readonly ServiceResolver<IFoundationQueries> _foundationQueries;
    private readonly ServiceResolver<IFoundationCommands> _foundationCommands;
    private readonly ServiceResolver<IMapper> _mapper;
    public SchoolsController(IServiceProvider serviceProvider)
    {
        _foundationQueries = serviceProvider.GetResolver<IFoundationQueries>();
        _foundationCommands = serviceProvider.GetResolver<IFoundationCommands>();
        _mapper = serviceProvider.GetResolver<IMapper>();
    }
    [HttpGet]
    [Route("{schoolGuid}/People")]
    public async Task<IActionResult> GetPeopleInSchool([FromRoute] Guid schoolGuid)
    {
        var resp = await _foundationQueries.Service.GetPeopleInSchool(schoolGuid);
        return resp.Status ? Ok(resp.Response) : BadRequest(resp.Message);
    }
    [HttpPost]
    [Route("{schoolGuid}/People")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> AddPersonToSchool([FromRoute] Guid schoolGuid, [FromBody] Guid personGuid)
    {
        var resp = await _foundationCommands.Service.AddPersonToSchool(schoolGuid, personGuid);
        return resp.Status ? Ok() : BadRequest(resp.Message);
    }
    [HttpPost]
    [Route("")]
    [ProducesResponseType(typeof(string), statusCode: 400)]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> AddNewSchool([FromBody] NewSchoolModel model)
    {
        var newSchoolCommand = _mapper.Service.Map<NewSchoolCommand>(model);
        var resp = await _foundationCommands.Service.AddNewSchool(newSchoolCommand);
        return resp.Status ? Ok() : BadRequest(resp.Message);
    }
}
