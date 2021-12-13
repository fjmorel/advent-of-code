namespace Puzzles2020.Solutions;

public class Solution09 : ISolution
{
    private readonly List<long> _nums;
    private long invalid = 0;

    public Solution09(string[] lines)
    {
        _nums = lines.ParseLongs();

    }

    public async ValueTask<long> GetPart1()
    {
        const int preamble = 25;
        Parallel.ForEach(_nums.Skip(preamble), (item, state, i) =>
        {
            var range = _nums.Skip((int)i).Take(preamble).ToList();

            if (!range.SelectMany(x => range.Where(y => y != x).Select(y => x + y)).Contains(item))
            {
                invalid = item;
                state.Stop();
            }
        });
        return invalid;
    }

    public async ValueTask<long> GetPart2()
    {
        var solution = -1L;
        Parallel.ForEach(_nums, (item, state, i) =>
        {
            if (item == invalid)
                return;
            long sum = 0;
            int j = 0;
            while (sum < invalid && (i + j) < _nums.Count)
            {
                sum += _nums[(int)i + j];
                j++;
            }
            if (sum == invalid)
            {
                var range = _nums.Skip((int)i).Take(j).ToList();
                solution = range.Min() + range.Max();
                state.Stop();
            }
        });
        return solution;
    }

}
