using SkillUpHub.Command.Contract.Repositories;

namespace SkillUpHub.Profile.Contract.Providers;

public interface IRepositoryProvider
{
    IProfileRepository ProfileRepository { get; }
}