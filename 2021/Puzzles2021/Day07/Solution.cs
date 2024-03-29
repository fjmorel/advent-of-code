namespace Puzzles2021.Day07;

public record Solution(int[] nums, int min, int max) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var nums = lines[0].ParseCsv<int>();
        return new(nums, nums.Min(), nums.Max());
    }

    public async ValueTask<long> GetPart1() => GetBestPosition(i => nums.Select(x => int.Abs(x - i)).Sum());

    public async ValueTask<long> GetPart2() => GetBestPosition(i => nums.Select(x => int.Abs(x - i)).Select(x => x * (x + 1) / 2).Sum());

    private long GetBestPosition(Func<int, long> calculator) => Enumerable.Range(min, max - min + 1).Min(calculator);
}
