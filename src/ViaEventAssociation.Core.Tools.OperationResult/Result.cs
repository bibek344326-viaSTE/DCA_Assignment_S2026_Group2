namespace ViaEventAssociation.Core.Tools.OperationResult;

public abstract record Result
{
    public static Result<T> Success<T>(T value) => new Success<T>(value);

    public static Result<None> Success() => new Success<None>(new None());

    public static Result<T> Failure<T>(Error error) => new Failure<T>([error]);

    public static Result<T> Failure<T>(IEnumerable<Error> errors) => new Failure<T>(errors);

    public static Result<T> Combine<T>(Result<T> primary, params Result<None>[] others)
    {
        var errors = new List<Error>();

        if (primary is Failure<T> primaryFailure)
            errors.AddRange(primaryFailure.Errors);

        foreach (var other in others)
        {
            if (other is Failure<None> f)
                errors.AddRange(f.Errors);
        }

        return errors.Count > 0
            ? new Failure<T>(errors)
            : primary;
    }

    public static Result<None> Combine(params Result<None>[] results)
    {
        var errors = new List<Error>();

        foreach (var result in results)
        {
            if (result is Failure<None> f)
                errors.AddRange(f.Errors);
        }

        return errors.Count > 0
            ? new Failure<None>(errors)
            : new Success<None>(new None());
    }
}

public abstract record Result<T> : Result
{
    public bool IsSuccess => this is Success<T>;

    public bool IsFailure => this is Failure<T>;

    public Error Error => this is Failure<T> failure
        ? failure.Errors.First()
        : throw new InvalidOperationException("Result does not contain errors.");

    public static implicit operator Result<T>(T value) => new Success<T>(value);

    public static implicit operator Result<T>(Error error) => new Failure<T>([error]);

    public static implicit operator Result<T>(Error[] errors) => new Failure<T>(errors);
}

public record Success<T>(T Value) : Result<T>;

public record Failure<T>(IEnumerable<Error> Errors) : Result<T>;
