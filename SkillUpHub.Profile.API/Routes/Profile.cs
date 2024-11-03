using MediatR;
using SkillUpHub.Profile.API.Interfaces;
using GetProfile = SkillUpHub.Query.Application.Handlers.GetProfile;

namespace SkillUpHub.Profile.API.Routes;

public class Profile : IApi
{
    public void RegisterRoutes(WebApplication app)
    {
        app.MapGet("/GetProfile", async (IMediator mediator) => await mediator.Send(new GetProfile.Query())).RequireAuthorization();
    }
}