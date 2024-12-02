namespace Puzzles2024.Day01;

public record Solution(List<long> left, List<long> right) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        List<long> left = [], right = [];
        foreach (var line in lines)
        {
            var values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            left.Add(long.Parse(values[0]));
            right.Add(long.Parse(values[1]));
        }

        left.Sort();
        right.Sort();
        return new Solution(left, right);
    }

    public async ValueTask<long> GetPart1()
    {
        return left.Zip(right).Sum(x => Math.Abs(x.First - x.Second));
    }

    public async ValueTask<long> GetPart2()
    {
        var rightLookup = right.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        return left.Sum(x => x * rightLookup.GetValueOrDefault(x, 0));
    }
}
