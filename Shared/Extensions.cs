#pragma warning disable CA1050
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

    /// <summary>
    /// Output a set of coordinates to the console for debugging (or sometimes viewing a solution)
    /// </summary>
    /// <param name="points">List/Set of points</param>
    /// <param name="characterSelector">Determine what to print for a given Point</param>
    public static void Print(this IReadOnlyCollection<Point> points, Func<Point, char> characterSelector)
    {
        var minX = points.Min(pt => pt.x);
        var minY = points.Min(pt => pt.y);
        var maxX = points.Max(pt => pt.x);
        var maxY = points.Max(pt => pt.y);

        AnsiConsole.WriteLine();

        for (var y = minY; y <= maxY; y++)
        {
            AnsiConsole.WriteLine();
            for (var x = minX; x <= maxX; x++)
            {
                AnsiConsole.Write(characterSelector(new(x, y)));
            }
        }

        AnsiConsole.WriteLine();
    }

    /// <summary>
    /// Output a set of coordinates to a string
    /// </summary>
    /// <param name="points">List/Set of points</param>
    /// <param name="characterSelector">Determine what to print for a given Point</param>
    public static string ToString(this IReadOnlyCollection<Point> points, Func<Point, char> characterSelector)
    {
        var output = new StringBuilder(points.Count + 10);
        var minX = points.Min(pt => pt.x);
        var minY = points.Min(pt => pt.y);
        var maxX = points.Max(pt => pt.x);
        var maxY = points.Max(pt => pt.y);


        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                output.Append(characterSelector(new(x, y)));
            }

            output.AppendLine();
        }

        return output.ToString();
    }
}
