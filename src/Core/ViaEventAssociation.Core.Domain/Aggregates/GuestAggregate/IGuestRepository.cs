using ViaEventAssociation.Core.Domain.Common.Repository;

namespace ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;

public interface IGuestRepository: IRepository<Guest, Email>
{
    
}