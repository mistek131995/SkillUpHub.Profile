using MediatR;
using SkillUpHub.Command.Infrastructure.Interfaces;
using SaveProfile = SkillUpHub.Command.Application.CommandHandlers.SaveProfile;

namespace SkillUpHub.Command.Application.MessageHandlers;

public class RabbitMqMessageHandler(IMediator mediator) : IRabbitMqMessageHandler
{
    public async Task CreateDefaultUserProfileAsync(Guid guid)
    {
        await mediator.Send(new SaveProfile.Command()
        {
            FirstName = "John",
            LastName = "Doe",
            BirthDate = DateTime.Now.AddYears(-20),
            CountryId = 0,
            Description = "",
            UserId = guid
        });
    }
}