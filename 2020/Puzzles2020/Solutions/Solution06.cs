namespace Puzzles2020.Solutions;

public class Solution06 : ISolution
{
    private readonly long part1;
    private readonly long part2;

    public Solution06(string[] lines)
    {
        var data = string.Join('\n', lines)
            .Split("\n\n")
            .Select(x => x.Split('\n'));
        foreach (var group in data)
        {
            var charAnswers = new Dictionary<char, int>();
            foreach (var line in group)
            foreach (var ch in line)
                charAnswers[ch] = charAnswers.GetValueOrDefault(ch) + 1;

            part1 += charAnswers.Count();
            part2 += charAnswers.Count(x => x.Value == group.Count());
        }
    }

    public async ValueTask<long> GetPart1() => part1;

    public async ValueTask<long> GetPart2() => part2;
}
