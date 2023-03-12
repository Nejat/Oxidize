# Oxidize<sup>*</sup>

Is a library that provides an alternative to the standard .Net error handling pattern of `throw` & `try {} catch {}`. 

Modeled after (_lifted straight from_) the [`Result`](https://doc.rust-lang.org/std/result/enum.Result.html) 
enumeration in the [`Rust`](https://www.rust-lang.org/) programming language, which enables operations to 
express results in the form of success `Ok` or failed `Err`  without resorting to throwing exceptions, while 
still providing clearly descriptive errors.

_* The library gets it's name from where it's inspiration originates, the
[venerable](https://survey.stackoverflow.co/2022/#most-loved-dreaded-and-wanted-language-love-dread) 
[`Rust`](https://www.rust-lang.org/) programming language_ 

## Motivation

The new pattern for returning the result of an operation, provides two clear benefits;

* The consumer of your code has to explicitly handle fail conditions
  * Mitigating unhandled runtime exceptions
  * Clearer self documenting code that better reflects it's intentions.
* Throwing exceptions to indicate an error has performance implications
  * Their usage should be limited for actual exceptions.

More subjective benefits include;

* More ergonomic than the `bool TrySomeOperation<T>(out T value)` pattern
* More expressive failure reporting than the `bool TrySomeOperation<T>(out T value)` pattern

## Caveats

Unfortunately .Net does not have the relatively lightweight `Enumeration` construct of 
[`Rust`](https://www.rust-lang.org/), _similar to the_ `Union` _type in other languages_, 
nor it's more robust pattern matching capabilities, nor the short circuit `?` operator 
to more ergonomically exit the current operation on fail conditions.

### Memory Allocations 

This method does introduce an additional memory allocation<sup>*</sup> for the successful 
scenario of an operation. However the failed scenario, arguably, has the same or similar 
allocations compared to when exceptions are thrown, but without the performance hit of 
exception handling. 

_* an implementation of a lightweight_ `Enuemration`_or_ `Union` _in .Net would solve this issue_


## Types

### `IResult{T}`

```csharp
namespace Oxidize;

public interface IResult<T>
{
    public static Type ExpectedResultType { get; } = typeof(T);
}
```
* `IResult{T}` is the return value of an operation that represents success or failure.
### `Ok{T}`<sup>*</sup>

```csharp
namespace Oxidize;

public record Ok<T>(T Result) : IResult<T>;
```

* `Ok{T}` indicates an operation completed successfully and it produced a result of `T`

### `Ok`<sup>*</sup>

```csharp
namespace Oxidize;

public record Ok : Ok<Unit>
{
    public new static readonly Ok Result = new();

    private Ok() : base(Unit.Void) { }
}
```

* `Ok` indicates an operation completed successfully, same a successful `void` operation
* Since it is not possible to represent with a value of `void`, an instance [`Unit`](#unit) is used
for `IResult{T}`
* `Ok` can not be instantiated, use the static instance `Ok.Result`

### `Unit`<sup>*</sup>

```csharp
namespace Oxidize;

public record Unit
{
    public static readonly Unit Void = new();

    private Unit() { }
}
```

* `Unit` represents a `void` return type of an operation
* `Unit` can not be instantiated, use the static instance `Unit.Void`

### `Err{T}`<sup>*</sup>

```csharp
namespace Oxidize;

public record Err<T>(IError Error) : IResult<T>;
```

* `Err{T}` indicates an operation failed  with an error value of [`IError`](#ierror-iequatableierror)

### `IError: IEquatable{IError}`

```csharp
namespace Oxidize;

public interface IError : IEquatable<IError>
{
    public string? Message { get; }

    public IError? InternalError { get; }
}
```

* `IError{T}` represent an error variant and should be used to define custom errors

</br></br>
_* unfortunately the use of_ `record struct` _instead of_ `record` _introduced other implementation limitations_

### Example

The following example demonstrates the use of `IResult<T>` for the operation `ValidateUniversalAnswer`
which provides clear failure scenarios, in this instance if a value is `TooSmall` or `ToLarge`. 

```csharp
using Oxidize;

var message = ValidateUniversalAnswer(42) switch
{
    Ok<int> ok => $"Perfect: {ok.Result}",
    Err<int> err =>
        err.Error switch
        {
            TooSmall small => $"Not quire right; {small.Message}",
            TooBig big => $"Not so perfect either: {big.Message}"
        },
};

Console.WriteLine(message);

IResult<int> ValidateUniversalAnswer(int value)
{
    const int minimum = 42;
    const int maximum = 42;

    return value switch
    {
        < minimum => new Err<int>(new TooSmall(value, minimum)),
        > maximum => new Err<int>(new TooBig(value, maximum)),
        _ => new Ok<int>(value)
    };
}

public record TooSmall(long Value, long Minimum) : IError
{
    public string? Message { get; } = $"{Value} is too small, it cannot be less than {Minimum}";

    public IError? InternalError { get; } = default;
}

public sealed record TooBig(long Value, long Maximum) : IError
{
    public string? Message { get; } = $"{Value} is too big, it cannot be greater than {Maximum}";

    public IError? InternalError { get; } = default;
}
```
