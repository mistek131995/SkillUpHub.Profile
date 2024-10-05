using System.Security.Claims;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using SkillHub.Profile.Application.Interfaces;
using IServiceProvider = SkillHub.Profile.Application.Interfaces.IServiceProvider;

namespace SkillUpHub.Profile.API.Services;

public class ProfileService(IServiceProvider serviceProvider) : Profile.ProfileService.ProfileServiceBase
{
    [Authorize]
    public override async Task<GetProfileResponse> GetProfile(Empty empty, ServerCallContext context)
    {
        var httpContext = context.GetHttpContext();
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                     throw new Exception("Не удалось получить Id пользователя");
        
        var profile = await serviceProvider.ProfileService.GetProfileAsync(Guid.Parse(userId));
        
        return new GetProfileResponse()
        {
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Description = profile.Description,
        };
    }

    [Authorize]
    public override async Task<SaveProfileResponse> SaveProfile(SaveProfileRequest request, ServerCallContext context)
    {
        var httpContext = context.GetHttpContext();
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                     throw new Exception("Не удалось получить Id пользователя");
        
        await serviceProvider.ProfileService.SaveProfileAsync(new IProfileService.SaveProfileDTO(
            Guid.Parse(userId), 
            request.FirstName, 
            request.LastName, 
            request.Description));

        return new SaveProfileResponse()
        {
            IsSuccess = true
        };
    }
}