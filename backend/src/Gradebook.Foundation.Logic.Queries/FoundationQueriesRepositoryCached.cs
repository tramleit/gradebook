using Gradebook.Foundation.Common;
using Gradebook.Foundation.Common.Foundation.Queries.Definitions;

namespace Gradebook.Foundation.Logic.Queries;

public class FoundationQueriesRepositoryCached : BaseRepositoryCached<FoundationQueriesRepository, object>, IFoundationQueriesRepository
{
    public FoundationQueriesRepositoryCached(FoundationQueriesRepository _base, object cacheMachine) : base(_base, cacheMachine)
    {
    }

    public Task<IEnumerable<PersonDto>> GetPeopleInSchool(Guid schoolGuid)
        => Base.GetPeopleInSchool(schoolGuid);

    public Task<Guid?> GetPersonGuidForUser(string userId)
        => Base.GetPersonGuidForUser(userId);

    public Task<IEnumerable<SchoolDto>> GetSchoolsForPerson(Guid personGuid)
        => Base.GetSchoolsForPerson(personGuid);

    public void SaveChanges()
    {
        Base.SaveChanges();
    }

    public Task SaveChangesAsync()
    {
        return Base.SaveChangesAsync();
    }
}
