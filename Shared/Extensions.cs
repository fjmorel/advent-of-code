public static class Extensions
{
    /// <summary>
    /// Parse a single number from each line
    /// </summary>
    public static List<long> ParseLongs(this string[] lines) => lines.Select(x => long.Parse(x)).ToList();

    /// <summary>
    /// Parse a single number from each line
    /// </summary>
    public static List<int> ParseInts(this string[] lines) => lines.Select(x => int.Parse(x)).ToList();

    /// <summary>
    /// Parse an array of numbers separate by comma
    /// </summary>
    public static int[] ParseCsvInts(this string line) => line.Split(',').Select(int.Parse).ToArray();
    /// <summary>
    /// Parse an array of numbers separate by comma
    /// </summary>
    public static long[] ParseCsvLongs(this string line) => line.Split(',').Select(long.Parse).ToArray();

    /// <summary>
    /// Parse an array of digits with no separator
    /// </summary>
    public static int[] ParseDigits(this string line) => line.Select(c => c - 48).ToArray();
}
