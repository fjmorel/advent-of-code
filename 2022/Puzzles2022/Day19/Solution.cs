namespace Puzzles2022.Day19;

using Blueprint = Dictionary<Solution.Mineral, Dictionary<Solution.Mineral, int>>;

public partial record Solution(List<Blueprint> _blueprints) : ISolution<Solution>
{
    private static readonly Mineral[] MINERALS = { Mineral.geode, Mineral.obsidian, Mineral.clay, Mineral.ore };

    public static Solution Init(string[] lines) => new(lines.ToList(ParseLine));

    private static Blueprint ParseLine(string line)
    {
        var matches = GetBlueprintRegex().Matches(line);
        var blueprint = new Dictionary<Mineral, Dictionary<Mineral, int>>();
        foreach (Match match in matches)
        {
            var mineral = Enum.Parse<Mineral>(match.Groups[1].ValueSpan);
            var robot = blueprint[mineral] = new();
            var costs = GetRobotRegex().Matches(match.Groups[2].Value);
            foreach (Match cost in costs)
            {
                robot.Add(Enum.Parse<Mineral>(cost.Groups[2].ValueSpan), int.Parse(cost.Groups[1].ValueSpan));
            }
        }

        return blueprint;
    }

    public async ValueTask<long> GetPart1()
    {
        const int time = 24;
        var startingResources = ImmutableDictionary<Mineral, int>.Empty
            .SetItem(Mineral.ore, 0)
            .SetItem(Mineral.clay, 0)
            .SetItem(Mineral.obsidian, 0)
            .SetItem(Mineral.geode, 0);
        var startingRobots = ImmutableDictionary<Mineral, int>.Empty
            .SetItem(Mineral.ore, 1)
            .SetItem(Mineral.clay, 0)
            .SetItem(Mineral.obsidian, 0)
            .SetItem(Mineral.geode, 0);
        var initialState = new State(startingResources, startingRobots);
        return _blueprints
            .Select((t, i) => GetMaxProduction(t, time, initialState) * (i + 1))
            .Sum();
    }

    public async ValueTask<long> GetPart2()
    {
        return 0;
    }

    public static long GetMaxProduction(Blueprint blueprint, int minutesLeft, State state)
    {
        long max = state.resources[Mineral.geode];
        if (minutesLeft < 1)
            return max;
        var nextIteration = minutesLeft - 1;

        // Simulate just mining as well as attempting to make each robot
        long.Max(max, GetMaxProduction(blueprint, nextIteration, state.Mine()));
        foreach (var mineral in MINERALS)
            if (state.CanProduceRobot(mineral, blueprint))
                long.Max(max, GetMaxProduction(blueprint, nextIteration, state.CreateRobot(blueprint, mineral).Mine()));

        return max;
    }

    private static ImmutableDictionary<Mineral, int> Adjust(ImmutableDictionary<Mineral, int> dictionary, Mineral mineral, int value) =>
        dictionary.SetItem(mineral, dictionary[mineral] + value);

    public readonly record struct State(ImmutableDictionary<Mineral, int> resources, ImmutableDictionary<Mineral, int> robots)
    {
        public bool CanProduceRobot(Mineral minerMineral, Blueprint blueprint)
        {
            var resources = this.resources;
            foreach (var mineral in MINERALS)
                if (resources[mineral] < blueprint[minerMineral].GetValueOrDefault(mineral))
                    return false;
            return true;
        }

        public State Mine()
        {
            var (resources, robots) = this;
            foreach (var mineral in MINERALS)
                resources = Adjust(resources, mineral, robots[mineral]);

            return new(resources, robots);
        }

        public State CreateRobot(Blueprint blueprint, Mineral minerMineral)
        {
            var (resources, robots) = this;
            foreach (var mineral in MINERALS)
                resources = Adjust(resources, mineral, blueprint[minerMineral].GetValueOrDefault(mineral));

            robots = Adjust(robots, minerMineral, 1);

            return new(resources, robots);
        }
    }

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
