using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Moq;
using static Oxidize.Tests.UnsupportedResult;
using static Oxidize.Tests.UnsupportedEmbeddedResult;

namespace Oxidize.Tests;

// ReSharper disable once InconsistentNaming
public class given_an_unsupported_result
{
    private static readonly string EXPECTED_MESSAGE;
    private static readonly string EMBEDDED_EXPECTED_MESSAGE;

    static given_an_unsupported_result()
    {
        EXPECTED_MESSAGE =
            $"{nameof(UnsupportedResult)} is not a supported IResult<T> type by {nameof(ResultExtensions)} utilities";
        EMBEDDED_EXPECTED_MESSAGE =
            $"{nameof(UnsupportedEmbeddedResult)} is not a supported IResult<T> type by {nameof(ResultExtensions)} utilities";
    }

    [Fact]
    public void and_an_unsupported_result2_it_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.And(SUT));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);
    }

    [Fact]
    public void and_an_err_value_of_T_it_should_panic_unsupported_result()
    {
        var error = new Mock<IError>().Object;

        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.And(new Err<Unit>(error)));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);
    }

    [Fact]
    public void and_then_it_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.AndThen(UnreachableAndThen));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);

        [ExcludeFromCodeCoverage]
        IResult<Unit> UnreachableAndThen(Unit _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void contains_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.Contains(Unit.Void));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);
    }

    [Fact]
    public void it_should_panic_unsupported_result()
    {
        var err = new Mock<IError>().Object;

        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.ContainsErr(err));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);
    }

    [Fact]
    public void unwrapping_an_err_or_default_it_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.UnwrapErrOrDefault());

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);
    }

    [Fact]
    public void expect_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.Expect("an unreachable value"));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);
    }

    [Fact]
    public void expect_err_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.ExpectErr("exception on ok"));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);
    }

    [Fact]
    public void inside_of_an_unsupported_result_it_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => EMBEDDED_SUT.Flatten());

        Assert.Equal(expected: typeof(UnsupportedEmbeddedResult), exception.UnsupportedType);
        Assert.Equal(EMBEDDED_EXPECTED_MESSAGE, exception.Message);
    }

    [Fact]
    public void or_a_value_of_T_it_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.UnwrapOr(Unit.Void));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);
    }

    [Fact]
    public void or_else_a_value_of_T2_it_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.UnwrapOrElse(UnreachableOrElse));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);

        [ExcludeFromCodeCoverage]
        Unit UnreachableOrElse(IError _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void it_should_not_peek_err()
    {
        var actual = SUT
            .PeekErr(UnreachablePeekErr)
            .IsOk();

        Assert.False(actual);

        [ExcludeFromCodeCoverage]
        void UnreachablePeekErr(IError _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void it_should_not_peek_ok()
    {
        var actual = SUT
            .PeekOk(UnreachablePeekOk)
            .IsOk();

        Assert.False(actual);

        [ExcludeFromCodeCoverage]
        void UnreachablePeekOk(Unit _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void it_should_not_be_an_err()
    {
        var actual = SUT.IsErr();

        Assert.False(actual);
    }

    [Fact]
    public void is_err_and_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.IsErrAnd(UnreachableIsErrAnd));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);

        [ExcludeFromCodeCoverage]
        bool UnreachableIsErrAnd(IError _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void it_should_not_be_ok()
    {
        var actual = SUT.IsOk();

        Assert.False(actual);
    }

    [Fact]
    public void is_ok_and_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.IsOkAnd(UnreachableIsOkAnd));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);

        [ExcludeFromCodeCoverage]
        bool UnreachableIsOkAnd(Unit _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void map_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.Map(UnreachableMap));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);

        [ExcludeFromCodeCoverage]
        bool UnreachableMap(Unit _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void map_err_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.MapErr(UnreachableMapErr));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);

        [ExcludeFromCodeCoverage]
        IError UnreachableMapErr(IError _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void map_or_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.MapOr(false, UnreachableMapOr));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);

        [ExcludeFromCodeCoverage]
        bool UnreachableMapOr(Unit _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void map_or_else_should_panic_unsupported_result()
    {
        var exception =
            Assert.Throws<UnsupportedResultException>(() => SUT.MapOrElse(UnreachableMapOk, UnreachableMapErr));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);

        [ExcludeFromCodeCoverage]
        bool UnreachableMapErr(IError _)
        {
            throw new UnreachableException();
        }

        [ExcludeFromCodeCoverage]
        bool UnreachableMapOk(Unit _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void unwrapping_or_default_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.UnwrapOrDefault());

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);
    }

    [Fact]
    public void unwrapping_or_else_should_panic_unsupported_result()
    {
        var exception = Assert.Throws<UnsupportedResultException>(() => SUT.UnwrapOrElse(UnreachableMapOk));

        Assert.Equal(expected: typeof(UnsupportedResult), exception.UnsupportedType);
        Assert.Equal(EXPECTED_MESSAGE, exception.Message);

        [ExcludeFromCodeCoverage]
        Unit UnreachableMapOk(IError _)
        {
            throw new UnreachableException();
        }
    }
}

/// <summary>
///     An implementation of <see cref="IResult{T}"/> to demonstrate unsupported
///     results when using <see cref="ResultExtensions"/>
/// </summary>
internal class UnsupportedResult : IResult<Unit>
{
    /// <summary>
    ///     Subject under test instance of <see cref="UnsupportedResult"/>
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static readonly UnsupportedResult SUT = new();

    /// <summary>
    ///     Only a single static instance is required for testing
    /// </summary>
    private UnsupportedResult()
    {
    }
}

/// <summary>
///     An implementation of <see cref="IResult{T}"/> to demonstrate unsupported
///     results when using <see cref="ResultExtensions"/>
/// </summary>
internal class UnsupportedEmbeddedResult : IResult<IResult<Unit>>
{
    /// <summary>
    ///     Subject under test instance of <see cref="UnsupportedResult"/>
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static readonly UnsupportedEmbeddedResult EMBEDDED_SUT = new();

    /// <summary>
    ///     Only a single static instance is required for testing
    /// </summary>
    private UnsupportedEmbeddedResult()
    {
    }
}