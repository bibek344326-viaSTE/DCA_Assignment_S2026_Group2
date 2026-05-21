using System.Diagnostics.CodeAnalysis;

namespace IntegrationTests.Queries;

internal static class QueryTestData
{
    private static readonly string? SeedDirectoryValue = FindSeedDirectory();

    public static string? SeedDirectory => SeedDirectoryValue;

    public static bool TryGetSeedDirectory([NotNullWhen(true)] out string? seedDirectory)
    {
        if (SeedDirectoryValue is not null)
        {
            seedDirectory = SeedDirectoryValue;
            return true;
        }

        seedDirectory = null;
        return false;
    }

    private static string? FindSeedDirectory()
    {
        var current = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (current is not null)
        {
            var candidate = Path.Combine(current.FullName, "Assignments", "Assignment8", "ViaEventAssociation");
            if (File.Exists(Path.Combine(candidate, "Events.json")))
                return candidate;

            current = current.Parent;
        }

        return null;
    }
}
