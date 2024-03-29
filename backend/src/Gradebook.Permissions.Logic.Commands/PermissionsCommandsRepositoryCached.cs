using Gradebook.Foundation.Common;

namespace Gradebook.Permissions.Logic.Commands;

public class PermissionsCommandsRepositoryCached : BaseRepositoryCached<PermissionsCommandsRepository, object>, IPermissionsCommandsRepository
{
    public PermissionsCommandsRepositoryCached(PermissionsCommandsRepository _base, object cacheMachine) : base(_base, cacheMachine)
    {
    }

    public void BeginTransaction()
    {
        Base.BeginTransaction();
    }

    public void CommitTransaction()
    {
        Base.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        Base.RollbackTransaction();
    }

    public void SaveChanges()
    {
        Base.SaveChanges();
    }

    public Task SaveChangesAsync()
    {
        return Base.SaveChangesAsync();
    }
}
