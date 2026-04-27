using UnitTests.Fakes;
using ViaEventAssociation.Core.AppEntry.Commands.Guest;
using ViaEventAssociation.Core.Application.CommandHandlers.Guest;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Guests.RegisterGuests;

public class RegisterGuestCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ValidData_AddsGuest_AndSaves()
    {
        var repo = new FakeGuestRepository();
        var uow = new FakeUnitOfWork();

        var command = RegisterGuestCommand.Create("abc@via.dk", "john", "doe", "https://example.com/pic.jpg").Payload!;
        var handler = new RegisterGuestCommandHandler(repo, uow);

        var result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        Assert.Single(repo.Values);
        Assert.Equal(1, uow.SaveChangesCallCount);

        var guest = repo.Values[0];
        Assert.Equal("abc@via.dk", guest.Id.Value);
        Assert.Equal("John", guest.FirstName);
        Assert.Equal("Doe", guest.LastName);
    }

    [Fact]
    public async Task HandleAsync_EmailAlreadyRegistered_ReturnsFailure_AndDoesNotSave()
    {
        var repo = new FakeGuestRepository();
        var uow = new FakeUnitOfWork();

        var existingGuest = Guest.Create("abc@via.dk", "John", "Doe", "https://example.com/pic.jpg").Payload!;
        await repo.AddAsync(existingGuest);

        var command = RegisterGuestCommand.Create("abc@via.dk", "Jane", "Smith", "https://example.com/pic2.jpg").Payload!;
        var handler = new RegisterGuestCommandHandler(repo, uow);

        var result = await handler.HandleAsync(command);

        Assert.True(result.IsFailure);
        var failure = Assert.IsType<Failure<None>>(result);
        Assert.Contains(failure.Errors, e => e.Code == "EMAIL_ALREADY_REGISTERED");
        Assert.Equal(0, uow.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_InvalidProfilePictureUrl_ReturnsFailure_AndDoesNotSave()
    {
        var repo = new FakeGuestRepository();
        var uow = new FakeUnitOfWork();

        var command = RegisterGuestCommand.Create("abc@via.dk", "John", "Doe", "not-a-url").Payload!;
        var handler = new RegisterGuestCommandHandler(repo, uow);

        var result = await handler.HandleAsync(command);

        Assert.True(result.IsFailure);
        var failure = Assert.IsType<Failure<None>>(result);
        Assert.Contains(failure.Errors, e => e.Code == Error.InvalidProfilePictureUrl.Code);
        Assert.Equal(0, uow.SaveChangesCallCount);
    }
}
