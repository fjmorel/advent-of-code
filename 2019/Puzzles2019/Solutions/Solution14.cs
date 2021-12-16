namespace Puzzles2019.Solutions;

public class Solution14 : ISolution
{
    private readonly Dictionary<string, Formula> reactions;
    private const string ORE = "ORE";
    private const string FUEL = "FUEL";
    private const long TRILLION = 1_000_000_000_000;

    public Solution14(string[] lines)
    {
        reactions = ParseReactions(lines);
    }

    public async ValueTask<long> GetPart1()
    {
        var state = GetInitialState();
        ProduceReagent(FUEL, 1, state);
        return state.OreNeeded;
    }

    public async ValueTask<long> GetPart2()
    {
        var state = GetInitialState();
        state.Supply[ORE] = TRILLION;

        // Run once to find out ore per run
        // Then run what's left divided by max ore per run to eliminate most of it
        long step = 1;
        ProduceReagent(FUEL, step, state);
        long produced = step;
        var maxOrePerFuel = TRILLION - state.Supply[ORE];

        while (state.Supply[ORE] > 0)
        {
            // As long as it produced the fuel, keep running
            step = Math.Max(state.Supply[ORE] / maxOrePerFuel, 1);
            ProduceReagent(FUEL, step, state);
            if (state.OreNeeded == 0)
                produced += step;
            else
                break;
        }

        return produced;
    }

    private State GetInitialState()
    {
        var lookup = new Dictionary<string, long>();
        lookup[ORE] = 0;
        foreach (var reagent in reactions.Keys)
            lookup[reagent] = 0;

        return new State(lookup);
    }

    private void ProduceReagent(string chemical, long quantityNeeded, State state)
    {
        // If need more than we have,
        // Then use up supply and add need
        var currentHave = state.Supply[chemical];
        if (quantityNeeded > currentHave)
        {
            quantityNeeded -= currentHave;
            state.Supply[chemical] = 0;
        }
        // Otherwise, partially use up supply and don't run reaction
        else
        {
            state.Supply[chemical] = currentHave - quantityNeeded;
            return;
        }

        // Figure out many reactions will be needed to produce it
        if (chemical != ORE)
        {
            var reaction = reactions[chemical];
            // Feels weird to go overboard with *2 and then do a second loop, but it greatly speeds up Part 2
            var output = reaction.output;
            while (output < quantityNeeded)
                output *= 2;
            while (output >= quantityNeeded + reaction.output)
                output -= reaction.output;
            var multiplier = output / reaction.output;

            foreach (var reagent in reaction.reagents)
                ProduceReagent(reagent.chemical, reagent.quantity * multiplier, state);
            state.Supply[chemical] += output - quantityNeeded;
        }
        else
        {
            state.OreNeeded += quantityNeeded;
        }
    }

    private Dictionary<string, Formula> ParseReactions(string[] lines)
    {
        var output = new Dictionary<string, Formula>();
        var regex = new Regex("(((?<reagent_count>[0-9]+) (?<reagent_name>[A-Za-z]+),? )+)=> (?<product>(?<product_count>[0-9]+) (?<product_name>[A-Za-z]+))");
        foreach (var line in lines)
        {
            var match = regex.Match(line);
            var groups = match.Groups;
            var counts = groups["reagent_count"].Captures.Select(x => long.Parse(x.ValueSpan));
            var names = groups["reagent_name"].Captures.Select(x => x.Value);
            var reagents = counts.Zip(names).Select((tuple) => new Reagent(tuple.Item1, tuple.Item2)).ToArray();
            output[groups["product_name"].Value] = new(long.Parse(groups["product_count"].ValueSpan), reagents);
        }

        return output;
    }

    public record State(Dictionary<string, long> Supply)
    {
        public long OreNeeded { get; set; }
    }

    public readonly record struct Formula(long output, Reagent[] reagents);

    public readonly record struct Reagent(long quantity, string chemical);
}
