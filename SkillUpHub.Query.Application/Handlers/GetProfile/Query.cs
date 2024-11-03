using MediatR;

namespace SkillUpHub.Query.Application.Handlers.GetProfile;

public class Query : IRequest<ViewModel>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}