namespace Puzzles2022.Solutions;

public class Solution01 : ISolution
{
    private readonly List<long> _elves = new();

    public Solution01(string[] lines)
    {
        var current = 0L;
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                _elves.Add(current);
                current = 0;
            }
            else
            {
                current += int.Parse(line);
            }
        }

        _elves.Add(current);
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
