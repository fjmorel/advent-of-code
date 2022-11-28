namespace Puzzles2019.Solutions;

public class Solution19 : ISolution
{
    private readonly long[] _opCodes;

    public Solution19(string[] lines)
    {
        _opCodes = lines[0].ParseCsvLongs();
    }

    public async ValueTask<long> GetPart1()
    {
        var bag = new ConcurrentBag<Point>();
        var points = Enumerable.Range(0, 50).SelectMany(x => Enumerable.Range(0, 50).Select(y => new Point(x, y)));
        await Parallel.ForEachAsync(points, async (pt, _) =>
        {
            if (await IsBeamed(pt))
                bag.Add(pt);
        });

        return bag.Count;
    }

    public async ValueTask<long> GetPart2()
    {
        int x = 0, y = 10;
        while (true)
        {
            // On each row, keep going right until there's a match
            while (!await IsBeamed(new(x, y)))
                x++;
            // starting from bottom left, if bottom-right and top-left, we found it!
            if (await IsBeamed(new(x + 99, y)) && await IsBeamed(new(x, y - 99)) && await IsBeamed(new(x + 99, y - 99)))
                return x * 10_000 + (y - 99);
            y += 1;
        }
    }

    private async Task<bool> IsBeamed(Point pt)
    {
        var output = GetChannel(1);
        var input = GetChannel(2);
        await input.Writer.WriteAsync(pt.x);
        await input.Writer.WriteAsync(pt.y);
        await new IntCodeComputer(_opCodes).Run(input.Reader, output.Writer);

        return await output.Reader.ReadAsync() == 1;
    }

    private Channel<long> GetChannel(int capacity) => Channel.CreateBounded<long>(new BoundedChannelOptions(capacity)
    {
        SingleReader = true,
        SingleWriter = true,
        FullMode = BoundedChannelFullMode.Wait,
        AllowSynchronousContinuations = true,
    });
}
