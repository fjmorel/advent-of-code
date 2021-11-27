var timer = Stopwatch.StartNew();
var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");

var reactions = ParseReactions();

Console.WriteLine($"_ :: {timer.Elapsed}");// setup time
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	var need = new Dictionary<string, long>();
	var have = new Dictionary<string, long>();
	need["FUEL"] = 1;
	Produce(need, have);
	return need["ORE"];
}

long Part2()
{
	var maxOrePerFuel = Part1();
	var need = new Dictionary<string, long>();
	var have = new Dictionary<string, long>();
	have["ORE"] = 1_000_000_000_000;
	long count = have["ORE"] / maxOrePerFuel;
	need["FUEL"] = count;
	count--;
	while (have["ORE"] > 0)
	{
		var completed = Produce(need, have, true);
		if (completed)
		{
			count++;
			need["FUEL"] = 1;
		}
		else
			break;
	}

	return count;
}

bool Produce(Dictionary<string, long> need, Dictionary<string, long> have, bool quitIfNeedOre = false)
{
	while (need.Any(x => x.Key != "ORE"))
	{
		// Pick a thing we still need
		var entry = need.First(x => x.Key != "ORE");
		var product = entry.Key;
		var needed = entry.Value;
		need.Remove(product);

		// Figure out if we already have some leftover we can use
		if (have.ContainsKey(product))
		{
			if (needed >= have[product])
			{
				needed -= have[product];
				have.Remove(product);
			}
			else
			{
				have[product] = have[product] - needed;
				continue;
			}
		}

		// Figure out many reactions will be needed to produce it
		var reaction = reactions[product];
		var multiplier = 1;
		while (reaction.output * multiplier < needed)
			multiplier++;

		var output = reaction.output * multiplier;
		foreach (var reagent in reaction.reagents)
		{
			var chemical = reagent.chemical;
			var reagentNeeded = reagent.quantity * multiplier;
			// Figure out if we already have some leftover we can use
			if (have.ContainsKey(chemical))
			{
				if (reagentNeeded >= have[chemical])
				{
					if (quitIfNeedOre && chemical == "ORE")
						return false;
					reagentNeeded -= have[chemical];
					have.Remove(chemical);
					need[chemical] = need.GetValueOrDefault(chemical) + reagentNeeded;
				}
				else
				{
					have[chemical] = have[chemical] - reagentNeeded;
					continue;
				}
			}
			else
			{
				if (quitIfNeedOre && chemical == "ORE")
					return false;
				need[chemical] = need.GetValueOrDefault(chemical) + reagentNeeded;
			}
		}

		have[product] = have.GetValueOrDefault(product) + (output - needed);
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