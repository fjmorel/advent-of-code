namespace Puzzles2022.Day01;

public record Solution(List<long> _elves) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
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
        return new Solution(elves);
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
