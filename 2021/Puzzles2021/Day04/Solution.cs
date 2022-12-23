namespace Puzzles2021.Day04;

public record Solution(int[] numbers, Dictionary<int, int> lookup, List<int[]> cards) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var numbers = lines[0].ParseCsv<int>();
        var lookup = numbers.Select((num, i) => (num, i)).ToDictionary(x => x.num, x => x.i);
        var cards = lines
            .Skip(1).Chunk(6)
            .Select(sixLines => sixLines.SelectMany(line => Regex.Matches(line, "([0-9]+)").Select(x => int.Parse(x.ValueSpan))).ToArray())
            .ToList();
        return new(numbers, lookup, cards);
    }

    public async ValueTask<long> GetPart1() => GetScore(cards.MinBy(CountNeededToWin)!);

    public async ValueTask<long> GetPart2() => GetScore(cards.MaxBy(CountNeededToWin)!);

    private long GetScore(int[] card)
    {
        var needed = CountNeededToWin(card);
        return card.Except(numbers[0..needed]).Sum() * numbers[needed - 1];
    }

    private int CountNeededToWin(int[] card) => Enumerable.Range(1, numbers.Length).FirstOrDefault(i => HasWinningLine(card, i), -1);

    private bool HasWinningLine(int[] card, int numDrawn) =>
        Enumerable.Range(0, 5).Any(i =>
            Enumerable.Range(0, 5).All(j => lookup[card[(i * 5) + j]] < numDrawn)
            || Enumerable.Range(0, 5).All(j => lookup[card[(j * 5) + i]] < numDrawn)
        );
}
