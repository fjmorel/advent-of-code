namespace Puzzles2020.Solutions;

public record Solution01(List<int> _nums) : ISolution<Solution01>
{
    public static Solution01 Init(string[] lines) => new(lines.ParsePerLine<int>().ToList());

    public async ValueTask<long> GetPart1() => FindProduct(2);

    public async ValueTask<long> GetPart2() => FindProduct(3);

    private long FindProduct(int numValues)
    {
        IEnumerable<ImmutableList<int>> combinations = _nums.Select(x => new List<int> { x }.ToImmutableList());
        for (var i = 2; i <= numValues; i++)
            combinations = combinations.SelectMany(list => _nums.Where(z => !list.Contains(z)).Select(z => list.Add(z)));

        return combinations
            .Where(list => list.Sum() == 2020)
            .Select(list => list.Aggregate(1, (acc, x) => acc * x))
            .First();
    }
}
