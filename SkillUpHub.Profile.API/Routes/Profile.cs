using SkillUpHub.Profile.API.Interfaces;

namespace SkillUpHub.Profile.API.Routes;

public class Profile : IApi
{
    public void RegisterRoutes(WebApplication app)
    {
        app.MapGet("/GetProfile", () =>
        {

        });
    }
}