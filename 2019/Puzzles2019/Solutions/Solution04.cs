namespace Puzzles2019.Solutions;

public record Solution04(int MIN, int MAX) : ISolution<Solution04>
{
    public static Solution04 Init(string[] lines)
    {
        var nums = lines[0].Split('-').Select(int.Parse).ToList();
        var min = nums.First();
        var max = nums.Last();
        return new(min, max);
    }

    public async ValueTask<long> GetPart1() => Enumerable.Range(MIN, MAX - MIN).Count(x => IsPossible(x, false));

    public async ValueTask<long> GetPart2() => Enumerable.Range(MIN, MAX - MIN).Count(x => IsPossible(x, true));


    private bool IsPossible(int num, bool streakLimit)
    {
        // Within range
        if (num < MIN || num > MAX)
            return false;

        var digits = num.GetBase10Digits();

        // Digits are ascending
        if (!digits.OrderByDescending(x => x).SequenceEqual(digits))
            return false;

        // Has repeated digit (with optional limit)
        var streaks = new List<int>();
        var streak = 1;
        for (var i = 1; i < digits.Count; i++)
        {
            if (digits[i] == digits[i - 1])
            {
                streak++;
            }
            else
            {
                streaks.Add(streak);
                streak = 1;
            }
        }

        streaks.Add(streak);

        return streaks.Any(x => streakLimit ? x == 2 : x >= 2);
    }
}
