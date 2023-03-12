namespace Oxidize;

/// <summary>
///     Defines the error of the failed variant <see cref="Err{T}" /> of <see cref="IResult{T}" />
/// </summary>
public interface IError : IEquatable<IError>
{
    /// <summary>
    ///     Message describing the failure of an operation
    /// </summary>
    public string? Message { get; }

    /// <summary>
    ///     Optional inner <see cref="IError" />
    /// </summary>
    public IError? InternalError { get; }
}