using SkillUpHub.Profile.Contract.Repositories;

namespace SkillUpHub.Profile.Contract.Providers;

public interface IRepositoryProvider
{
    IProfileRepository ProfileRepository { get; }
}