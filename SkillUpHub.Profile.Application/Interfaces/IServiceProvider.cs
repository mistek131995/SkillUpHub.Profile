namespace SkillHub.Profile.Application.Interfaces;

public interface IServiceProvider
{
    IProfileService ProfileService { get; }
}