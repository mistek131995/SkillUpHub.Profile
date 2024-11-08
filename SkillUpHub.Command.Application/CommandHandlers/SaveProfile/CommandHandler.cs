﻿using MediatR;
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
            profile = new Contract.Models.Profile(
                id: Uuid.NewDatabaseFriendly(Database.PostgreSql),
                userId: request.UserId,
                firstName: request.FirstName,
                lastName: request.LastName,
                description: "");
            
            await repositoryProvider.ProfileRepository.SaveAsync(profile);
            messageBusClient.PublishMessage(request.UserId, routingKey: "create-profile-success");
        }
        else
        {
            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;
            profile.Description = request.Description;

            await repositoryProvider.ProfileRepository.SaveAsync(profile);
            messageBusClient.PublishMessage(request.UserId, routingKey: "update-profile-success");
        }
        
        return Unit.Value;
    }
}