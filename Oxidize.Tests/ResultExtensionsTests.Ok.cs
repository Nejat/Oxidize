using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Moq;

namespace Oxidize.Tests;

// ReSharper disable once InconsistentNaming
public class given_ok
{
    [Fact]
    public void and_an_ok_it_should_be_an_ok()
    {
        var sut = Ok.Result.And(Ok.Result);
        var actual = sut.Contains(Unit.Void);

        Assert.True(actual);
    }

    [Fact]
    public void and_an_err_T_it_should_be_err_T()
    {
        var error = new Mock<IError>().Object;
        var sut = Ok.Result.And(new Err<Unit>(error));
        var actual = sut.IsErr();

        Assert.True(actual);
        Assert.Same(error, sut.UnwrapErrOrDefault());
    }

    [Fact]
    public void and_then_an_ok_it_should_be_an_ok()
    {
        var expected = Unit.Void;

        var sut = Ok.Result.AndThen(_ => Ok.Result);
        var actual = sut.Contains(expected);

        Assert.True(actual);
    }
    
    [Fact]
    public void and_then_an_err_T_it_should_be_an_err_T()
    {
        var error = new Mock<IError>().Object;

        var sut = Ok.Result.AndThen(_ => new Err<Unit>(error));
        var actual = sut.IsErr();

        Assert.True(actual);
    }

    [Fact]
    public void it_should_contain_an_ok()
    {
        var expected = Unit.Void;

        var sut = Ok.Result;
        var actual = sut.Contains(expected);

        Assert.True(actual);
    }

    [Fact]
    public void it_should_not_contain_an_err()
    {
        var sut = Ok.Result;
        var err = new Mock<IError>().Object;
        var actual = sut.ContainsErr(err);

        Assert.False(actual);
    }

    [Fact]
    public void unwrapping_err_or_default_should_not_be_an_err()
    {
        var sut = Ok.Result;
        var actual = sut.UnwrapErrOrDefault();

        Assert.Null(actual);
    }

    [Fact]
    public void it_should_expect_an_ok()
    {
        var expected = Unit.Void;

        var sut = Ok.Result;
        var actual = sut.Expect("a unit result");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void it_should_panic_when_expecting_an_err_with_expected_message()
    {
        var expected = Unit.Void;

        var sut = Ok.Result;

        var exception = Assert.Throws<ExpectedErrException>(() => sut.ExpectErr("on some error"));

        Assert.Equal("Expected on some error", exception.Message);
        Assert.Equal(expected, exception.Result);
    }

    [Fact]
    public void it_should_panic_when_expecting_an_err_with_generic_expected_message()
    {
        var expected = Unit.Void;

        var sut = Ok.Result;

        var exception = Assert.Throws<ExpectedErrException>(() => sut.ExpectErr());

        Assert.Equal("Expected an Err when Ok", exception.Message);
        Assert.Equal(expected, exception.Result);
    }

    [Fact]
    public void inside_of_an_ok_value_unit_should_return_inside_ok()
    {
        var expected = Unit.Void;

        var sut = new Ok<IResult<Unit>>(Ok.Result);
        var actual = sut.Flatten().Contains(expected);

        Assert.True(actual);
    }

    [Fact]
    public void or_a_result2_it_should_return_an_ok()
    {
        var expected = Unit.Void;

        var sut = Ok.Result;
        var actual = sut.UnwrapOr(Unit.Void);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void or_else_a_result2_it_should_return_an_ok()
    {
        var expected = Unit.Void;

        var sut = Ok.Result;
        var actual = sut.UnwrapOrElse(UnreachableOrElse);

        Assert.Equal(expected, actual);

        [ExcludeFromCodeCoverage]
        Unit UnreachableOrElse(IError _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void it_should_peek_at_the_ok()
    {
        var expected = Unit.Void;

        var sut = Ok.Result;
        var peeked = false;

        var actual = sut
            .Peek(result => peeked = result.IsOk())
            .Contains(expected);

        Assert.True(peeked && actual);
    }

    [Fact]
    public void it_should_not_peek_err()
    {
        var expected = Unit.Void;

        var sut = Ok.Result;

        var actual = sut
            .PeekErr(UnreachablePeekErr)
            .Expect("a unit result");

        Assert.Equal(expected, actual);

        [ExcludeFromCodeCoverage]
        void UnreachablePeekErr(IError _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void it_should_peek_ok_at_the_ok()
    {
        var expected = Unit.Void;

        var sut = Ok.Result;
        var peeked = false;

        var actual = sut
            .PeekOk(value => peeked = value == expected)
            .Expect("a unit result");

        Assert.True(peeked);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void it_should_not_be_an_err()
    {
        var sut = Ok.Result;
        var actual = sut.IsErr();

        Assert.False(actual);
    }

    [Fact]
    public void it_should_not_be_an_err_nor_a_checked_err()
    {
        var sut = Ok.Result;
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
        var sut = Ok.Result;
        var actual = sut.IsOk();

        Assert.True(actual);
    }

    [Fact]
    public void it_should_be_ok_when_valid_value()
    {
        var expected = Unit.Void;

        var sut = Ok.Result;
        var actual = sut.IsOkAnd(value => value == expected);

        Assert.True(actual);
    }

    [Fact]
    public void it_should_not_be_ok_when_invalid_value()
    {
        var expected = Unit.Void;

        var sut = Ok.Result;
        var actual = sut.IsOkAnd(value => value != expected);

        Assert.False(actual);
    }

    [Fact]
    public void it_should_map_to_another_type()
    {
        const string expected = "Unit { }";

        var sut = Ok.Result;
        var actual = sut
            .Map(value => value.ToString())
            .Expect("a string value");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void it_should_not_map_an_err()
    {
        var expected = Unit.Void;

        var sut = Ok.Result;
        var actual = sut
            .MapErr(UnreachableMapErr)
            .Expect("a unit result");

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
        const string expected = "Unit { }";

        var sut = Ok.Result;
        var actual = sut
            .MapOr(string.Empty, value => value.ToString());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void it_should_map_to_another_type_not_map_an_err_to_another_type()
    {
        const string expected = "Unit { }";

        var sut = Ok.Result;
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
    public void it_should_be_an_ok()
    {
        var expected = Unit.Void;

        var sut = Ok.Result;
        var actual = sut.UnwrapOrDefault();

        Assert.Equal(expected, actual);
    }
}