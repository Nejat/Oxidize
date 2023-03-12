namespace Oxidize;

/// <summary>
///     Record that indicates an operation failed and contains a specific error of type <see cref="IError" />
/// </summary>
/// <param name="Error">an <see cref="IError" /> that describes the failure of an operation</param>
/// <typeparam name="T"><see cref="Type" /> of expected result</typeparam>
public record Err<T>(IError Error) : IResult<T>;