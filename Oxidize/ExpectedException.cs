namespace Oxidize;

/// <summary>
///     <see cref="Exception" /> when an <see cref="Ok{T}" /> was expected, but the operation failed
/// </summary>
public class ExpectedException : Exception
{
    /// <summary>
    ///     Instantiates an instance of <see cref="ExpectedException" /> containing a descriptive
    ///     <paramref name="message" /> and the <paramref name="error" />
    /// </summary>
    /// <param name="message">descriptive text describing expected <see cref="Ok{T}" /></param>
    /// <param name="error"><see cref="IError" /> caused by failed operation <see cref="Err{T}" /></param>
    public ExpectedException(string message, IError error) : base(message)
    {
        Error = error;
    }

    /// <summary>
    ///     <see cref="IError" /> caused by failed operation <see cref="Err{T}" />
    /// </summary>
    public IError Error { get; }
}