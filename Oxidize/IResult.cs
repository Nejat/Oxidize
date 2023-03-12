namespace Oxidize;

/// <summary>
///     Result interface that represents an <see cref="Ok{T}" />
///     or an <see cref="Err{T}" /> result record of an operation
/// </summary>
/// <typeparam name="T">
///     <see cref="Type" /> of <see cref="Ok{T}" /> value expected from a successful operation
/// </typeparam>
public interface IResult<T>
{
    /// <summary>
    ///     <see cref="Type" /> of expected result from a successful operation
    /// </summary>
    public static Type ExpectedResultType { get; } = typeof(T);
}