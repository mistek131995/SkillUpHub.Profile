using MediatR;

namespace SkillUpHub.Command.Application.CommandHandlers.SaveProfile;

public class Command : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public DateTime BirthDate { get; set; }
    public int CountryId { get; set; }
}