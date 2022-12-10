namespace Puzzles2021.Solutions;

public record Solution24(long _lowest, long _highest) : ISolution<Solution24>
{
    public static Solution24 Init(string[] lines)
    {
        var groups = lines.Chunk(18).Select(ParseGroup).ToList();
        var (lowest, highest) = FindLimits(groups);
        return new(lowest, highest);
    }

    private static Group ParseGroup(string[] lines) => new(GetValue(lines[4]), GetValue(lines[5]), GetValue(lines[15]));
    private static int GetValue(string line) => int.Parse(line.Split(' ')[2]);

    public async ValueTask<long> GetPart1() => _highest;

    public async ValueTask<long> GetPart2() => _lowest;

    public static (long lowest, long highest) FindLimits(List<Group> groups)
    {
        (long lowest, long highest) = (long.MaxValue, long.MinValue);
        for (long model = 11_111_111_111_111; model <= 99_999_999_999_999; model++)
        {
            var digits = model.GetDigits(true);
            int step = 0;
            long output = 0;

            foreach ((int line5, int line6, int line16) in groups)
            {
                var input = digits[step];
                var test = (output % 26) + line6 == input;
                if (input != 0 && line5 == 26 && test)
                {
                    output = output / line5;
                }
                else if (input != 0 && line5 == 1 && !test)
                {
                    output = 26 * (output / line5) + input + line16;
                }
                else
                {
                    // Skip this range of numbers because this digit will not lead to a solution
                    model += (long)double.Pow(10, 13 - step) - 1;
                    break;
                }

                step++;
            }

            if (output == 0)
            {
                (lowest, highest) = (long.Min(model, lowest), long.Max(model, highest));
            }
        }

        return (lowest, highest);
    }

    public static long ProcessGroup(long w, long z, Group group)
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

    public readonly record struct Group(int line5, int line6, int line16);
}
