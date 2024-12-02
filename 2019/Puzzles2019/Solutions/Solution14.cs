namespace Puzzles2019.Solutions;

public partial record Solution14(Dictionary<string, Solution14.Formula> reactions) : ISolution<Solution14>
{
    private const string ORE = "ORE";
    private const string FUEL = "FUEL";
    private const long TRILLION = 1_000_000_000_000;
    private long _orePerFuel;

    public static Solution14 Init(string[] lines) => new(ParseReactions(lines));

    public async ValueTask<long> GetPart1()
    {
        var state = GetInitialState();
        _orePerFuel = ProduceReagent(FUEL, 1, state);
        return _orePerFuel;
    }

    public async ValueTask<long> GetPart2()
    {
        if (_orePerFuel == 0)
            await GetPart1();

        var supply = GetInitialState();
        supply[ORE] = TRILLION;

        // Then run what's left divided by max ore per run to eliminate most of it
        long produced = 0;
        while (supply[ORE] > 0)
        {
            // As long as it produced the fuel, keep running
            long step = long.Max(supply[ORE] / _orePerFuel, 1);
            var oreNeeded = ProduceReagent(FUEL, step, supply);
            if (oreNeeded == 0)
                produced += step;
            else
                break;
        }

        return produced;
    }

    private Dictionary<string, long> GetInitialState()
    {
        var lookup = new Dictionary<string, long> { [ORE] = 0 };
        foreach (var reagent in reactions.Keys)
            lookup[reagent] = 0;

        return lookup;
    }

    private long ProduceReagent(string chemical, long quantityNeeded, Dictionary<string, long> supply)
    {
        var currentHave = supply[chemical];

        // Partially use up supply and don't run reaction if we have enough
        if (quantityNeeded <= currentHave)
        {
            supply[chemical] = currentHave - quantityNeeded;
            return 0;
        }

        // Otherwise use up all of it, then create the leftover need
        quantityNeeded -= currentHave;
        supply[chemical] = 0;

        if (chemical == ORE)
            return quantityNeeded;

        // Figure out many reactions will be needed to produce it
        var reaction = reactions[chemical];
        var multiplier = quantityNeeded / reaction.output + 1;
        if (reaction.output * multiplier >= quantityNeeded + reaction.output)
            multiplier--;
        var output = reaction.output * multiplier;

        var oreNeeded = 0L;
        foreach (var reagent in reaction.reagents)
            oreNeeded += ProduceReagent(reagent.name, reagent.quantity * multiplier, supply);

        supply[chemical] += output - quantityNeeded;
        return oreNeeded;
    }

    private static Dictionary<string, Formula> ParseReactions(string[] lines)
    {
        var output = new Dictionary<string, Formula>();
        foreach (var line in lines)
        {
            var match = ParseRegex.Match(line);
            var groups = match.Groups;
            var counts = groups["reagent_count"].Captures.Select(x => long.Parse(x.ValueSpan));
            var names = groups["reagent_name"].Captures.Select(x => x.Value);
            var reagents = counts.Zip(names).Select((tuple) => new Reagent(tuple.Item1, tuple.Item2)).ToArray();
            output[groups["product_name"].Value] = new(long.Parse(groups["product_count"].ValueSpan), reagents);
        }

        return output;
    }

    public readonly record struct Formula(long output, Reagent[] reagents);

    public readonly record struct Reagent(long quantity, string name);

    [GeneratedRegex("(((?<reagent_count>[0-9]+) (?<reagent_name>[A-Za-z]+),? )+)=> (?<product>(?<product_count>[0-9]+) (?<product_name>[A-Za-z]+))")]
    private static partial Regex ParseRegex { get; }
}
