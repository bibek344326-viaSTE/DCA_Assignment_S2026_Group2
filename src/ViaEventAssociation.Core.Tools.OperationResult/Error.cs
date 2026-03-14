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
}