using MediatR;
using SkillUpHub.Command.Contract.Models;
using SkillUpHub.Command.Infrastructure.Interfaces;
using SkillUpHub.Profile.Contract.Providers;
using UUIDNext;

namespace SkillUpHub.Command.Application.CommandHandlers.SaveProfile;

public class CommandHandler(IRepositoryProvider repositoryProvider, IMessageBusClient messageBusClient) : IRequestHandler<Command, Unit>
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
                messageBusClient.PublishMessage(new
                {
                    request.UserId, 
                    request.SessionId
                }, exchange: "create-user", routingKey: "create-user.rollback-account");
                
                messageBusClient.PublishMessage(new
                {
                    UserId = request.SessionId,
                    Message = "Ошибка создания профиля аккаунта, попробуйте позднее.",
                    Method = "show-toast-notification",
                    Type = NotificationType.Error
                }, exchange: "notification", routingKey: "notification.toast");
                
                messageBusClient.PublishErrorMessage(ex);
            }
        }
        else
        {
            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;
            profile.Description = request.Description;

            await repositoryProvider.ProfileRepository.SaveAsync(profile);
            messageBusClient.PublishMessage(request.UserId, exchange: "", routingKey: "update-profile-success");
        }
        
        return Unit.Value;
    }
}