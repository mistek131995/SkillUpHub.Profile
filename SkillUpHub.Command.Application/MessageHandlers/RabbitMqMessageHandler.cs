using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SkillUpHub.Command.Infrastructure.Interfaces;
using SaveProfile = SkillUpHub.Command.Application.CommandHandlers.SaveProfile;

namespace SkillUpHub.Command.Application.MessageHandlers;

public class RabbitMqMessageHandler : IRabbitMqMessageHandler
{
    private readonly IMediator _mediator;
    public RabbitMqMessageHandler(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        _mediator = scope.ServiceProvider.GetService<IMediator>();
    }
    
    public async Task CreateDefaultUserProfileAsync(Guid userId, Guid sessionId)
    {
        try
        {
            await _mediator.Send(new SaveProfile.Command()
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Now.AddYears(-20),
                CountryId = 0,
                Description = "",
                UserId = userId,
                SessionId = sessionId
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            throw;
        }

    }
}