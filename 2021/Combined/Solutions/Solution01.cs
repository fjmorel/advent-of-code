namespace Combined.Solutions;

public class Solution01 : ISolution
{
    private readonly List<int> _nums;

    public Solution01(string[] lines)
    {
        _nums = lines.Select(x => int.Parse(x)).ToList();
    }

    public Task<long> GetPart1() => Task.FromResult(_nums.Skip(1).Select((x, i) => x > _nums[i]).LongCount(x => x));
    public Task<long> GetPart2() => Task.FromResult(_nums.Skip(3).Select((x, i) => x > _nums[i]).LongCount(x => x));

}