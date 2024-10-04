namespace SkillUpHub.Profile.Contract.Repositories;

public interface IProfileRepository : IBaseRepository
{
    Task<Models.Profile?> GetByUserIdAsync(Guid userId);
    Task<Models.Profile> SaveAsync(Models.Profile profile);
}