namespace Puzzles2021.Solutions;

public record Solution01(List<int> _nums) : ISolution<Solution01>
{
    public static Solution01 Init(string[] lines) => new(lines.ParseInts());

    public async ValueTask<long> GetPart1() => _nums.Skip(1).Select((x, i) => x > _nums[i]).LongCount(x => x);
    public async ValueTask<long> GetPart2() => _nums.Skip(3).Select((x, i) => x > _nums[i]).LongCount(x => x);
}
