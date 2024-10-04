namespace SkillUpHub.Profile.Contract.Services;

public interface IProfileService : IBaseService
{
    public record SaveProfileDTO(Guid UserId, string FirstName, string LastName, string Description);
    public Task SaveProfileAsync(SaveProfileDTO profileDTO);
    
    public record GetProfileDTO(string FirstName, string LastName, string Description);
    public Task<GetProfileDTO> GetProfileAsync(Guid userId);
}