using Api.Models.Invitations;
using Gradebook.Foundation.Common;
using Gradebook.Foundation.Common.Extensions;
using Gradebook.Foundation.Common.Foundation.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.People;

[Route("api/[controller]")]
[ApiController]
[Authorize()]
public class PeopleController : ControllerBase
{
    private readonly ServiceResolver<IFoundationQueries> _foundationQueries;
    public PeopleController(IServiceProvider serviceProvider)
    {
        _foundationQueries = serviceProvider.GetResolver<IFoundationQueries>();
    }
    [HttpGet]
    [Route("{userGuid}/schools")]
    public async Task<IActionResult> GetSchools([FromRoute] string userGuid)
    {
        var userId = await _foundationQueries.Service.GetPersonGuidForUser(userGuid);
        if(!userId.Status)
            return BadRequest(userId.Message);
        var resp = await _foundationQueries.Service.GetSchoolsForPerson(userId.Response);
        return resp.Status ? Ok(resp.Response) : BadRequest();
    }
}
