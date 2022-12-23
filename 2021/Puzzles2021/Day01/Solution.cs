namespace Puzzles2021.Day01;

public record Solution(List<int> _nums) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines.ParsePerLine<int>());

    public async ValueTask<long> GetPart1() => _nums.Skip(1).Select((x, i) => x > _nums[i]).LongCount(x => x);
    public async ValueTask<long> GetPart2() => _nums.Skip(3).Select((x, i) => x > _nums[i]).LongCount(x => x);
}
