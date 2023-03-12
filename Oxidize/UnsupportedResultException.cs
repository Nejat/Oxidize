namespace Oxidize;

/// <summary>
///     <see cref="Exception"/> when <see cref="ResultExtensions"/> utility encounters
///     an unsupported implementation of <see cref="IResult{T}"/>
/// </summary>
public class UnsupportedResultException : Exception
{
    /// <summary>
    ///     Instantiates an instance of <see cref="UnsupportedResultException"/>
    ///     containing the <paramref name="unsupportedType"/>
    /// </summary>
    /// <param name="unsupportedType"></param>
    public UnsupportedResultException(Type unsupportedType)
        : base($"{unsupportedType.Name} is not a supported IResult<T> type by {nameof(ResultExtensions)} utilities")
    {
        UnsupportedType = unsupportedType;
    }

    /// <summary>
    ///     <see cref="Type"/> that caused the exception
    /// </summary>
    public Type UnsupportedType { get; }
}