namespace SkillUpHub.Profile.Contract.Services;

public interface IProfileService : IBaseService
{
    public record ProfileDTO(Guid UserId, string FirstName, string LastName, string Description);
    public Task SaveProfile(ProfileDTO profileDTO);
}