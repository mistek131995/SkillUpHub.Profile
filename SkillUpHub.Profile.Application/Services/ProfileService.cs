using SkillUpHub.Profile.Contract.Providers;
using SkillUpHub.Profile.Contract.Services;

namespace SkillHub.Profile.Application.Services;

public class ProfileService(IRepositoryProvider repositoryProvider) : IProfileService
{
    public async Task SaveProfile(IProfileService.ProfileDTO profileDTO)
    {
        var profile = await repositoryProvider.ProfileRepository.GetByUserIdAsync(profileDTO.UserId) ??
                      new SkillUpHub.Profile.Contract.Models.Profile(
                          profileDTO.FirstName, 
                          profileDTO.LastName,
                          profileDTO.Description);

        await repositoryProvider.ProfileRepository.SaveAsync(profile);
    }
}