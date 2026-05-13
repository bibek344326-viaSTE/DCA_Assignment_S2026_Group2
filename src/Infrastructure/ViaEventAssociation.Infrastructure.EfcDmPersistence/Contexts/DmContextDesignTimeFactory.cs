using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ViaEventAssociation.Infrastructure.EfcDmPersistence.Contexts;

public class DmContextDesignTimeFactory : IDesignTimeDbContextFactory<DmContext>
{
    public DmContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<DmContext>()
            .UseSqlite("Data Source=via-event-association-write-model.db")
            .Options;

        return new DmContext(options);
    }
}
