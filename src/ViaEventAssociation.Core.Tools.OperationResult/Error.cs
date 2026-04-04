namespace ViaEventAssociation.Core.Tools.OperationResult;

public record Error(string Code, string Message)
{
    public static Error NullString => new("NULL_STRING", "The provided string is null.");
    public static Error BlankString => new("BLANK_STRING", "The provided string is blank.");

    public static Error TooShortTitle(int minTitleLength) =>
        new("TITLE_TOO_SHORT", $"The provided title is too short, minimum length is {minTitleLength} characters.");

    public static Error TooLongTitle(int maxTitleLength) =>
        new("TITLE_TOO_LONG", $"The provided title is too long, maximum length is {maxTitleLength} characters.");

    public static Error TooLongDescription(int maxDescriptionLength) =>
        new("DESCRIPTION_TOO_LONG", $"The provided description is too long, maximum length is {maxDescriptionLength} characters.");

    public static Error EventStatusIsActive => new("EVENT_STATUS_ACTIVE", "Active events cannot be modified.");
    public static Error EventStatusIsCanceled => new("EVENT_STATUS_CANCELED", "Cancelled events cannot be modified.");
    
    public static Error InvalidDateTimeRange => new("INVALID_DATE_TIME_RANGE", "Start date and time must be before end date and time.");
    
    public static Error DurationTooShort => new ("DURATION_TOO_SHORT", "The duration of the event is too short, it must be 1 hour or longer.");
    
    public static Error InvalidStartDateTime=> new ("START_TIME_TOO_EARLY", "The start time {start} is invalid. Rooms are usable from 08 am on a day, to 01 am on the next day.");
    
    public static Error InvalidEndDateTime => new ("END_TIME_TOO_LATE", "The end time {end} is invalid. Rooms are usable from 08 am on a day, to 01 am on the next day.");
    
    public static Error DurationTooLong => new ("INVALID_DURATION", "The duration of the event is invalid, maximum duration is {maxDuration}, start: {start}, end: {end}.");
    
    public static Error EventStartTimeInThePast => new ("EVENT_START_TIME_IN_PAST", "The start time of the event cannot be in the past."); 
    
    public static Error TooFewGuests(int minGuests) =>
        new("TOO_FEW_GUESTS",
            $"The maximum number of guests must be at least {minGuests}.");

    public static Error TooManyGuests(int maxGuests) =>
        new("TOO_MANY_GUESTS",
            $"The maximum number of guests cannot exceed {maxGuests}.");

    public static Error EventStatusIsActiveAndMaxGuestsReduced =>
        new("ACTIVE_EVENT_GUESTS_REDUCED",
            "Maximum number of guests of an active event cannot be reduced (it may only be increased).");

    public static Error TooManyGuestsForLocation(int maxCapacity) =>
        new("TOO_MANY_GUESTS_FOR_LOCATION",
            $"The selected number of guests exceeds the location capacity of {maxCapacity} people.");
    
    public static Error EventTitleIsDefault =>
        new("EVENT_TITLE_DEFAULT", "The event title must be changed from the default title.");

    public static Error EventDescriptionMissing =>
        new("EVENT_DESCRIPTION_MISSING", "The event description must be set before the event can be made ready.");

    public static Error EventDateTimeMissing =>
        new("EVENT_DATE_TIME_MISSING", "The event start and end date/time must be set before the event can be made ready.");

    public static Error EventVisibilityMissing =>
        new("EVENT_VISIBILITY_MISSING", "The event visibility must be set before the event can be made ready.");

    public static Error InvalidMaxGuestsRange =>
        new("INVALID_MAX_GUESTS_RANGE", "Maximum number of guests must be between 5 and 50.");
    
    public static Error EventTimeIsInThePast =>
        new("EVENT_TIME_IN_PAST", "Event start time cannot be in the past.");

    public static Error InvalidEmail =>
        new("INVALID_EMAIL", "The provided email address is not valid.");

    public static Error TooLongEmail(int maxEmailLength) =>
        new("EMAIL_TOO_LONG", $"The provided email is too long, maximum length is {maxEmailLength} characters.");
    
    public static Error EmailMustBeVia =>
        new("EMAIL_MUST_BE_VIA", "The email must end with @via.dk.");

    public static Error InvalidViaEmailIdentifier =>
        new("INVALID_VIA_EMAIL_IDENTIFIER", "The email identifier must be either 3 or 4 letters, or 6 digits.");

    public static Error FirstNameTooShort(int minLength) =>
        new("FIRST_NAME_TOO_SHORT", $"First name must be at least {minLength} characters long.");

    public static Error FirstNameTooLong(int maxLength) =>
        new("FIRST_NAME_TOO_LONG", $"First name cannot be longer than {maxLength} characters.");

    public static Error LastNameTooShort(int minLength) =>
        new("LAST_NAME_TOO_SHORT", $"Last name must be at least {minLength} characters long.");

    public static Error LastNameTooLong(int maxLength) =>
        new("LAST_NAME_TOO_LONG", $"Last name cannot be longer than {maxLength} characters.");

    public static Error InvalidFirstName =>
        new("INVALID_FIRST_NAME", "First name must contain only letters a-z.");

    public static Error InvalidLastName =>
        new("INVALID_LAST_NAME", "Last name must contain only letters a-z.");

    // NEW ERRORS FOR EVENT PARTICIPATION (ID: 11)
    public static Error EventNotActive =>
        new("EVENT_NOT_ACTIVE", "Only active events can be joined.");

    public static Error EventIsFull =>
        new("EVENT_IS_FULL", "The event has reached maximum capacity. There is no more room.");

    public static Error EventHasStarted =>
        new("EVENT_HAS_STARTED", "Cannot join an event that has already started. Only future events can be participated.");

    public static Error EventIsPrivate =>
        new("EVENT_IS_PRIVATE", "Only public events can be participated without invitation.");

    public static Error GuestAlreadyParticipating =>
        new("GUEST_ALREADY_PARTICIPATING", "You are already registered for this event. A guest cannot take two slots at an event.");

    public static Error EventNotFound =>
        new("EVENT_NOT_FOUND", "The specified event could not be found.");

    public static Error GuestNotFound =>
        new("GUEST_NOT_FOUND", "The specified guest could not be found.");

    public static Error EventNotReadyOrActive =>
        new("EVENT_NOT_READY_OR_ACTIVE", "Guest can only be invited to an event that is Ready or Active.");

    public static Error GuestAlreadyInvited =>
        new("GUEST_ALREADY_INVITED", "Guest is already invited to this event.");

    public static Error InvitationNotFound =>
        new("INVITATION_NOT_FOUND", "There is no pending invitation for this guest.");

    public static Error EventCancelled =>
        new("EVENT_CANCELLED", "Cannot accept invitation for a cancelled event.");
}