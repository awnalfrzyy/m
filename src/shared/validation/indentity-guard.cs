using diggie_server.src.infrastructure.persistence.repositories;

namespace diggie_server.src.shared.validation;

public static class IdentityGuard
{
    public static async Task EnsureRegistrationIsUnique(
        string email,
        string name,
        RepositoryUser repository)
    {
        if (await repository.ExistsByEmailAsync(email))
            throw new Exception("This email is already registered, please use another email");

        if (await repository.ExistsByNameAsync(name))
        {
            var existingNames = await repository.GetSimilarNamesAsync(name);
            var suggestions = GenerateSuggestions(name, existingNames);

            throw new Exception($"Name '{name}' is already taken. Try: {string.Join(", ", suggestions)}");
        }
    }

    private static List<string> GenerateSuggestions(string baseName, List<string> existing)
    {
        var suggestions = new List<string>();
        var random = new Random();

        while (suggestions.Count < 3)
        {
            string suggestion = $"{baseName}{random.Next(10, 99)}";

            if (!existing.Contains(suggestion) && !suggestions.Contains(suggestion))
            {
                suggestions.Add(suggestion);
            }
        }
        return suggestions;
    }
}