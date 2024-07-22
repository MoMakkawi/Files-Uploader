using Bogus;

using Domain.Identities;

namespace Application.Helper;

internal static class FakeDataGenerator
{
    public static IEnumerable<User> GenerateUsers(int count = 5)
    {
        // Create a new Faker instance for a user
        var userFaker = new Faker<User>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(p => p.UserName, f => f.Name.FullName())
            .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.Password, f => f.Name.Random.Words());

        return Enumerable.Range(0, count)
            .Select(p => userFaker.Generate());
    }
}
