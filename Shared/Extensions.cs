using System.Numerics;

namespace Shared;

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

    public static List<int> GetDigits(this int num, bool bigEndian)
    {
        var digits = GetDigits(num);
        if (bigEndian)
            digits.Reverse();

        return digits;
    }

    private static List<int> GetDigits(int num)
    {
        var digits = new List<int>();
        while (num > 0)
        {
            var digit = num % 10;
            digits.Add(digit);
            num = (num - digit) / 10;
        }

        return digits;
    }

    public static List<long> GetDigits(this long num, bool bigEndian)
    {
        var digits = GetDigits(num);
        if (bigEndian)
            digits.Reverse();

        return digits;
    }

    private static List<long> GetDigits(long num)
    {
        var digits = new List<long>();
        while (num > 0)
        {
            var digit = num % 10;
            digits.Add(digit);
            num = (num - digit) / 10;
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
    /// Output a set of coordinates to a string
    /// </summary>
    /// <param name="points">List/Set of points</param>
    /// <param name="characterSelector">Determine what to print for a given Point</param>
    public static string ToString(this IReadOnlyCollection<Point> points, Func<Point, char> characterSelector)
    {
        var output = new StringBuilder(points.Count + 10);
        output.AppendLine();
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

    /// <summary>
    /// Convert input text into a lookup by coordinates
    /// </summary>
    public static Dictionary<Point, int> ToDigitGrid(this IEnumerable<string> lines)
    {
        var y = 0;
        var dict = new Dictionary<Point, int>();
        foreach (var line in lines)
        {
            for (var x = 0; x < line.Length; x++)
            {
                dict[new(x, y)] = line[x] - '0';
            }

            y++;
        }

        return dict;
    }

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
}
