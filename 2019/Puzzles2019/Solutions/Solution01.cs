namespace Puzzles2019.Solutions;

public record Solution01(List<long> _nums) : ISolution<Solution01>
{
    public static Solution01 Init(string[] lines) => new(lines.ParsePerLine<long>());

    public async ValueTask<long> GetPart1() => _nums.Select(GetFuel).Sum();
    public async ValueTask<long> GetPart2() => _nums.Select(GetFuel).Select(GetFuelForFuel).Sum();

    private long GetFuel(long mass) => mass / 3 - 2;

    private long GetFuelForFuel(long original)
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
