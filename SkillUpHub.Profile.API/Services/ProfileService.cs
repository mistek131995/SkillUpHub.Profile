using Grpc.Core;
using SkillUpHub.Profile.API;

namespace SkillUpHub.Profile.API.Services;

public class ProfileService : Profile.ProfileService.ProfileServiceBase
{
    public async override Task<GetProfileResponse> GetProfile(GetProfileRequest request, ServerCallContext context)
    {
        return new GetProfileResponse()
        {
            FirstName = "John",
        };
    }
}