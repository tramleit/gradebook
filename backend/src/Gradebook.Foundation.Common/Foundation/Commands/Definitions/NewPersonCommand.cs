namespace Gradebook.Foundation.Common.Foundation.Commands.Definitions;

public class NewPersonCommand
{
    public Guid CreatorGuid { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime Birthday { get; set; }
}
