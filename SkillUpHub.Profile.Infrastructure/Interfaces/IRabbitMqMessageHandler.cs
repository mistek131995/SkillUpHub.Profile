﻿namespace SkillUpHub.Profile.Infrastructure.Interfaces;

public interface IRabbitMqMessageHandler
{
    Task CreateDefaultUserProfileAsync(Guid guid);
}