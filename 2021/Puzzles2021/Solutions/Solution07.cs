namespace Puzzles2021.Solutions;

public record Solution07(int[] nums, int min, int max) : ISolution<Solution07>
{
    public static Solution07 Init(string[] lines)
    {
        var nums = lines[0].ParseCsvInts();
        return new(nums, nums.Min(), nums.Max());
    }

    public async ValueTask<long> GetPart1() => GetBestPosition(i => nums.Select(x => Math.Abs(x - i)).Sum());

    public async ValueTask<long> GetPart2() => GetBestPosition(i => nums.Select(x => Math.Abs(x - i)).Select(x => x * (x + 1) / 2).Sum());

    private long GetBestPosition(Func<int, long> calculator) => Enumerable.Range(min, max - min + 1).Min(calculator);
}
