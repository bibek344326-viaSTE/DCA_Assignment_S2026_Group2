using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.OperationalResult;

public class ResultTests
{
    private static readonly ResultError SampleError = new("ERR_001", "Something went wrong");
    private static readonly ResultError AnotherError = new("ERR_002", "Another failure");

    // --- Success creation ---

    [Fact]
    public void Success_WithValue_ReturnsSuccessResult()
    {
        var result = Result.Success(42);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        var success = Assert.IsType<Success<int>>(result);
        Assert.Equal(42, success.Value);
    }

    [Fact]
    public void Success_WithoutValue_ReturnsSuccessNoneResult()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.IsType<Success<None>>(result);
    }

    // --- Failure creation ---

    [Fact]
    public void Failure_WithSingleError_ReturnsFailureResult()
    {
        var result = Result.Failure<int>(SampleError);

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        var failure = Assert.IsType<Failure<int>>(result);
        Assert.Single(failure.Errors);
        Assert.Equal("ERR_001", failure.Errors.First().Code);
    }

    [Fact]
    public void Failure_WithMultipleErrors_ReturnsAllErrors()
    {
        var errors = new[] { SampleError, AnotherError };
        var result = Result.Failure<string>(errors);

        var failure = Assert.IsType<Failure<string>>(result);
        Assert.Equal(2, failure.Errors.Count());
    }

    // --- Implicit operators ---

    [Fact]
    public void ImplicitOperator_FromValue_CreatesSuccess()
    {
        Result<string> result = "hello";

        Assert.IsType<Success<string>>(result);
        Assert.Equal("hello", ((Success<string>)result).Value);
    }

    [Fact]
    public void ImplicitOperator_FromError_CreatesFailure()
    {
        Result<int> result = SampleError;

        var failure = Assert.IsType<Failure<int>>(result);
        Assert.Single(failure.Errors);
        Assert.Equal("ERR_001", failure.Errors.First().Code);
    }

    [Fact]
    public void ImplicitOperator_FromErrorArray_CreatesFailure()
    {
        Result<int> result = new[] { SampleError, AnotherError };

        var failure = Assert.IsType<Failure<int>>(result);
        Assert.Equal(2, failure.Errors.Count());
    }

    [Fact]
    public void ImplicitOperator_FromNone_CreatesSuccess()
    {
        Result<None> result = new None();

        Assert.IsType<Success<None>>(result);
    }

    // --- Pattern matching ---

    [Fact]
    public void PatternMatching_CanDistinguishSuccessAndFailure()
    {
        Result<int> success = Result.Success(10);
        Result<int> failure = Result.Failure<int>(SampleError);

        var successMessage = success switch
        {
            Success<int> s => $"Value: {s.Value}",
            Failure<int> f => $"Error: {f.Errors.First().Message}",
            _ => "Unknown"
        };

        var failureMessage = failure switch
        {
            Success<int> s => $"Value: {s.Value}",
            Failure<int> f => $"Error: {f.Errors.First().Message}",
            _ => "Unknown"
        };

        Assert.Equal("Value: 10", successMessage);
        Assert.Equal("Error: Something went wrong", failureMessage);
    }

    // --- Combine ---

    [Fact]
    public void Combine_AllSuccess_ReturnsSuccess()
    {
        var r1 = Result.Success();
        var r2 = Result.Success();

        var combined = Result.Combine(r1, r2);

        Assert.True(combined.IsSuccess);
    }

    [Fact]
    public void Combine_SomeFailures_ReturnsAllErrors()
    {
        var r1 = Result.Success();
        var r2 = Result.Failure<None>(SampleError);
        var r3 = Result.Failure<None>(AnotherError);

        var combined = Result.Combine(r1, r2, r3);

        var failure = Assert.IsType<Failure<None>>(combined);
        Assert.Equal(2, failure.Errors.Count());
    }

    [Fact]
    public void Combine_WithPrimaryValue_AllSuccess_ReturnsPrimary()
    {
        var primary = Result.Success(42);
        var other = Result.Success();

        var combined = Result.Combine(primary, other);

        var success = Assert.IsType<Success<int>>(combined);
        Assert.Equal(42, success.Value);
    }

    [Fact]
    public void Combine_WithPrimaryValue_SomeFailures_ReturnsAllErrors()
    {
        var primary = Result.Failure<int>(SampleError);
        var other = Result.Failure<None>(AnotherError);

        var combined = Result.Combine(primary, other);

        var failure = Assert.IsType<Failure<int>>(combined);
        Assert.Equal(2, failure.Errors.Count());
    }

    // --- ResultError ---

    [Fact]
    public void ResultError_HasCodeAndMessage()
    {
        var error = new ResultError("NOT_FOUND", "Item was not found");

        Assert.Equal("NOT_FOUND", error.Code);
        Assert.Equal("Item was not found", error.Message);
    }

    [Fact]
    public void ResultError_ValueEquality_WorksCorrectly()
    {
        var error1 = new ResultError("ERR", "msg");
        var error2 = new ResultError("ERR", "msg");

        Assert.Equal(error1, error2);
    }

    // --- IsSuccess / IsFailure properties ---

    [Fact]
    public void IsSuccess_OnSuccess_ReturnsTrue()
    {
        Result<int> result = 42;

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void IsFailure_OnFailure_ReturnsTrue()
    {
        Result<int> result = SampleError;

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
    }
}
