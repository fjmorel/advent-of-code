namespace Puzzles2021.Solutions;

public class Solution06 : ISolution
{
	private readonly IEnumerable<Fish> initial;

	public Solution06(string[] lines)
	{
		initial = lines[0].Split(',').GroupBy(x => x).Select(group => new Fish(group.Count(), int.Parse(group.Key)));
	}

	public async ValueTask<long> GetPart1() => Enumerable.Range(1, 80).Aggregate(initial, RunDay).Sum(x => x.count);

	public async ValueTask<long> GetPart2() => Enumerable.Range(1, 256).Aggregate(initial, RunDay).Sum(x => x.count);

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
}
