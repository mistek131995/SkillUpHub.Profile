using UUIDNext;

namespace SkillUpHub.Profile.Contract.Models;

public class Profile(Guid id, string firstName, string lastName, string description)
{
    public Guid Id { get; set; } = id;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Description { get; set; } = description;

    public Profile(string firstName, string lastName, string description) : 
        this(Uuid.NewDatabaseFriendly(Database.PostgreSql), firstName, lastName, description) { }
}