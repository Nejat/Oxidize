using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Moq;

namespace Oxidize.Tests;

// ReSharper disable once InconsistentNaming
public class given_an_ok_value_of_T
{
    [Fact]
    public void and_an_ok_value_of_T2_it_should_be_an_ok_value_of_T2()
    {
        const int expected = 42;

        var sut = new Ok<int>(24).And(new Ok<int>(42));
        var actual = sut.Contains(expected);

        Assert.True(actual);
    }

    [Fact]
    public void and_an_err_value_of_T_it_should_not_be_an_ok_value_of_T()
    {
        var error = new Mock<IError>().Object;
        var sut = new Ok<int>(24).And(new Err<int>(error));
        var actual = sut.IsErr();

        Assert.True(actual);
        Assert.Same(error, sut.UnwrapErrOrDefault());
    }

    [Fact]
    public void and_then_an_ok_value_of_T2_it_should_be_an_ok_value_of_T2()
    {
        const int expected = 42;

        var sut = new Ok<int>(24).AndThen(insufficient => new Ok<int>(insufficient + 18));
        var actual = sut.Contains(expected);

        Assert.True(actual);
    }

    [Fact]
    public void it_should_contain_a_value_T()
    {
        const int expected = 42;

        var sut = new Ok<int>(42);
        var actual = sut.Contains(expected);

        Assert.True(actual);
    }

    [Fact]
    public void that_is_unit_it_should_contain_a_value_T_that_is_unit()
    {
        var expected = Unit.Void;

        var sut = new Ok<Unit>(Unit.Void);
        var actual = sut.Contains(expected);

        Assert.True(actual);
    }

    [Fact]
    public void it_should_not_contain_an_err()
    {
        var sut = new Ok<int>(42);
        var err = new Mock<IError>().Object;
        var actual = sut.ContainsErr(err);

        Assert.False(actual);
    }

    [Fact]
    public void unwrapping_err_or_default_should_not_be_an_err()
    {
        var sut = new Ok<int>(42);
        var actual = sut.UnwrapErrOrDefault();

        Assert.Null(actual);
    }

    [Fact]
    public void it_should_expect_a_value_T()
    {
        const int expected = 42;

        var sut = new Ok<int>(42);
        var actual = sut.Expect("an integer value");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void it_should_panic_when_expecting_an_err()
    {
        const int expected = 42;

        var sut = new Ok<int>(42);

        var exception = Assert.Throws<ExpectedErrException>(() => sut.ExpectErr("exception on ok"));

        Assert.Equal("Expected exception on ok", exception.Message);
        Assert.Equal(expected, exception.Result);
    }

    [Fact]
    public void inside_of_an_ok_T_it_should_return_inside_value_T()
    {
        const int expected = 42;

        var sut = new Ok<IResult<int>>(new Ok<int>(42));
        var actual = sut.Flatten().Contains(expected);

        Assert.True(actual);
    }

    [Fact]
    public void or_a_value_of_T2_it_should_return_an_ok_value_of_T()
    {
        const int expected = 42;

        var sut = new Ok<int>(42);
        var actual = sut.UnwrapOr(24);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void or_else_a_value_of_T2_it_should_return_an_ok_value_of_T()
    {
        const int expected = 42;

        var sut = new Ok<int>(42);
        var actual = sut.UnwrapOrElse(UnreachableOrElse);

        Assert.Equal(expected, actual);

        [ExcludeFromCodeCoverage]
        int UnreachableOrElse(IError _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void it_should_peek_at_result_T()
    {
        const int expected = 42;

        var sut = new Ok<int>(42);
        var peeked = false;

        var actual = sut
            .Peek(result => peeked = result.IsOk())
            .Contains(expected);

        Assert.True(peeked && actual);
    }

    [Fact]
    public void it_should_not_peek_err()
    {
        const int expected = 42;

        var sut = new Ok<int>(42);

        var actual = sut
            .PeekErr(UnreachablePeekErr)
            .Expect("an integer value");

        Assert.Equal(expected, actual);

        [ExcludeFromCodeCoverage]
        void UnreachablePeekErr(IError _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void it_should_peek_ok_at_the_value_T()
    {
        const int expected = 42;

        var sut = new Ok<int>(42);
        var peeked = false;

        var actual = sut
            .PeekOk(value => peeked = value == expected)
            .Expect("an integer value");

        Assert.True(peeked);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void it_should_not_be_an_err()
    {
        var sut = new Ok<int>(42);
        var actual = sut.IsErr();

        Assert.False(actual);
    }

    [Fact]
    public void it_should_not_be_an_err_nor_a_checked_err()
    {
        var sut = new Ok<int>(42);
        var actual = sut.IsErrAnd(UnreachableIsErrAnd);

        Assert.False(actual);

        [ExcludeFromCodeCoverage]
        bool UnreachableIsErrAnd(IError _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void it_should_be_ok()
    {
        var sut = new Ok<int>(42);
        var actual = sut.IsOk();

        Assert.True(actual);
    }

    [Fact]
    public void it_should_be_ok_when_valid_value()
    {
        const int expected = 42;

        var sut = new Ok<int>(42);
        var actual = sut.IsOkAnd(value => value == expected);

        Assert.True(actual);
    }

    [Fact]
    public void it_should_not_be_ok_when_invalid_value()
    {
        const int expected = 42;

        var sut = new Ok<int>(24);
        var actual = sut.IsOkAnd(value => value == expected);

        Assert.False(actual);
    }

    [Fact]
    public void it_should_map_to_another_type()
    {
        const string expected = "42";

        var sut = new Ok<int>(42);
        var actual = sut
            .Map(value => value.ToString())
            .Expect("a string value");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void it_should_not_map_an_err()
    {
        const int expected = 42;

        var sut = new Ok<int>(42);
        var actual = sut
            .MapErr(UnreachableMapErr)
            .Expect("an integer value");

        Assert.Equal(expected, actual);

        [ExcludeFromCodeCoverage]
        IError UnreachableMapErr(IError _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void it_should_map_to_another_type_not_the_alternative_value()
    {
        const string expected = "42";

        var sut = new Ok<int>(42);
        var actual = sut
            .MapOr(string.Empty, value => value.ToString());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void it_should_map_to_another_type_not_map_an_err_to_another_type()
    {
        const string expected = "42";

        var sut = new Ok<int>(42);
        var actual = sut
            .MapOrElse
            (
                value => value.ToString(),
                UnreachableMapOrElse
            );

        Assert.Equal(expected, actual);

        [ExcludeFromCodeCoverage]
        string UnreachableMapOrElse(IError _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void it_should_be_an_ok_value_of_t()
    {
        const int expected = 42;

        var sut = new Ok<int>(42);
        var actual = sut.UnwrapOrDefault();

        Assert.Equal(expected, actual);
    }
}