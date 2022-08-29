using Gradebook.Foundation.Common;
using Gradebook.Foundation.Common.Foundation.Commands;
using Gradebook.Foundation.Common.Foundation.Commands.Definitions;
using Gradebook.Foundation.Common.Identity.Logic.Interfaces;
using Gradebook.Foundation.Identity.Models;

namespace Gradebook.Foundation.Logic.Commands;

public class FoundationCommands : BaseLogic<IFoundationCommandsRepository>, IFoundationCommands
{
    private readonly ServiceResolver<IIdentityLogic> _identityLogic;
    public FoundationCommands(IFoundationCommandsRepository repository,
        IServiceProvider serviceProvider) : base(repository)
    {
        _identityLogic = new ServiceResolver<IIdentityLogic>(serviceProvider);
    }

    public async Task<ResponseWithStatus<bool>> AddNewStudent(NewStudentCommand command)
    {
        var resp = await Repository.AddNewStudent(command);
        await Repository.SaveChangesAsync();
        return resp;
    }

    public async Task<ResponseWithStatus<bool>> NewAdministrator(NewAdministratorCommand command)
    {
        if(command.UserGuid is null){
            var currentUserId = await _identityLogic.Service.CurrentUserId();
            if(!currentUserId.Status) return new ResponseWithStatus<bool>(false);
            command.UserGuid = currentUserId.Response;
        }

        await _identityLogic.Service.AddUserRole(UserRoles.SuperAdmin);

        var resp = await Repository.AddNewAdministrator(command);
        if(resp.Status){
            await Repository.SaveChangesAsync();
            return new ResponseWithStatus<bool>(true);
        }
        return new ResponseWithStatus<bool>(false);
    }

    public async Task<ResponseWithStatus<bool>> NewAdministratorWithSchool(NewAdministratorCommand administratorCommand, NewSchoolCommand schoolCommand)
    {
        if(administratorCommand.UserGuid is null){
            var currentUserId = await _identityLogic.Service.CurrentUserId();
            if(!currentUserId.Status) return new ResponseWithStatus<bool>(false);
            administratorCommand.UserGuid = currentUserId.Response;
        }

        await _identityLogic.Service.AddUserRole(UserRoles.SuperAdmin);

        var respAdmin = await Repository.AddNewAdministrator(administratorCommand);
        var respSchool = await Repository.AddNewSchool(schoolCommand);
        var respAddAdminToSchool = await Repository.AddAdministratorToSchool(respAdmin.Response, respSchool.Response);
        
        if(respAdmin.Status && respSchool.Status && respAddAdminToSchool.Status){
            await Repository.SaveChangesAsync();
            return new ResponseWithStatus<bool>(true);
        }
        return new ResponseWithStatus<bool>(false);
    }
}