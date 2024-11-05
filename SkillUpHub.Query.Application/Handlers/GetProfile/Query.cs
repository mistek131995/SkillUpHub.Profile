using MediatR;

namespace SkillUpHub.Query.Application.Handlers.GetProfile;

public class Query : IRequest<ViewModel>
{
    public Guid UserId { get; set; }
}