using Bogus;
using ReadFlow.DAL.Entities;
using ReadFlow.DAL.Enums;

namespace ReadFlow.DAL.Tests.Builders;

public class UserBuilder
{
    private readonly Faker<User> _faker;

    public UserBuilder()
    {
        _faker = new Faker<User>()
            .RuleFor(u => u.Id, _ => Guid.NewGuid())
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.PasswordHash, "$2a$11$dummyhashfortest1234567890123456789012")
            .RuleFor(u => u.DisplayName, f => f.Name.FullName())
            .RuleFor(u => u.Bio, f => f.Lorem.Sentence(10))
            .RuleFor(u => u.ProfilePictureUrl, f => f.Internet.Avatar())
            .RuleFor(u => u.Role, UserRole.User)
            .RuleFor(u => u.CreatedAt, _ => DateTime.UtcNow)
            .RuleFor(u => u.UpdatedAt, _ => DateTime.UtcNow);
    }

    public User Build() => _faker.Generate();

    public List<User> Build(int count) => _faker.Generate(count);

    public UserBuilder WithUsername(string username)
    {
        _faker.RuleFor(u => u.Username, username);
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _faker.RuleFor(u => u.Email, email);
        return this;
    }

    public UserBuilder WithDisplayName(string displayName)
    {
        _faker.RuleFor(u => u.DisplayName, displayName);
        return this;
    }

    public UserBuilder AsAdmin()
    {
        _faker.RuleFor(u => u.Role, UserRole.Admin);
        return this;
    }

    public UserBuilder AsModerator()
    {
        _faker.RuleFor(u => u.Role, UserRole.Moderator);
        return this;
    }

    public UserBuilder AsUser()
    {
        _faker.RuleFor(u => u.Role, UserRole.User);
        return this;
    }
}
