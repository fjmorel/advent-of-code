using System.Numerics;

namespace Shared;

public static class Extensions
{
    /// <summary>
    /// If the index is equal to the list length, loop back to 0.
    /// Otherwise, keep going.
    /// </summary>
    public static int LoopIndex<T>(this int listIndex, IReadOnlyList<T> list) => listIndex == list.Count ? 0 : listIndex;

    /// <summary>
    /// Combine Select and ToList on puzzle input
    /// </summary>
    public static List<T2> ToList<T1, T2>(this IEnumerable<T1> lines, Func<T1, T2> select) => lines.Select(select).ToList();

    /// <summary>
    /// Parse a single number from each line
    /// </summary>
    public static List<T> ParsePerLine<T>(this IEnumerable<string> lines) where T : IParsable<T> => lines.ToList(x => T.Parse(x, null));

    /// <summary>
    /// Parse an array of numbers separate by comma
    /// </summary>
    public static T[] ParseCsv<T>(this string line) where T : IParsable<T> => line.Split(',').Select(x => T.Parse(x, null)).ToArray();

    /// <summary>
    /// Parse an array of digits with no separator
    /// </summary>
    public static int[] ParseDigits(this IEnumerable<char> line) => line.Select(c => c - '0').ToArray();

    /// <summary>
    /// Get the digits from a number, in Base 10
    /// </summary>
    public static List<T> GetBase10Digits<T>(this T num) where T : INumber<T>, IBinaryInteger<T>
    {
        var digits = new List<T>();
        var ten = T.One + T.One + T.One + T.One + T.One + T.One + T.One + T.One + T.One + T.One;
        while (num > T.Zero)
        {
            (T quotient, T remainder) = T.DivRem(num, ten);
            digits.Add(remainder);
            num = quotient;
        }

        return digits;
    }

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
    /// Output a set of coordinates to a string that looks like a grid based on whether coordinates are in the set
    /// </summary>
    /// <param name="points">Set of points</param>
    /// <param name="includeHeader">Whether to include a line describing the min/max range of coordinates</param>
    public static string ToGridString(this IReadOnlySet<Point> points, bool includeHeader = true) =>
        points.ToString(pt => points.Contains(pt) ? '⬜' : '⬛', includeHeader);

    /// <summary>
    /// Output a set of coordinates to a string
    /// </summary>
    /// <param name="points">List/Set of points</param>
    /// <param name="characterSelector">Determine what to print for a given Point</param>
    /// <param name="includeHeader">Whether to include a line describing the min/max range of coordinates</param>
    public static string ToString(this IReadOnlyCollection<Point> points, Func<Point, char> characterSelector, bool includeHeader = true)
    {
        var output = new StringBuilder(points.Count + 10);
        output.AppendLine();
        var minX = points.Min(pt => pt.x);
        var minY = points.Min(pt => pt.y);
        var maxX = points.Max(pt => pt.x);
        var maxY = points.Max(pt => pt.y);

        if (includeHeader)
            output.AppendLine($"Coordinates: ({minX}, {minY}) to ({maxX}, {maxY})");

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

    /// <summary>
    /// Convert input text into a lookup by coordinates
    /// </summary>
    public static Dictionary<Point, int> ToDigitGrid(this IEnumerable<string> lines) => lines.ToGrid(x => x - '0');

    /// <summary>
    /// Convert input text into a lookup by coordinates
    /// </summary>
    public static Dictionary<Point, T> ToGrid<T>(this IEnumerable<string> lines, Func<char, T> converter)
    {
        var y = 0;
        var dict = new Dictionary<Point, T>();
        foreach (var line in lines)
        {
            for (var x = 0; x < line.Length; x++)
            {
                dict[new(x, y)] = converter(line[x]);
            }

            y++;
        }

        return dict;
    }

    /// <summary>
    /// Convert input text into a set of coordinates (keeping only active ones)
    /// </summary>
    public static HashSet<Point> ToPointSet(this IEnumerable<string> lines, Func<char, bool> converter)
    {
        var y = 0;
        var points = new HashSet<Point>();
        foreach (var line in lines)
        {
            for (var x = 0; x < line.Length; x++)
            {
                if (converter(line[x]))
                    points.Add(new(x, y));
            }

            y++;
        }

        return points;
    }
}
