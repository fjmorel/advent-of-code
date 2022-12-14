namespace Shared;

public static class AsciiLetters
{
    private static readonly Dictionary<char, long> _letters = typeof(Letters)
        .GetFields()
        .ToDictionary(x => x.Name[0], x => ((x.GetRawConstantValue() as string)!).ToBitNumbers()[0]);

    public static string ParseAsciiLetters(this string input)
    {
        try
        {
            var output = new StringBuilder();
            var columns = input.ToBitNumbers();
            foreach (var chunk in columns)
            {
                var found = false;
                foreach (var (letter, letterColumns) in _letters)
                {
                    var equal = letterColumns == chunk;
                    if (!equal)
                        continue;

                    output.Append(letter);
                    found = true;
                    break;
                }

                if (!found)
                    throw new ArgumentException("Could not parse letter");
            }

            return output.ToString();
        }
        catch (Exception)
        {
            return input;
        }
    }

    private static List<long> ToBitNumbers(this string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var nums = new List<long>();
        for (var i = 0; i < lines[0].Length; i++)
        {
            nums.Add(lines.Select((line, index) => line[i] == '⬜' ? 1 *  int.RotateLeft(1, index) : 0).Sum());
        }

        return nums.Where(x => x != 0).Chunk(4)
            .ToList(chunk => chunk.Select((value, index) => value * int.RotateLeft(1, 4 * (index + 1))).Sum());
    }
}

// ReSharper disable UnusedMember.Local (used through reflection)
file static class Letters
{
    public const string A = """
        ⬛⬜⬜⬛
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        ⬜⬜⬜⬜
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        """;

    public const string B = """
        ⬜⬜⬜⬛
        ⬜⬛⬛⬜
        ⬜⬜⬜⬛
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        ⬜⬜⬜⬛
        """;

    public const string C = """
        ⬛⬜⬜⬛
        ⬜⬛⬛⬜
        ⬜⬛⬛⬛
        ⬜⬛⬛⬛
        ⬜⬛⬛⬜
        ⬛⬜⬜⬛
        """;

    public const string D = """
        ⬜⬜⬜⬛
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        ⬜⬜⬜⬛
        """;

    public const string E = """
        ⬜⬜⬜⬜
        ⬜⬛⬛⬛
        ⬜⬜⬜⬛
        ⬜⬛⬛⬛
        ⬜⬛⬛⬛
        ⬜⬜⬜⬜
        """;

    public const string F = """
        ⬜⬜⬜⬜
        ⬜⬛⬛⬛
        ⬜⬜⬜⬛
        ⬜⬛⬛⬛
        ⬜⬛⬛⬛
        ⬜⬛⬛⬛
        """;

    public const string G = """
        ⬛⬜⬜⬛
        ⬜⬛⬛⬜
        ⬜⬛⬛⬛
        ⬜⬛⬜⬜
        ⬜⬛⬛⬜
        ⬛⬜⬜⬜
        """;

    public const string H = """
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        ⬜⬜⬜⬜
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        """;

    public const string J = """
        ⬛⬛⬜⬜
        ⬛⬛⬛⬜
        ⬛⬛⬛⬜
        ⬛⬛⬛⬜
        ⬜⬛⬛⬜
        ⬛⬜⬜⬛
        """;

    public const string K = """
        ⬜⬛⬛⬜
        ⬜⬛⬜⬛
        ⬜⬜⬛⬛
        ⬜⬛⬜⬛
        ⬜⬛⬜⬛
        ⬜⬛⬛⬜
        """;

    public const string L = """
        ⬜⬛⬛⬛
        ⬜⬛⬛⬛
        ⬜⬛⬛⬛
        ⬜⬛⬛⬛
        ⬜⬛⬛⬛
        ⬜⬜⬜⬜
        """;

    public const string P = """
        ⬜⬜⬜⬛
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        ⬜⬜⬜⬛
        ⬜⬛⬛⬛
        ⬜⬛⬛⬛
        """;

    public const string R = """
        ⬜⬜⬜⬛
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        ⬜⬜⬜⬛
        ⬜⬛⬜⬛
        ⬜⬛⬛⬜
        """;

    public const string U = """
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        ⬜⬛⬛⬜
        ⬛⬜⬜⬛
        """;

    public const string Z = """
        ⬜⬜⬜⬜
        ⬛⬛⬛⬜
        ⬛⬛⬜⬛
        ⬛⬜⬛⬛
        ⬜⬛⬛⬛
        ⬜⬜⬜⬜
        """;
}
