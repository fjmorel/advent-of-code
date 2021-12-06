var timer = Stopwatch.StartNew();
var lines = File.ReadAllLines("input.txt");
// lines = File.ReadAllLines("example.txt");

var initial = lines[0].Split(',').GroupBy(x => x).Select(group => new Fish(group.Count(), int.Parse(group.Key)));

Console.WriteLine($"SETUP :: {timer.Elapsed}");
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1() => Enumerable.Range(1, 80).Aggregate(initial, RunDay).Sum(x => x.count);

long Part2() => Enumerable.Range(1, 256).Aggregate(initial, RunDay).Sum(x => x.count);

IEnumerable<Fish> RunDay(IEnumerable<Fish> list, int i)
	=> list
			.SelectMany(fish =>
			{
				if (fish.days < 1)
					return new Fish[] { fish with { days = 6 }, fish with { days = 8 } };
				else
					return new Fish[] { fish with { days = fish.days - 1 } };
			})
			.GroupBy(x => x.days)
			.Select(x => new Fish(x.Sum(x => x.count), x.Key));

public record Fish(long count, int days);
