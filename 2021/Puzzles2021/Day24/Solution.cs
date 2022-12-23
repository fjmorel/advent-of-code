namespace Puzzles2021.Day24;

public record Solution(List<Solution.InstructionGroup> _groups) : ISolution<Solution>
{
    private static readonly ImmutableArray<int> ALL_DIGITS = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.ToImmutableArray();
    private static readonly ImmutableArray<int> REVERSE_DIGITS = ALL_DIGITS.Reverse().ToImmutableArray();

    public static Solution Init(string[] lines) => new(lines.Chunk(18).Select(InstructionGroup.Parse).ToList());

    public async ValueTask<string> GetPart1String() => FindLimit(REVERSE_DIGITS, ImmutableList<int>.Empty, 0) ?? "";

    public async ValueTask<string> GetPart2String() => FindLimit(ALL_DIGITS, ImmutableList<int>.Empty, 0) ?? "";

    public string? FindLimit(ImmutableArray<int> validDigits, ImmutableList<int> modelNumberDigits, long output)
    {
        int step = modelNumberDigits.Count - 1;
        if (step >= 0)
        {
            var input = modelNumberDigits[step];
            (int line5, int line6, int line16) = _groups[step];

            var test = (output % 26) + line6 == input;
            if (input != 0 && line5 == 26 && test)
                output /= line5;
            else if (input != 0 && line5 == 1 && !test)
                output = 26 * output + input + line16;
            else// the rest of the digits will never make a valid number
                return null;

            if (output == 0 && modelNumberDigits.Count == 14)
                return string.Join("", modelNumberDigits.Select(x => x.ToString()));
        }

        foreach (var digit in validDigits)
        {
            var digits = modelNumberDigits.Add(digit);
            var result = FindLimit(validDigits, digits, output);
            if (result is not null)
                return result;
        }

        return null;
    }

    public static long ProcessGroup(long w, long z, InstructionGroup group)
    {
        long x = 0, y = 0;
        x += z;
        x %= 26;
        z /= group.line5;
        x += group.line6;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y += 25;
        y *= x;
        y += 1;
        z *= y;
        y = 0;
        y += w;
        y += group.line16;
        y *= x;
        z += y;
        return z;
    }

    public readonly record struct InstructionGroup(int line5, int line6, int line16)
    {
        public static InstructionGroup Parse(string[] lines) =>
            new(int.Parse(lines[4].AsSpan()[6..]), int.Parse(lines[5].AsSpan()[6..]), int.Parse(lines[15].AsSpan()[6..]));
    }
}
