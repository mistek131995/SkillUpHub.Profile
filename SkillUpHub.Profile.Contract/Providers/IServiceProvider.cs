using SkillUpHub.Profile.Contract.Services;

namespace SkillUpHub.Profile.Contract.Providers;

public interface IServiceProvider
{
    IProfileService ProfileService { get; }
}