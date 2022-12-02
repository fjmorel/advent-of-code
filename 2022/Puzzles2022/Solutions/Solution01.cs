namespace Puzzles2022.Solutions;

public record Solution01(List<long> _elves) : ISolution<Solution01>
{
    public static Solution01 Init(string[] lines)
    {
        var elves = new List<long>();
        var current = 0L;
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                elves.Add(current);
                current = 0;
            }
            else
            {
                current += int.Parse(line);
            }
        }

        elves.Add(current);
        return new Solution01(elves);
    }

    public async ValueTask<long> GetPart1()
    {
        return _elves.Max();
    }

    public async ValueTask<long> GetPart2()
    {
        return _elves.OrderDescending().Take(3).Sum();
    }
}
