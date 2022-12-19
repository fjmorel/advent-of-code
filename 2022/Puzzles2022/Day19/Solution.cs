namespace Puzzles2022.Day19;

public partial record Solution(List<Dictionary<Solution.Mineral, List<Solution.Cost>>> _blueprints) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        return new Solution(lines.ToList(ParseLine));
    }

    private static Dictionary<Mineral, List<Cost>> ParseLine(string line)
    {
        var matches = GetBlueprintRegex().Matches(line);
        var blueprint = new Dictionary<Mineral, List<Cost>>();
        foreach (Match match in matches)
        {
            var mineral = Enum.Parse<Mineral>(match.Groups[1].ValueSpan);
            var robot = blueprint[mineral] = new();
            var costs = GetRobotRegex().Matches(match.Groups[2].Value);
            foreach (Match cost in costs)
            {
                robot.Add(new(Enum.Parse<Mineral>(cost.Groups[2].ValueSpan), int.Parse(cost.Groups[1].ValueSpan)));
            }
        }

        return blueprint;
    }

    public async ValueTask<long> GetPart1()
    {
        return 0;
    }

    public async ValueTask<long> GetPart2()
    {
        return 0;
    }

    public record struct Cost(Mineral mineral, int amount);

    [GeneratedRegex("Each ([a-z]+) robot costs ([a-z0-9 ]+).")]
    private static partial Regex GetBlueprintRegex();

    [GeneratedRegex("([0-9]+) ([a-z]+)")]
    private static partial Regex GetRobotRegex();

    public enum Mineral
    {
        ore,
        clay,
        obsidian,
        geode,
    }
}
