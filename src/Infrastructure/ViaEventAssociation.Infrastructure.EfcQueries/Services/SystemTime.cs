using ViaEventAssociation.Core.Domain.Contracts;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Services;

public sealed class SystemTime : ISystemTime
{
    public DateTime Now => DateTime.Now;
}
