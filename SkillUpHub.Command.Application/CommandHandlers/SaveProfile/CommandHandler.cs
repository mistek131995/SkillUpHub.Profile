using MediatR;
using Microsoft.Extensions.Options;
using SkillUpHub.Command.Contract.Models;
using SkillUpHub.Command.Infrastructure.Interfaces;
using SkillUpHub.Profile.Contract.Providers;
using UUIDNext;

namespace SkillUpHub.Command.Application.CommandHandlers.SaveProfile;

public class CommandHandler(IRepositoryProvider repositoryProvider, IMessageBusClient messageBusClient, IOptions<RabbitMqSettings> options) : IRequestHandler<Command, Unit>
{
    public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
    {
        var profile = await repositoryProvider.ProfileRepository.GetByUserIdAsync(request.UserId);

        if (profile == null)
        {
            try
            {
                profile = new Contract.Models.Profile(
                    id: Uuid.NewDatabaseFriendly(Database.PostgreSql),
                    userId: request.UserId,
                    firstName: request.FirstName,
                    lastName: request.LastName,
                    description: "");

                await repositoryProvider.ProfileRepository.SaveAsync(profile);
                
                messageBusClient.PublishMessage(new
                {
                    UserId = request.SessionId,
                    Message = "Аккаунт успешно создан. На электронную поту отправлено письмо с инструкцией по активации аккаунта.",
                    Method = "show-toast-notification",
                    Type = NotificationType.Success
                }, exchange: "notification", routingKey: "notification.toast");
                
                messageBusClient.PublishMessage(new
                {
                    UserId = request.SessionId,
                    Message = "Аккаунт успешно создан. На электронную поту отправлено письмо с инструкцией по активации аккаунта.",
                    Method = "register-action",
                    Type = ActionType.Success
                }, exchange: "notification", routingKey: "notification.action");
            }
            catch (Exception ex)
            {
                var errorEndpoint = options.Value.Exchanges.Find(x => x.Id == "create-account-failure");
                messageBusClient.PublishMessage((userId: request.UserId, sessionId: request.SessionId), exchange: errorEndpoint.Name, routingKey: "");
                
                messageBusClient.PublishErrorMessage(ex);
            }
        }
        else
        {
            try
            {
                profile.FirstName = request.FirstName;
                profile.LastName = request.LastName;
                profile.Description = request.Description;

                await repositoryProvider.ProfileRepository.SaveAsync(profile);
                messageBusClient.PublishMessage(request.UserId, exchange: "", routingKey: "update-profile-success");
            }
            catch (Exception ex)
            {
                var errorEndpoint = options.Value.Queues.Find(x => x.Id == "update-profile-failure");
                messageBusClient.PublishMessage((userId: request.UserId, sessionId: request.SessionId), exchange: "", routingKey: errorEndpoint.Key);
                
                messageBusClient.PublishErrorMessage(ex);
            }
        }
        
        return Unit.Value;
    }
}