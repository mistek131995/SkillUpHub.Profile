using Microsoft.EntityFrameworkCore;

namespace SkillUpHub.Command.Infrastructure.Contexts;

public class PGContext(DbContextOptions<PGContext> options) : DbContext(options)
{
    public DbSet<Entities.Profile> Profiles { get; set; }
}