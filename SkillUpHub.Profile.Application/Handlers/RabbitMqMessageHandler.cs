using SkillHub.Profile.Application.Interfaces;
using SkillUpHub.Profile.Contract.Providers;
using SkillUpHub.Profile.Infrastructure.Interfaces;
using Profile =  SkillUpHub.Profile.Contract.Models.Profile;

namespace SkillHub.Profile.Application.Handlers;

public class RabbitMqMessageHandler(IRepositoryProvider repositoryProvider) : IRabbitMqMessageHandler
{
    public async Task CreateDefaultUserProfileAsync(Guid guid)
    {
        await repositoryProvider.ProfileRepository.SaveAsync(new SkillUpHub.Profile.Contract.Models.Profile(
            userId: guid,
            firstName: "UserName",
            lastName: "UserName",
            description: String.Empty));
    }
}