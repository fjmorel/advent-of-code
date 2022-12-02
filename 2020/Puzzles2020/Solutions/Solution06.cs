namespace Puzzles2020.Solutions;

public record Solution06(long part1, long part2) : ISolution<Solution06>
{
    public static Solution06 Init(string[] lines)
    {
        long part1 = 0, part2 = 0;
        var data = string.Join('\n', lines)
            .Split("\n\n")
            .Select(x => x.Split('\n'));
        foreach (var group in data)
        {
            var charAnswers = new Dictionary<char, int>();
            foreach (var line in group)
            foreach (var ch in line)
                charAnswers[ch] = charAnswers.GetValueOrDefault(ch) + 1;

            part1 += charAnswers.Count;
            part2 += charAnswers.Count(x => x.Value == group.Length);
        }

        return new(part1, part2);
    }

    public async ValueTask<long> GetPart1() => part1;

    public async ValueTask<long> GetPart2() => part2;
}
