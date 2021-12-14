namespace Puzzles2020.Solutions;

public class Solution01 : ISolution
{
    private readonly List<int[]> _nums;

    public Solution01(string[] lines)
    {
        _nums = lines.ParseInts().Select(x => new int[] { x }).ToList();

    }

    public async ValueTask<long> GetPart1() => FindProduct(2);

    public async ValueTask<long> GetPart2() => FindProduct(3);

    private long FindProduct(int numValues)
    {
        IEnumerable<IEnumerable<int>> combinations = _nums;
        for (var i = 2; i <= numValues; i++)
            combinations = combinations.SelectMany(list => _nums.Where(z => !list.Contains(z[0])).Select(z => list.Concat(z)));

        return combinations
            .Where(list => list.Sum() == 2020)
            .Select(list => list.Aggregate(1, (acc, x) => acc * x))
            .First();
    }
}
