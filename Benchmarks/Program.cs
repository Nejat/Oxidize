using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Oxidize;

BenchmarkRunner.Run<Exceptional>();

[MemoryDiagnoser]
public class Exceptional
{
    [Benchmark]
    public void DotNetFail()
    {
        try
        {
            var _result = DotNetReferenceType(fail: true);
        }
        catch (Exception _err)
        {
        }
    }

    [Benchmark]
    public void OxidizedFail()
    {
        var _result = OxidizedReferenceType(fail: true);
    }

    [Benchmark]
    public void SuccessIntrinsicTypeDotNet()
    {
        try
        {
            var _result = DotNetIntrinsicType(fail: false);
        }
        catch
        {
        }
    }

    [Benchmark]
    public void SuccessIntrinsicTypeOxidized()
    {
        var _result = OxidizedIntrinsicType(fail: false);
    }

    [Benchmark]
    public void SuccessValueTypeDotNet()
    {
        try
        {
            var _result = DotNetValueType(fail: false);
        }
        catch
        {
        }
    }

    [Benchmark]
    public void SuccessValueTypeOxidized()
    {
        var _result = OxidizedValueType(fail: false);
    }

    [Benchmark]
    public void SuccessReferenceTypeDotNet()
    {
        try
        {
            var _result = DotNetReferenceType(fail: false);
        }
        catch
        {
        }
    }

    [Benchmark]
    public void SuccessReferenceTypeOxidized()
    {
        var _result = OxidizedReferenceType(fail: false);
    }

    private IResult<int> OxidizedIntrinsicType(bool fail)
    {
        return fail ? new Err<int>(new Error("Oops")) : new Ok<int>(42);
    }

    private IResult<Data> OxidizedReferenceType(bool fail)
    {
        return fail
            ? new Err<Data>(new Error("Oops"))
            : new Ok<Data>(new Data(42));
    }

    private IResult<DataStruct> OxidizedValueType(bool fail)
    {
        return fail
            ? new Err<DataStruct>(new Error("Oops"))
            : new Ok<DataStruct>(new DataStruct(42));
    }

    private int DotNetIntrinsicType(bool fail)
    {
        return fail ? throw new Exception("Oops") : 42;
    }

    private Data DotNetReferenceType(bool fail)
    {
        return fail ? throw new Exception("Oops") : new Data(42);
    }

    private DataStruct DotNetValueType(bool fail)
    {
        return fail ? throw new Exception("Oops") : new DataStruct(42);
    }
}

public record Error(string? ErrorMessage) : IError
{
    public string? Message { get; } = $"Error Occured: {ErrorMessage}";

    public IError? InternalError { get; } = default;

    public bool Equals(IError? other)
    {
        if (other is not Error error) return false;
        if (ReferenceEquals(this, error)) return false;

        return ErrorMessage == error.ErrorMessage;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ErrorMessage);
    }
}

public record Data(int Value);

public record struct DataStruct(int Value);