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
	var need = InitLookup();
	var have = InitLookup();
	need[FUEL] = 1;
	TryProduce(false, need, have);
	return need[ORE];
}

long Part2()
{
	var need = InitLookup();
	var have = InitLookup();
	have[ORE] = TRILLION;

	// Run once to find out ore per run
	// Then run what's left divided by max ore per run to eliminate most of it
	long step = 1;
	need[FUEL] = step;
	TryProduce(true, need, have);
	long produced = step;
	var maxOrePerFuel = TRILLION - have[ORE];

	while (have[ORE] > 0)
	{
		// As long as it produced the fuel, keep running
		step = Math.Max(have[ORE] / maxOrePerFuel, 1);
		need[FUEL] = step;
		if (TryProduce(true, need, have))
			produced += step;
		else
			break;
	}

	return produced;
}

Dictionary<string, long> InitLookup()
{
	var lookup = new Dictionary<string, long>();
	lookup[ORE] = 0;
	foreach (var reagent in reactions.Keys)
		lookup[reagent] = 0;
	return lookup;
}

bool TryProduce(bool quitIfNeedOre, Dictionary<string, long> need, Dictionary<string, long> have)
{
	while (need.Any(x => x.Value > 0 && x.Key != ORE))
	{
		var (product, needed) = need.Where(x => x.Value > 0 && x.Key != ORE).First();
		if (!TryProduceReagent(product, needed, quitIfNeedOre, need, have))
			return false;
	}
	return true;
}

bool TryProduceReagent(string chemical, long quantityNeeded, bool quitIfNeedOre, Dictionary<string, long> need, Dictionary<string, long> have)
{
	// If need more than we have,
	// Then use up supply and add need
	var currentHave = have[chemical];
	if (quantityNeeded > currentHave)
	{
		quantityNeeded -= currentHave;
		have[chemical] = 0;
	}
	// Otherwise, partially use up supply and don't run reaction
	else
	{
		have[chemical] = currentHave - quantityNeeded;
		need[chemical] = 0;
		return true;
	}

	// Figure out many reactions will be needed to produce it
	if (chemical != ORE)
	{
		var reaction = reactions[chemical];
		var output = reaction.output;
		while (output < quantityNeeded)
			output += reaction.output;
		var multiplier = output / reaction.output;

		foreach (var reagent in reaction.reagents)
		{
			var subChemical = reagent.chemical;
			var reagentNeeded = reagent.quantity * multiplier;
			if (!TryProduceReagent(subChemical, reagentNeeded, quitIfNeedOre, need, have))
				return false;
		}
		have[chemical] += output - quantityNeeded;
		need[chemical] = 0;
	}
	else
	{
		need[chemical] += quantityNeeded;
	}

	return true;
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
		var reagents = counts.Zip(names).Select((tuple) => new Reagent(tuple.Item1, tuple.Item2)).ToList();
		output[groups["product_name"].Value] = new(long.Parse(groups["product_count"].ValueSpan), reagents);
	}
	return output;
}

public readonly record struct Formula(long output, List<Reagent> reagents);
public readonly record struct Reagent(long quantity, string chemical);