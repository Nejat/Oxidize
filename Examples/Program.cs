using Oxidize;
using static System.Console;

#pragma warning disable CS8509

var message = ValidateUniversalAnswer(42) switch
{
    Ok<int> ok => $"Perfect: {ok.Result}",
    Err<int> err =>
        err.Error switch
        {
            TooSmall small => $"Not quire right; {small.Message}",
            TooBig big => $"Not so perfect either: {big.Message}",
        },
};

WriteLine(message);

IResult<int> ValidateUniversalAnswer(int value)
{
    const int minimum = 42;
    const int maximum = 42;

    return value switch
    {
        < minimum => new Err<int>(new TooSmall(value, minimum)),
        > maximum => new Err<int>(new TooBig(value, maximum)),
        _ => new Ok<int>(value)
    };
}

public record TooSmall(long Value, long Minimum) : IError
{
    public string? Message { get; } = $"{Value} is too small, it cannot be less than {Minimum}";

    public IError? InternalError { get; } = default;

    public bool Equals(IError? error)
    {
        if (error is not TooSmall other) return false;
        if (ReferenceEquals(this, other)) return true;

        return Message == other.Message &&
               Equals(InternalError, other.InternalError) &&
               Value == other.Value &&
               Minimum == other.Minimum;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Message, InternalError, Value, Minimum);
    }
}

public sealed record TooBig(long Value, long Maximum) : IError
{
    public string? Message { get; } = $"{Value} is too big, it cannot be greater than {Maximum}";

    public IError? InternalError { get; } = default;

    public bool Equals(IError? error)
    {
        if (error is not TooBig other) return false;
        if (ReferenceEquals(this, other)) return true;

        return Message == other.Message &&
               Equals(InternalError, other.InternalError) &&
               Value == other.Value &&
               Maximum == other.Maximum;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Message, InternalError, Value, Maximum);
    }
}