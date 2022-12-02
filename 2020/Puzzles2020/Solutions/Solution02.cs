namespace Puzzles2020.Solutions;

public record Solution02(long oldValid, long reallyValid) : ISolution<Solution02>
{
    public static Solution02 Init(string[] lines)
    {
        var oldValid = 0;
        var reallyValid = 0;
        foreach (var line in lines)
        {
            // todo: convert to Regex to parse object
            var pieces = line.Split(' ');
            var nums = pieces[0].Split('-');
            var a = int.Parse(nums[0]);
            var b = int.Parse(nums[1]);
            var letter = pieces[1][0];
            var password = pieces[2];

            var count = password.Count(x => x == letter);
            if (count <= b && count >= a)
                oldValid++;

            var first = password[a - 1];
            var second = password[b - 1];
            if (first != second && (first == letter || second == letter))
                reallyValid++;
        }

        return new(oldValid, reallyValid);
    }

    public async ValueTask<long> GetPart1() => oldValid;

    public async ValueTask<long> GetPart2() => reallyValid;
}
