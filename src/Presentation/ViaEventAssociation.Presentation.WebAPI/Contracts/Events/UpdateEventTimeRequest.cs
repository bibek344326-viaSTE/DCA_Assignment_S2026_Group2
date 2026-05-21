namespace ViaEventAssociation.Presentation.WebAPI.Contracts.Events;

public sealed record UpdateEventTimeRequest(DateTime StartDateTime, DateTime EndDateTime);
