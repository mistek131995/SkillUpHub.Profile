using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            var guid = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value 
                       ?? throw new NullReferenceException("Sub (Guid) пользователя не был найден");
            
            var query = new GetProfile.Query
            {
                UserId = userId ?? Guid.Parse(guid)
            };

            return await mediator.Send(query);
        }).RequireAuthorization();
    }
}