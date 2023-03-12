namespace Oxidize;

/// <summary>
///     <see cref="Exception" /> when an <see cref="Err{T}" /> was expected, but the operation was successful
/// </summary>
public class ExpectedErrException : Exception
{
    /// <summary>
    ///     Instantiates an instance of <see cref="ExpectedErrException" /> containing a
    ///     descriptive <paramref name="message" /> and successful <paramref name="result" />
    /// </summary>
    /// <param name="message">descriptive text describing expected <see cref="Err{T}" /></param>
    /// <param name="result">value of successful operation <see cref="Ok{T}" /></param>
    public ExpectedErrException(string message, object? result) : base(message)
    {
        Result = result;
    }

    /// <summary>
    ///     Result of successful operation <see cref="Ok{T}" />
    /// </summary>
    public object? Result { get; }
}