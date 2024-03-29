using AutoMapper;
using Gradebook.Foundation.Common.Foundation.Commands.Definitions;
using Gradebook.Foundation.Domain.Models;

namespace Gradebook.Foundation.Logic.Commands;

public class FoundationCommandsMappings : Profile
{
    public FoundationCommandsMappings()
    {
        CreateMap<NewStudentCommand, Student>();
        CreateMap<NewTeacherCommand, Teacher>();
        CreateMap<NewSchoolCommand, School>();
        CreateMap<NewPersonCommand, Administrator>();
        CreateMap<NewAdministratorCommand, Administrator>();
    }
}
