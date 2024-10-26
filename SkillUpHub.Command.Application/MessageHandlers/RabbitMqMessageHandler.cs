using SkillUpHub.Command.Infrastructure.Interfaces;
using SkillUpHub.Profile.Contract.Providers;

namespace SkillUpHub.Command.Application.MessageHandlers;

public class RabbitMqMessageHandler(IRepositoryProvider repositoryProvider) : IRabbitMqMessageHandler
{
    public async Task CreateDefaultUserProfileAsync(Guid guid)
    {
        await repositoryProvider.ProfileRepository.SaveAsync(new SkillUpHub.Command.Contract.Models.Profile(
            userId: guid,
            firstName: "UserName",
            lastName: "UserName",
            description: String.Empty));
    }
}