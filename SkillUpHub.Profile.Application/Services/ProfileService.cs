using SkillUpHub.Profile.Contract.Providers;
using SkillUpHub.Profile.Contract.Services;

namespace SkillHub.Profile.Application.Services;

public class ProfileService(IRepositoryProvider repositoryProvider) : IProfileService
{
    public async Task SaveProfileAsync(IProfileService.SaveProfileDTO profileDTO)
    {
        var profile = await repositoryProvider.ProfileRepository.GetByUserIdAsync(profileDTO.UserId) ??
                      new SkillUpHub.Profile.Contract.Models.Profile(
                          profileDTO.FirstName, 
                          profileDTO.LastName,
                          profileDTO.Description);

        await repositoryProvider.ProfileRepository.SaveAsync(profile);
    }

    public async Task<IProfileService.GetProfileDTO> GetProfileAsync(Guid userId)
    {
        var profile = await repositoryProvider.ProfileRepository.GetByUserIdAsync(userId) ?? 
                      throw new Exception("Профиль не найден");
        
        return new IProfileService.GetProfileDTO(profile.FirstName, profile.LastName, profile.Description);
    }
}