var timer = Stopwatch.StartNew();
var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");

var reactions = ParseReactions();
const string ORE = "ORE";
const string FUEL = "FUEL";
const long TRILLION = 1_000_000_000_000;

Console.WriteLine($"_ :: {timer.Elapsed}");// setup time
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	var state = GetInitialState();
	Produce(new(1, FUEL), state);
	return state.OreNeeded;
}

long Part2()
{
	var state = GetInitialState();
	state.Supply[ORE] = TRILLION;

	// Run once to find out ore per run
	// Then run what's left divided by max ore per run to eliminate most of it
	long step = 1;
	Produce(new(step, FUEL), state);
	long produced = step;
	var maxOrePerFuel = TRILLION - state.Supply[ORE];

	while (state.Supply[ORE] > 0)
	{
		// As long as it produced the fuel, keep running
		step = Math.Max(state.Supply[ORE] / maxOrePerFuel, 1);
		Produce(new(step, FUEL), state);
		if (state.OreNeeded == 0)
			produced += step;
		else
			break;
	}

	return produced;
}

State GetInitialState()
{
	var lookup = new Dictionary<string, long>();
	lookup[ORE] = 0;
	foreach (var reagent in reactions.Keys)
		lookup[reagent] = 0;

	return new State(lookup);
}

void Produce(Reagent initialNeed, State state)
{
	var needs = new Queue<Reagent>();
	needs.Enqueue(initialNeed);
	while (needs.Count > 0)
	{
		var reagent = needs.Dequeue();
		ProduceReagent(reagent.chemical, reagent.quantity, state);
	}
}

void ProduceReagent(string chemical, long quantityNeeded, State state)
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

Dictionary<string, Formula> ParseReactions()
{
	var output = new Dictionary<string, Formula>();
	var regex = new Regex("(((?<reagent_count>[0-9]+) (?<reagent_name>[A-Za-z]+),? )+)=> (?<product>(?<product_count>[0-9]+) (?<product_name>[A-Za-z]+))");
	foreach (var line in list)
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