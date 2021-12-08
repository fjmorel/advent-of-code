namespace Combined.Solutions;

public class Solution01 : ISolution
{
    private readonly List<int> _nums;

    public Solution01(string[] lines)
    {
        _nums = lines.ParseInts();
    }

    public async Task<long> GetPart1() => _nums.Skip(1).Select((x, i) => x > _nums[i]).LongCount(x => x);
    public async Task<long> GetPart2() => _nums.Skip(3).Select((x, i) => x > _nums[i]).LongCount(x => x);

}