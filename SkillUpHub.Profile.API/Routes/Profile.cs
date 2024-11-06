using MediatR;
using SkillUpHub.Profile.API.Interfaces;
using GetProfile = SkillUpHub.Query.Application.Handlers.GetProfile;

namespace SkillUpHub.Profile.API.Routes;

public class Profile : IApi
{
    public void RegisterRoutes(WebApplication app)
    {
        app.MapGet("/GetProfile", async (Guid? userId, IMediator mediator, HttpContext httpContext) =>
        {
            var query = new GetProfile.Query
            {
                UserId = userId ?? Guid.Parse(httpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")!.Value)
            };

            return await mediator.Send(query);
        }).RequireAuthorization();
    }
}