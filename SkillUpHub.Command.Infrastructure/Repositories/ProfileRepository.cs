using Microsoft.EntityFrameworkCore;
using SkillUpHub.Command.Contract.Repositories;
using SkillUpHub.Command.Infrastructure.Contexts;
using SkillUpHub.Command.Infrastructure.Mappers;

namespace SkillUpHub.Command.Infrastructure.Repositories;

public class ProfileRepository(PGContext context) : IProfileRepository
{
    public async Task<Command.Contract.Models.Profile?> GetByUserIdAsync(Guid userId)
    {
        var mapper = new ProfileMapper();
        var profile = await context.Profiles.FirstOrDefaultAsync(x => x.UserId == userId);
        
        return profile == null ? null : mapper.MappingToContractModel(profile);
    }

    public async Task<Command.Contract.Models.Profile> SaveAsync(Command.Contract.Models.Profile profile)
    {
        var mapper = new ProfileMapper();
        var dbProfile = mapper.MappingToInfrastructureModel(profile);
        
        var existProfile = await context.Profiles.FirstOrDefaultAsync(x => x.UserId == profile.Id);
        
        if(existProfile != null)
            context.Entry(existProfile).CurrentValues.SetValues(dbProfile);
        else
            context.Profiles.Add(dbProfile);

        await context.SaveChangesAsync();

        return profile;
    }
}