namespace Combined.Solutions;

public class Solution07 : ISolution
{
	private readonly int[] nums;
	private readonly int min;
	private readonly int max;

	public Solution07(string[] lines)
	{
		nums = lines[0].ParseCsvInts();
		min = nums.Min();
		max = nums.Max();
	}

	public async ValueTask<long> GetPart1() => GetBestPosition(i => nums.Select(x => Math.Abs(x - i)).Sum());

	public async ValueTask<long> GetPart2() => GetBestPosition(i => nums.Select(x => Math.Abs(x - i)).Select(x => x * (x + 1) / 2).Sum());

	long GetBestPosition(Func<int, long> calculator) => Enumerable.Range(min, max - min + 1).Min(calculator);
}

