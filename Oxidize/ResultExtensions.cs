using static System.String;

// todo: the following pragma and comment would apply if c# interfaces can implementation visibility
// all switches define all expected paths, verified in testing,
// all other paths are unsanctioned so a default path for switches
// in this library for these extensions are unnecessary
// #pragma warning disable CS8509

namespace Oxidize;

/// <summary>
///     A collection of extension methods for working with <see cref="IResult{T}" /> values
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    ///     Evaluates to the <paramref name="rhs" /> <see cref="IResult{T}" />
    ///     if the <paramref name="lhs" /> is <see cref="Oxidize.Ok{T}" />
    /// </summary>
    /// <param name="lhs">left hand instance of <see cref="IResult{T}" /></param>
    /// <param name="rhs">
    ///     right hand instance of <see cref="IResult{T}" /> resultant,
    ///     if <paramref name="lhs" /> is <see cref="Oxidize.Ok{T}" />
    /// </param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>
    ///     returns <paramref name="rhs" /> if <paramref name="lhs" /> is <see cref="Oxidize.Ok{T}" />,
    ///     otherwise it returns the <see cref="Oxidize.Err{T}" /> of <paramref name="lhs" />
    /// </returns>
    /// <remarks>
    ///     Arguments are eagerly evaluated; use <see cref="AndThen{T}" /> for lazy evaluation
    /// </remarks>
    public static IResult<T> And<T>(this IResult<T> lhs, IResult<T> rhs)
    {
        return lhs switch
        {
            Ok<T> => rhs,
            Err<T> => lhs,
            _ => throw new UnsupportedResultException(lhs.GetType())
        };
    }

    /// <summary>
    ///     Lazily evaluates to the <paramref name="rhs" /> <see cref="IResult{T}" />
    ///     if the <paramref name="lhs" /> is <see cref="Oxidize.Ok{T}" />
    /// </summary>
    /// <param name="lhs">left hand instance of <see cref="IResult{T}" /></param>
    /// <param name="rhs">
    ///     right hand instance of <see cref="IResult{T}" /> resultant,
    ///     if <paramref name="lhs" /> is <see cref="Oxidize.Ok{T}" />
    /// </param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>
    ///     returns <paramref name="rhs" /> if <paramref name="lhs" /> is <see cref="Oxidize.Ok{T}" />,
    ///     otherwise it returns the <see cref="Oxidize.Err{T}" /> of <paramref name="lhs" />
    /// </returns>
    public static IResult<T> AndThen<T>(this IResult<T> lhs, Func<T, IResult<T>> rhs)
    {
        return lhs switch
        {
            Ok<T> ok => rhs(ok.Result),
            Err<T> => lhs,
            _ => throw new UnsupportedResultException(lhs.GetType())
        };
    }

    /// <summary>
    ///     Checks if an instance of <see cref="IResult{T}" /> contains
    ///     a <paramref name="value" /> <typeparamref name="T" />
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <param name="value">value <typeparamref name="T" /> to check</param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>
    ///     Returns a true if the <see cref="IResult{T}" /> is <see cref="Oxidize.Ok{T}" />
    ///     and the <paramref name="value" /> matches the result, false otherwise
    /// </returns>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static bool Contains<T>(this IResult<T> result, T value)
        where T : IEquatable<T>
    {
        return result switch
        {
            Ok<T> ok => ok.Result.Equals(value),
            Err<T> => false,
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }

    /// <summary>
    ///     Checks if an instance of <see cref="IResult{T}" /> contains
    ///     a <paramref name="error" /> <see cref="IError" />
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <param name="error">error <see cref="IError" /> to check</param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <typeparam name="TError">
    ///     <see cref="IError" />, required to implement <see cref="IEquatable{IError}" />
    /// </typeparam>
    /// <returns>
    ///     Returns a true if the <see cref="IResult{T}" /> is <see cref="Oxidize.Err{T}" />
    ///     and the <paramref name="error" /> matches the result, false otherwise
    /// </returns>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static bool ContainsErr<T, TError>(this IResult<T> result, TError error)
        where TError : IError, IEquatable<IError>
    {
        return result switch
        {
            Ok<T> => false,
            Err<T> err => error.Equals(err.Error),
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }

    /// <summary>
    ///     Expects <paramref name="result" /> to be an instance of
    ///     <see cref="Oxidize.Ok{T}" />, retrieves it's value
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <param name="expected">a <see cref="string" /> message of what you expect</param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>
    ///     Returns a value <typeparamref name="T" /> if <paramref name="result" />
    ///     is an instance of <see cref="Oxidize.Ok{T}" />, otherwise it
    ///     throws an <see cref="ExpectedException" />
    /// </returns>
    /// <exception cref="ExpectedException">
    ///     Thrown if <paramref name="result" /> is an instance of <see cref="Oxidize.Err{T}" />
    /// </exception>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static T Expect<T>(this IResult<T> result, string? expected = default)
    {
        return result switch
        {
            Ok<T> ok => ok.Result,
            Err<T> err => throw new ExpectedException
            (
                IsNullOrWhiteSpace(expected)
                    ? "Expected Ok when an error occurred"
                    : $"Expected {expected}",
                err.Error
            ),
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }

    /// <summary>
    ///     Expects <paramref name="result" /> to be an instance of
    ///     <see cref="Oxidize.Err{T}" />, retrieves it's <see cref="IError" />
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <param name="expected">optional, a <see cref="string" /> message of what you expect</param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>
    ///     Returns a value <see cref="IError" /> if <paramref name="result" />
    ///     is an instance of <see cref="Oxidize.Err{T}" />, otherwise it
    ///     throws an <see cref="ExpectedErrException" />
    /// </returns>
    /// <exception cref="ExpectedErrException">
    ///     Thrown if <paramref name="result" /> is an instance of <see cref="Oxidize.Ok{T}" />
    /// </exception>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static IError ExpectErr<T>(this IResult<T> result, string? expected = default)
    {
        return result switch
        {
            Ok<T> ok => throw new ExpectedErrException
            (
                IsNullOrWhiteSpace(expected)
                    ? "Expected an Err when Ok"
                    : $"Expected {expected}",
                ok.Result
            ),
            Err<T> err => err.Error,
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }

    /// <summary>
    ///     Converts an <see cref="IResult{T}" /> who's  <see cref="Oxidize.Ok{T}" />
    ///     <typeparamref name="T" /> type parameter is <see cref="IResult{T}" />
    /// </summary>
    /// <param name="result">
    ///     instance of <see cref="IResult{T}" />, where
    ///     <typeparamref name="T" /> is <see cref="IResult{T}" />
    /// </param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>Returns an a values of <see cref="IResult{T}" /></returns>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static IResult<T> Flatten<T>(this IResult<IResult<T>> result)
    {
        return result switch
        {
            Ok<IResult<T>> ok => ok.Result,
            Err<IResult<T>> err => new Err<T>(err.Error),
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }

    /// <summary>
    ///     Determines if the <paramref name="result" /> <see cref="IResult{T}" />
    ///     is an <see cref="Oxidize.Err{T}" />
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>
    ///     Returns true if <paramref name="result" /> is
    ///     an <see cref="Oxidize.Err{T}" />, false otherwise
    /// </returns>
    public static bool IsErr<T>(this IResult<T> result) => result is Err<T>;

    /// <summary>
    ///     Checks if <paramref name="result" /> <see cref="IResult{T}" />
    ///     <see cref="Oxidize.Err{T}" /> matches a defined <paramref name="predicate" />
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <param name="predicate">
    ///     <see cref="Func{IError,TResult}" /> predicate to verify <see cref="Oxidize.Err{T}" />
    /// </param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>
    ///     Returns true if <paramref name="result" /> is an <see cref="Oxidize.Err{T}" />
    ///     and it matches the <paramref name="predicate" />, false otherwise
    /// </returns>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static bool IsErrAnd<T>(this IResult<T> result, Func<IError, bool> predicate)
    {
        return result switch
        {
            Ok<T> => false,
            Err<T> err => predicate(err.Error),
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }

    /// <summary>
    ///     Determines if the <paramref name="result" /> <see cref="IResult{T}" />
    ///     is an <see cref="Oxidize.Ok{T}" />
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>
    ///     Returns true if <paramref name="result" /> is
    ///     an <see cref="Oxidize.Ok{T}" />, false otherwise
    /// </returns>
    public static bool IsOk<T>(this IResult<T> result) => result is Ok<T>;

    /// <summary>
    ///     Checks if <paramref name="result" /> <see cref="IResult{T}" />
    ///     <see cref="Oxidize.Ok{T}" /> matches a defined <paramref name="predicate" />
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <param name="predicate">
    ///     <see cref="Func{T,TResult}" /> predicate to verify <see cref="Oxidize.Ok{T}" />
    /// </param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>
    ///     Returns true if <paramref name="result" /> is <see cref="Oxidize.Ok{T}" />
    ///     and it matches the <paramref name="predicate" />, false otherwise
    /// </returns>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static bool IsOkAnd<T>(this IResult<T> result, Func<T, bool> predicate)
    {
        return result switch
        {
            Ok<T> ok => predicate(ok.Result),
            Err<T> => false,
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }

    /// <summary>
    ///     Maps an <see cref="Oxidize.Ok{T}" /> <typeparamref name="TSource" /> value
    ///     to an <see cref="Oxidize.Ok{T}" /> <typeparamref name="TMap" /> value
    /// </summary>
    /// <param name="result">result <see cref="IResult{T}" /> to map</param>
    /// <param name="map">mapping <see cref="Func{T, TResult}" /></param>
    /// <typeparam name="TSource"><see cref="Type" /> of result <see cref="IResult{T}" /></typeparam>
    /// <typeparam name="TMap"><see cref="Type" /> of mapped <see cref="IResult{T}" /></typeparam>
    /// <returns>
    ///     A mapped value of <see cref="Oxidize.Ok{T}" /> of <see cref="IResult{T}" />,
    ///     otherwise the original <see cref="Oxidize.Err{T}" /> as <see cref="Oxidize.Err{T}" />
    /// </returns>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static IResult<TMap> Map<TSource, TMap>(this IResult<TSource> result, Func<TSource, TMap> map)
    {
        return result switch
        {
            Ok<TSource> ok => new Ok<TMap>(map(ok.Result)),
            Err<TSource> err => new Err<TMap>(err.Error),
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }

    /// <summary>
    ///     Maps an <see cref="Oxidize.Err{T}" /> value to another
    ///     <see cref="Oxidize.Err{T}" /> value
    /// </summary>
    /// <param name="result">result <see cref="IResult{T}" /> to map</param>
    /// <param name="map">mapping <see cref="Func{T, T}" /></param>
    /// <typeparam name="T"><see cref="Type" /> of <see cref="IResult{T}" /></typeparam>
    /// <returns>
    ///     A mapped value of <see cref="Oxidize.Err{T}" /> otherwise
    ///     the original <see cref="Oxidize.Ok{T}" />
    /// </returns>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static IResult<T> MapErr<T>(this IResult<T> result, Func<IError, IError> map)
    {
        return result switch
        {
            Ok<T> => result,
            Err<T> err => new Err<T>(map(err.Error)),
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }


    /// <summary>
    ///     Maps an <see cref="Oxidize.Ok{T}" /> <typeparamref name="TSource" />
    ///     to an <see cref="Oxidize.Ok{T}" /> <typeparamref name="TMap" /> value
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <param name="default">
    ///     default <typeparamref name="TMap" /> value if <paramref name="result" />
    ///     is an <see cref="Oxidize.Err{T}" />
    /// </param>
    /// <param name="map">value mapping <see cref="Func{T, TResult}" /></param>
    /// <typeparam name="TSource">
    ///     source <see cref="Type" /> of <paramref name="result" />
    /// </typeparam>
    /// <typeparam name="TMap">
    ///     mapped <see cref="Type" /> of <paramref name="result" />
    /// </typeparam>
    /// <returns>
    ///     Returns a mapped <see cref="Oxidize.Ok{T}" /> <typeparamref name="TMap" />
    ///     value, otherwise <paramref name="default" /> <see cref="Oxidize.Ok{T}" />
    ///     <typeparamref name="TMap" /> value
    /// </returns>
    /// <remarks>
    ///     Argument <paramref name="default" /> is eagerly evaluated;
    ///     use <see cref="MapOrElse{TSource,TMap}" /> for lazy evaluation
    /// </remarks>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static TMap MapOr<TSource, TMap>
    (
        this IResult<TSource> result,
        TMap @default,
        Func<TSource, TMap> map
    )
    {
        return result switch
        {
            Ok<TSource> ok => map(ok.Result),
            Err<TSource> => @default,
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }

    /// <summary>
    ///     Maps an <see cref="Oxidize.Ok{T}" /> <typeparamref name="TSource" />
    ///     to an <see cref="Oxidize.Ok{T}" /> <typeparamref name="TMap" /> value
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <param name="map">value mapping <see cref="Func{T, TResult}" /></param>
    /// <param name="alternative">
    ///     alternative value mapping <see cref="Func{T, TResult}" />,
    ///     if <paramref name="result" /> is an <see cref="Oxidize.Err{T}" />
    /// </param>
    /// <typeparam name="TSource">
    ///     source <see cref="Type" /> of <paramref name="result" />
    /// </typeparam>
    /// <typeparam name="TMap">
    ///     mapped <see cref="Type" /> of <paramref name="result" />
    /// </typeparam>
    /// <returns>
    ///     Returns a mapped <see cref="Oxidize.Ok{T}" /> <typeparamref name="TMap" />
    ///     value, otherwise an <paramref name="alternative" /> mapped value of an
    ///     <see cref="Oxidize.Err{T}" /> <typeparamref name="TMap" /> value
    /// </returns>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static TMap MapOrElse<TSource, TMap>
    (
        this IResult<TSource> result,
        Func<TSource, TMap> map,
        Func<IError, TMap> alternative
    )
    {
        return result switch
        {
            Ok<TSource> ok => map(ok.Result),
            Err<TSource> err => alternative(err.Error),
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }

    /// <summary>
    ///     Peeks at an <see cref="IResult{T}" /> value
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <param name="peek">peeking <see cref="Action{T}" /></param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>Return original <paramref name="result" /> <see cref="IResult{T}" /></returns>
    public static IResult<T> Peek<T>(this IResult<T> result, Action<IResult<T>> peek)
    {
        peek(result);

        return result;
    }

    /// <summary>
    ///     Peeks at only <see cref="Oxidize.Err{T}" /> values
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <param name="peek">peeking <see cref="Action{T}" /></param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>Return original <paramref name="result" /> <see cref="IResult{T}" /></returns>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static IResult<T> PeekErr<T>(this IResult<T> result, Action<IError> peek)
    {
        if (result is Err<T> err) peek(err.Error);

        return result;
    }

    /// <summary>
    ///     Peeks at only <see cref="Oxidize.Ok{T}" /> values
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <param name="peek">peeking <see cref="Action{T}" /></param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>Return original <paramref name="result" /> <see cref="IResult{T}" /></returns>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static IResult<T> PeekOk<T>(this IResult<T> result, Action<T> peek)
    {
        if (result is Ok<T> ok) peek(ok.Result);

        return result;
    }

    /// <summary>
    ///     Retrieves the specific <see cref="IError" /> of an
    ///     <see cref="IResult{T}" /> <see cref="Oxidize.Err{T}" /> instance
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>
    ///     Returns an <see cref="IError" /> instance if <paramref name="result" />
    ///     is an <see cref="Oxidize.Err{T}" />, null otherwise
    /// </returns>
    /// <remarks>
    ///     A null result indicates that <paramref name="result" /> is an
    ///     instance of <see cref="Oxidize.Ok{T}" />
    /// </remarks>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static IError? UnwrapErrOrDefault<T>(this IResult<T> result)
    {
        return result switch
        {
            Ok<T> => default,
            Err<T> err => err.Error,
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }

    /// <summary>
    ///     Unwraps the value <typeparamref name="T" /> from an <see cref="Oxidize.Ok{T}" />
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <param name="default">
    ///     default <typeparamref name="T" /> value if <paramref name="result" />
    ///     is an <see cref="Oxidize.Err{T}" />
    /// </param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>
    ///     Returns the value <typeparamref name="T" /> of an <see cref="Oxidize.Ok{T}" />
    ///     <paramref name="result" />, otherwise a <paramref name="default" /> value
    /// </returns>
    /// <remarks>
    ///     Argument <paramref name="default" /> is eagerly evaluated;
    ///     use <see cref="UnwrapOrElse{T}" /> for lazy evaluation
    /// </remarks>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static T UnwrapOr<T>(this IResult<T> result, T @default)
    {
        return result switch
        {
            Ok<T> ok => ok.Result,
            Err<T> => @default,
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }

    /// <summary>
    ///     Unwraps the value <typeparamref name="T" /> from an <see cref="Oxidize.Ok{T}" />
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <param name="default">
    ///     default <see cref="Func{T,TResult}" /> evaluator if <paramref name="result" />
    ///     is an <see cref="Oxidize.Err{T}" />
    /// </param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>
    ///     Returns the value <typeparamref name="T" /> of an <see cref="Oxidize.Ok{T}" />
    ///     <paramref name="result" />, otherwise the <paramref name="default" />
    ///     evaluation of an <see cref="Oxidize.Err{T}" /> <paramref name="result" />
    /// </returns>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static T UnwrapOrElse<T>(this IResult<T> result, Func<IError, T> @default)
    {
        return result switch
        {
            Ok<T> ok => ok.Result,
            Err<T> err => @default(err.Error),
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }

    /// <summary>
    ///     Unwraps the value <typeparamref name="T" /> from an <see cref="Oxidize.Ok{T}" />
    /// </summary>
    /// <param name="result">instance of <see cref="IResult{T}" /></param>
    /// <typeparam name="T">
    ///     return <see cref="Type" /> of successful operation, <see cref="Oxidize.Ok{T}" />
    /// </typeparam>
    /// <returns>
    ///     Returns the value <typeparamref name="T" /> of an <see cref="Oxidize.Ok{T}" />
    ///     <paramref name="result" />, otherwise the default value of <typeparamref name="T?" />
    /// </returns>
    /// <exception cref="UnsupportedResultException">
    ///     Only <see cref="Oxidize.Ok"/>,  <see cref="Oxidize.Ok{T}"/> and
    ///     <see cref="Oxidize.Err{T}"/> are supported, any other implementation
    ///     will throw an exception 
    /// </exception>
    public static T? UnwrapOrDefault<T>(this IResult<T> result)
    {
        return result switch
        {
            Ok<T> ok => ok.Result,
            Err<T> => default,
            _ => throw new UnsupportedResultException(result.GetType())
        };
    }
}