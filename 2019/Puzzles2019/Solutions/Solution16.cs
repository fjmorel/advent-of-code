namespace Puzzles2019.Solutions;

public record Solution16(string[] _lines) : ISolution<Solution16>
{
    private readonly int[] pattern = { 0, 1, 0, -1 };

    public static Solution16 Init(string[] lines) => new(lines);

    public async ValueTask<long> GetPart1()
    {
        var offset = 0;
        var digits = ParseLine(_lines[0]);
        for (var phase = 1; phase <= 100; phase++)
        {
            // too slow for part 2
            for (var i = offset + 1; i <= digits.Length; i++)
            {
                var sum = 0;
                for (var j = i - 1; j < digits.Length; j++)
                {
                    switch (pattern[((j + 1) / i) % pattern.Length])
                    {
                        case 1:
                            sum += digits[j];
                            break;
                        case -1:
                            sum -= digits[j];
                            break;
                        default:
                            j += i - 1;
                            break;
                    }
                }

                digits[i - 1] = int.Abs(sum % 10);
            }
        }

        return GetMessage(digits, offset);
    }

    public async ValueTask<long> GetPart2()
    {
        const int repeated = 10_000;//10_000;
        var baseDigits = ParseLine(_lines[0]);
        var originalLength = baseDigits.Length;
        var digits = new int[originalLength * repeated];
        // 650 digits, repeated 10_000 times = 6_500_000 digits

        for (var i = 0; i < originalLength; i++)
        {
            for (var j = 0; j < repeated; j++)
            {
                digits[i + (j * originalLength)] = baseDigits[i];
            }
        }

        // 5_970_807. Far into the last half, so we can rely on the pattern being just 1 for digits starting at that point
        var offset = int.Parse(string.Join("", baseDigits.Take(7).Select(x => x.ToString())));

        for (var phase = 1; phase <= 100; phase++)
        {
            // skip last because it never changes
            // only works for offsets in 2nd half of number (part 2)
            for (var i = digits.Length - 2; i >= offset; i--)
            {
                digits[i] = (digits[i] + digits[i + 1]) % 10;
            }
        }

        return GetMessage(digits, offset);
    }

    private int[] ParseLine(string line) => line.Select(x => int.Parse(new string(x, 1))).ToArray();

    private long GetMessage(int[] final, int offset)
    {
        return long.Parse(string.Join("", final.Skip(offset).Take(8).Select(x => x.ToString())));
    }
}
