using UUIDNext;

namespace SkillUpHub.Command.Contract.Models;

public class Profile(Guid id, Guid userId, string firstName, string lastName, string description)
{
    public Guid Id { get; set; } = id;
    public Guid UserId { get; set; } = userId;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Description { get; set; } = description;

    public Profile(Guid userId, string firstName, string lastName, string description) : 
        this(Uuid.NewDatabaseFriendly(Database.PostgreSql), userId, firstName, lastName, description) { }
}