var timer = Stopwatch.StartNew();
var lines = File.ReadAllLines("input.txt");
// lines = File.ReadAllLines("example.txt");
var numbers = lines[0].Split(',').Select(x => int.Parse(x)).ToArray();
var lookup = numbers.Select((num, i) => (num, i)).ToDictionary(x => x.num, x => x.i);
var cards = lines
	.Skip(1).Chunk(6)
	.Select(sixLines => sixLines.SelectMany(line => Regex.Matches(line, "([0-9]+)").Select(x => int.Parse(x.ValueSpan))).ToArray())
	.ToList();

Console.WriteLine($"_ :: {timer.Elapsed}");// setup time
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1() => GetScore(cards.MinBy(CountNeededToWin)!);

long Part2() => GetScore(cards.MaxBy(CountNeededToWin)!);

long GetScore(int[] card)
{
	var needed = CountNeededToWin(card);
	return card.Except(numbers[0..needed]).Sum() * numbers[needed - 1];
}

int CountNeededToWin(int[] card) => Enumerable.Range(1, numbers.Length).FirstOrDefault(i => HasWinningLine(card, i), -1);

bool HasWinningLine(int[] card, int numDrawn) =>
	Enumerable.Range(0, 5).Any(i =>
		Enumerable.Range(0, 5).All(j => lookup[card[(i * 5) + j]] < numDrawn)
		|| Enumerable.Range(0, 5).All(j => lookup[card[(j * 5) + i]] < numDrawn)
	);
