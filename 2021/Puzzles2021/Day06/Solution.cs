namespace Puzzles2021.Day06;

public record Solution(IEnumerable<Solution.Fish> initial) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var initial = lines[0].Split(',').GroupBy(x => x).Select(group => new Fish(group.Count(), int.Parse(group.Key)));
        return new(initial);
    }

    public async ValueTask<long> GetPart1() => Enumerable.Range(1, 80).Aggregate(initial, RunDay).Sum(x => x.count);

    public async ValueTask<long> GetPart2() => Enumerable.Range(1, 256).Aggregate(initial, RunDay).Sum(x => x.count);

    private IEnumerable<Fish> RunDay(IEnumerable<Fish> list, int i) => list
        .SelectMany(fish =>
        {
            if (fish.days < 1)
                return [fish with { days = 6 }, fish with { days = 8 }];
            return new Fish[] { fish with { days = fish.days - 1 } };
        })
        .GroupBy(x => x.days)
        .Select(g => new Fish(g.Sum(fish => fish.count), g.Key));

    public record Fish(long count, int days);
}
