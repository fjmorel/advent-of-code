namespace Combined.Solutions;

public class Solution04 : ISolution
{
	private readonly int[] numbers;
	private readonly Dictionary<int, int> lookup;
	private readonly List<int[]> cards;

	public Solution04(string[] lines)
	{
		numbers = lines[0].ParseCsvInts();
		lookup = numbers.Select((num, i) => (num, i)).ToDictionary(x => x.num, x => x.i);
		cards = lines
			.Skip(1).Chunk(6)
			.Select(sixLines => sixLines.SelectMany(line => Regex.Matches(line, "([0-9]+)").Select(x => int.Parse(x.ValueSpan))).ToArray())
			.ToList();
	}

	public async ValueTask<long> GetPart1() => GetScore(cards.MinBy(CountNeededToWin)!);

	public async ValueTask<long> GetPart2() => GetScore(cards.MaxBy(CountNeededToWin)!);

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
}

