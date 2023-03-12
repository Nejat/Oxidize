namespace Oxidize;

/// <summary>
///     A record that represents is a Void return value
/// </summary>
/// <remarks>
///     Can not be instantiated, use static field <see cref="Void" />
/// </remarks>
public record Unit
{
    public static readonly Unit Void = new();

    /// <summary>
    ///     Can not be instantiated
    /// </summary>
    private Unit()
    {
    }
}