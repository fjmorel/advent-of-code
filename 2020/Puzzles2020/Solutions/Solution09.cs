namespace Puzzles2020.Solutions;

public record Solution09(List<long> _nums) : ISolution<Solution09>
{
    private long invalid;

    public static Solution09 Init(string[] lines) => new(lines.ParseLongs());

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
