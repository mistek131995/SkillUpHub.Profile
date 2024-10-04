using Microsoft.EntityFrameworkCore;

namespace SkillUpHub.Profile.Infrastructure.Contexts;

public class PGContext(DbContextOptions<PGContext> options) : DbContext(options)
{
    public DbSet<Entities.Profile> Profiles { get; set; }
}