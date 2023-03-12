using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Moq;

namespace Oxidize.Tests;

// ReSharper disable once InconsistentNaming
public class given_an_err_T
{
    [Fact]
    public void and_an_ok_T_it_should_be_an_err_T()
    {
        var error = new Mock<IError>().Object;

        var sut = new Err<int>(error).And(new Ok<int>(42));
        var actual = sut.IsErr();

        Assert.True(actual);
        Assert.Same(error, sut.UnwrapErrOrDefault());
    }

    [Fact]
    public void and_an_err_T2_it_should_be_an_err_T()
    {
        var error = new Mock<IError>().Object;
        var error2 = new Mock<IError>().Object;
        var sut = new Err<int>(error).And(new Err<int>(error2));
        var actual = sut.IsErr();

        Assert.True(actual);
        Assert.Same(error, sut.UnwrapErrOrDefault());
    }

    [Fact]
    public void and_then_a_value_T_it_should_be_an_err_T()
    {
        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error).AndThen(UnreachableAndThen);
        var actual = sut.IsErr();

        Assert.True(actual);

        [ExcludeFromCodeCoverage]
        IResult<int> UnreachableAndThen(int _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void it_should_not_contain_a_value_T()
    {
        const int expected = 42;

        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);
        var actual = sut.Contains(expected);

        Assert.False(actual);
    }

    [Fact]
    public void it_should_contain_an_err_T()
    {
        var mock = new Mock<IError>();
        var error = mock.Object;

        // ContainsErr requires IEquatable implemented
        mock.Setup(val => val.Equals(It.IsAny<IError>()))
            .Returns<IError>(val => ReferenceEquals(error, val));

        var sut = new Err<int>(error);
        var actual = sut.ContainsErr(error);

        Assert.True(actual);
        Assert.Same(error, sut.Error);
    }

    [Fact]
    public void unwrapping_err_or_default_should_be_an_err_T()
    {
        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);
        var actual = sut.UnwrapErrOrDefault();

        Assert.Same(error, actual);
    }

    [Fact]
    public void it_should_expect_to_panic_with_expected_message()
    {
        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);

        var exception = Assert.Throws<ExpectedException>(() => sut.Expect("an integer value"));

        Assert.Equal("Expected an integer value", exception.Message);
        Assert.Same(error, exception.Error);
    }

    [Fact]
    public void it_should_expect_to_panic_with_a_generic_expected_message()
    {
        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);

        var exception = Assert.Throws<ExpectedException>(() => sut.Expect());

        Assert.Equal("Expected Ok when an error occurred", exception.Message);
        Assert.Same(error, exception.Error);
    }

    [Fact]
    public void it_should_expect_an_err_T()
    {
        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);
        var actual = sut.ExpectErr("an exception");

        Assert.Same(error, actual);
    }

    [Fact]
    public void flattening_T_of_Err_T_it_should_return_err_T()
    {
        var error = new Mock<IError>().Object;
        var sut = new Err<IResult<int>>(error);
        var actual = sut.Flatten().ExpectErr("an exception");

        Assert.Same(error, actual);
    }

    [Fact]
    public void unwrapping_or_a_value_T_it_should_be_a_value_T()
    {
        const int expected = 42;

        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);
        var actual = sut.UnwrapOr(42);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void unwrapping_or_else_evaluating_a_value_T_it_should_return_a_value_T()
    {
        const int expected = 42;

        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);
        var actual = sut.UnwrapOrElse(_ => 42);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void it_should_peek_at_error()
    {
        var mock = new Mock<IError>();
        var error = mock.Object;

        // ContainsErr requires IEquatable implemented
        mock.Setup(val => val.Equals(It.IsAny<IError>()))
            .Returns<IError>(val => ReferenceEquals(val, error));

        var sut = new Err<int>(error);
        var peeked = false;

        var actual = sut
            .Peek(result => peeked = result.IsErr())
            .ContainsErr(error);

        Assert.True(peeked && actual);
    }

    [Fact]
    public void it_should_peek_err_at_error()
    {
        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);
        var peeked = false;

        var actual = sut
            .PeekErr(err => peeked = err == error)
            .ExpectErr("an exception");

        Assert.True(peeked);
        Assert.Same(error, actual);
    }

    [Fact]
    public void it_should_not_peek_at_ok()
    {
        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);
        var peeked = false;

        var actual = sut
            .PeekOk(UnreachablePeekOk)
            .ExpectErr("an exception");

        Assert.False(peeked);
        Assert.Same(error, actual);

        [ExcludeFromCodeCoverage]
        void UnreachablePeekOk(int _)
        {
            throw new UnreachableException();
        }
    }

    [Fact]
    public void it_should_be_an_err_T()
    {
        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);
        var actual = sut.IsErr();

        Assert.True(actual);
    }

    [Fact]
    public void it_should_be_an_err_and_a_true_predicate()
    {
        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);
        var actual = sut.IsErrAnd(err => string.IsNullOrEmpty(err.Message));

        Assert.True(actual);
    }
    
    [Fact]
    public void it_should_be_an_err_and_a_false_predicate()
    {
        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);
        var actual = sut.IsErrAnd(err => err.Message == "Unexpected message");

        Assert.False(actual);
    }

    [Fact]
    public void it_should_not_be_ok()
    {
        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);
        var actual = sut.IsOk();

        Assert.False(actual);
    }

    [Fact]
    public void it_should_not_be_ok_and_true_predicate()
    {
        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);
        var actual = sut.IsOkAnd(UnreachableIsOkAnd);

        Assert.False(actual);

        [ExcludeFromCodeCoverage]
        bool UnreachableIsOkAnd(int _)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void it_should_not_map_to_another_type()
    {
        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);

        var actual = sut
            .Map(UnreachableMap)
            .ExpectErr("an exception");

        Assert.Same(error, actual);

        [ExcludeFromCodeCoverage]
        int UnreachableMap(int _)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void it_should_map_an_to_another_err_T()
    {
        var error2 = new Mock<IError>().Object;
        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);

        var actual = sut
            .MapErr(_ => error2)
            .ExpectErr("an exception");

        Assert.Same(error2, actual);
    }

    [Fact]
    public void it_should_map_to_a_default_value_not_to_another_type()
    {
        const string expected = "Default Value";

        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);
        var actual = sut.MapOr("Default Value", UnreachableMapOr);

        Assert.Equal(expected, actual);

        [ExcludeFromCodeCoverage]
        string UnreachableMapOr(int _)
        {
            throw new NotImplementedException();
        }
    }


    [Fact]
    public void it_should_map_an_err_to_an_alternate_value_not_to_map_value()
    {
        const string expected = "!42";

        var error = new Mock<IError>().Object;
        var sut = new Err<int>(error);
        var actual = sut.MapOrElse(UnreachableMapOrElse, _ => "!42");

        Assert.Equal(expected, actual);

        [ExcludeFromCodeCoverage]
        string UnreachableMapOrElse(int _)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void unwrapping_or_default_should_be_default_ot_T()
    {
        var expected = default(int?);
        var error = new Mock<IError>().Object;
        var sut = new Err<int?>(error);
        var actual = sut.UnwrapOrDefault();

        Assert.Equal(expected, actual);
    }
}