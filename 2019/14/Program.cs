var timer = Stopwatch.StartNew();
var list = File.ReadAllLines("input.txt");
list = File.ReadAllLines("example.txt");

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
	var need = new Dictionary<string, long>();
	var have = new Dictionary<string, long>();
	need[FUEL] = 1;
	Produce(need, have);
	return need[ORE];
}

long Part2()
{
	var need = new Dictionary<string, long>();
	var have = new Dictionary<string, long>();
	have[ORE] = TRILLION;

	// Run once to find out ore per run
	// Then run what's left divided by max ore per run to eliminate most of it
	long step = 1;
	need[FUEL] = step;
	Produce(need, have);
	long produced = 0;// instead of 'step' to avoid OBOE for some reason
	var maxOrePerFuel = TRILLION - have[ORE];

	step = have[ORE] / maxOrePerFuel;
	need[FUEL] = step;
	while (have[ORE] > 0)
	{
		// As long as it produced the fuel, keep running
		Produce(need, have, true);
		if (!need.ContainsKey(FUEL))
		{
			produced += step;
			step = Math.Max(have[ORE] / maxOrePerFuel, 1);
			need[FUEL] = step;
		}
		else
			break;
	}

	return produced;
}

void Produce(Dictionary<string, long> need, Dictionary<string, long> have, bool quitIfNeedOre = false)
{
	while (need.Any(x => x.Key != ORE))
	{
		// Pick a thing we still need
		var entry = need.First(x => x.Key != ORE);
		var product = entry.Key;
		var needed = entry.Value;

		// Figure out if we already have some leftover we can use
		if (have.ContainsKey(product))
		{
			var currentHave = have[product];
			if (needed >= currentHave)
			{
				needed -= currentHave;
				have.Remove(product);
			}
			else
			{
				have[product] = currentHave - needed;
				continue;
			}
		}

		// Figure out many reactions will be needed to produce it
		var reaction = reactions[product];
		var output = reaction.output;
		while (output < needed)
			output += reaction.output;
		var multiplier = output / reaction.output;

		foreach (var reagent in reaction.reagents)
		{
			var chemical = reagent.chemical;
			var reagentNeeded = reagent.quantity * multiplier;
			if (!TryHandleChemical(chemical, reagentNeeded, quitIfNeedOre, need, have))
				return;
		}

		have[product] = have.GetValueOrDefault(product) + (output - needed);
		need.Remove(product);
	}
}

bool TryHandleChemical(string chemical, long quantityNeeded, bool quitIfNeedOre, Dictionary<string, long> need, Dictionary<string, long> have)
{
	var alreadyHave = have.GetValueOrDefault(chemical);
	if (alreadyHave > 0)
	{
		if (quantityNeeded >= alreadyHave)
		{
			if (quitIfNeedOre && chemical == ORE)
				return false;
			quantityNeeded -= alreadyHave;
			need[chemical] = need.GetValueOrDefault(chemical) + quantityNeeded;
			have.Remove(chemical);
		}
		else
		{
			have[chemical] = alreadyHave - quantityNeeded;
		}
	}
	else
	{
		if (quitIfNeedOre && chemical == ORE)
			return false;
		need[chemical] = need.GetValueOrDefault(chemical) + quantityNeeded;
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