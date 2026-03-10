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
}