namespace Puzzles2019.Solutions;

public class Solution04 : ISolution
{
    private readonly int MIN;
    private readonly int MAX;

    public Solution04(string[] lines)
    {
        var nums = lines[0].Split('-').Select(int.Parse).ToList();
        MIN = nums.First();
        MAX = nums.Last();
    }

    public async ValueTask<long> GetPart1() => Enumerable.Range(MIN, MAX - MIN).Count(x => IsPossible(x, false));

    public async ValueTask<long> GetPart2() => Enumerable.Range(MIN, MAX - MIN).Count(x => IsPossible(x, true));


    bool IsPossible(int num, bool streakLimit)
    {
        // Within range
        if (num < MIN || num > MAX)
            return false;

        var digits = GetDigits(num).ToList();

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

    IEnumerable<int> GetDigits(int num)
    {
        while (num > 0)
        {
            var digit = num % 10;
            yield return digit;
            num = (num - digit) / 10;
        }
    }
}
