namespace IntegrationTests.Persistence.DmContextConfigurationTests;

public class DmContextConfigurationTests
{
    [Fact]
    public void DmContext_CanCreateSqliteSchema()
    {
        using var fixture = new SqliteTestFixture();

        Assert.True(fixture.Context.Database.CanConnect());
    }
}
