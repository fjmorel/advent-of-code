using System.ComponentModel;

namespace Shared;

/// <summary>
/// Basic interface for defining a puzzle solution
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]// do not use directly except in test runner
public interface ISolution
{
    // Default implementations in case a day requires number or string value
    ValueTask<long> GetPart1() => new(0L);
    ValueTask<long> GetPart2() => new(0L);
    async ValueTask<string> GetPart1String() => (await GetPart1()).ToString();
    async ValueTask<string> GetPart2String() => (await GetPart2()).ToString();
}

/// <summary>
/// Basic interface for defining a puzzle solution
/// </summary>
public interface ISolution<TSelf> : ISolution where TSelf : ISolution<TSelf>
{
    /// <summary>
    /// Create and initialize a solution
    /// </summary>
    /// <param name="lines">Lines from input file</param>
    static abstract TSelf Init(string[] lines);
}
