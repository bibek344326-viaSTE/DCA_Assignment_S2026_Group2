namespace ViaEventAssociation.Core.Tools.OperationResult;

public abstract record Result
{
    public static Result<T> Success<T>(T value) => new Success<T>(value);

    public static Result<None> Success() => new Success<None>(new None());

    public static Result<T> Failure<T>(ResultError error) => new Failure<T>([error]);

    public static Result<T> Failure<T>(IEnumerable<ResultError> errors) => new Failure<T>(errors);

    public static Result<T> Combine<T>(Result<T> primary, params Result<None>[] others)
    {
        var errors = new List<ResultError>();

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
        var errors = new List<ResultError>();

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

    public static implicit operator Result<T>(T value) => new Success<T>(value);

    public static implicit operator Result<T>(ResultError error) => new Failure<T>([error]);

    public static implicit operator Result<T>(ResultError[] errors) => new Failure<T>(errors);
}

public record Success<T>(T Value) : Result<T>;

public record Failure<T>(IEnumerable<ResultError> Errors) : Result<T>;
