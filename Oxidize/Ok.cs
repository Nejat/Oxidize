namespace Oxidize;

/// <summary>
///     Record that indicates an operation succeeded and contains a result of type <typeparamref name="T" />
/// </summary>
/// <param name="Result">result value of successful operation</param>
/// <typeparam name="T"><see cref="Type" /> of <paramref name="Result" /></typeparam>
public record Ok<T>(T Result) : IResult<T>;

/// <summary>
///     Record that indicates an operation succeeded
/// </summary>
/// <remarks>
///     Equivalent of a successful <see cref="Void" /> operation
/// </remarks>
public record Ok : Ok<Unit>
{
    /// <summary>
    ///     Default <see cref="Ok" /> <see cref="Unit" /> <see cref="IResult{T}" />
    /// </summary>
    public new static readonly Ok Result = new();

    /// <summary>
    ///     Limit instantiation of <see cref="Ok" /> <see cref="Unit" /> <see cref="IResult{T}" />
    /// </summary>
    private Ok() : base(Unit.Void)
    {
    }
}