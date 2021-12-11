namespace Puzzles2019.Solutions;

public class Solution01 : ISolution
{
    private readonly List<long> _nums;

    public Solution01(string[] lines)
    {
        _nums = lines.ParseLongs();
    }

    public async ValueTask<long> GetPart1() => _nums.Select(GetFuel).Sum();
    public async ValueTask<long> GetPart2() => _nums.Select(GetFuel).Select(GetFuelForFuel).Sum();

    long GetFuel(long mass) => mass / 3 - 2;

    long GetFuelForFuel(long original)
    {
        var addedFuel = GetFuel(original);
        var totalFuel = original;
        while (addedFuel > 0)
        {
            totalFuel += addedFuel;
            addedFuel = GetFuel(addedFuel);
        }
        return totalFuel;
    }
}