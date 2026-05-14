namespace IntegrationTests.Queries;

internal static class QueryTestData
{
    public static string SeedDirectory => FindSeedDirectory();

    private static string FindSeedDirectory()
    {
        var current = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (current is not null)
        {
            var candidate = Path.Combine(current.FullName, "Assignments", "Assignment8", "ViaEventAssociation");
            if (File.Exists(Path.Combine(candidate, "Events.json")))
                return candidate;

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("Could not find Assignments/Assignment8/ViaEventAssociation.");
    }
}
