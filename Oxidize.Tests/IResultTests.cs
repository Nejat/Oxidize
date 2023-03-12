// ReSharper disable InconsistentNaming

namespace Oxidize.Tests;

public class IResultTests
{
    [Fact]
    public void give_an_implementation_of_result_ot_T_it_should_have_a_corresponding_expected_result_type()
    {
        var actual = IResult<int>.ExpectedResultType;
        var expected = typeof(int);

        Assert.Equal(expected, actual);
    }
}