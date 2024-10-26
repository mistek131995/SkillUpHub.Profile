using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillUpHub.Command.Infrastructure.Entities;

public class Profile
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(50)] 
    public string LastName { get; set; } = string.Empty;
    [MaxLength(512)]
    public string Description { get; set; } = string.Empty;
}