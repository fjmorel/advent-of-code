namespace Puzzles2019.Solutions;

public class Solution07 : ISolution
{
    private readonly long[] _opCodes;

    public Solution07(string[] lines)
    {
        _opCodes = lines[0].ParseCsvLongs();
    }

    public async ValueTask<long> GetPart1()
    {
        long max = 0;
        foreach (var settings in GetSettings(0, 4))
        {
            var output = await GetSelfThrust(settings);
            max = Math.Max(max, output);
        }

        return max;
    }

    public async ValueTask<long> GetPart2()
    {
        long max = 0;
        foreach (var settings in GetSettings(5, 9))
        {
            var output = await GetFeedbackThrust(settings);
            max = Math.Max(max, output);
        }

        return max;
    }

    IEnumerable<int[]> GetSettings(int start, int end)
    {
        for (var i = start; i <= end; i++)
        {
            for (var j = start; j <= end; j++)
            {
                if (i == j)
                    continue;

                for (var k = start; k <= end; k++)
                {
                    if (i == k || j == k)
                        continue;
                    for (var l = start; l <= end; l++)
                    {
                        if (i == l || j == l || k == l)
                            continue;
                        for (var m = start; m <= end; m++)
                        {
                            if (i == m || j == m || k == m || l == m)
                                continue;
                            yield return new[] { i, j, k, l, m };
                        }
                    }
                }
            }
        }
    }

    async Task<long> GetSelfThrust(int[] settings)
    {
        var computer = new Computer(_opCodes);
        long output = 0;
        foreach (var setting in settings)
        {
            var channel = Channel.CreateUnbounded<long>();
            var writer = channel.Writer;
            await writer.WriteAsync(setting);
            await writer.WriteAsync(output);
            var reader = channel.Reader;
            await computer.Run(channel.Reader, channel.Writer);
            output = await reader.ReadAsync();
        }

        return output;
    }

    async Task<long> GetFeedbackThrust(int[] settings)
    {
        var count = settings.Length;

        // Set up initial data
        var channels = Enumerable.Range(1, count).Select(x => Channel.CreateUnbounded<long>()).ToList();
        channels.Add(channels[0]);// for easier logic in for loop
        for (var i = 0; i < count; i++)
        {
            var writer = channels[i].Writer;
            await writer.WriteAsync(settings[i]);
            if (i == 0)
                await writer.WriteAsync(0);
        }

        var computer = new Computer(_opCodes);

        // read from current channel, but write into next computer's channel
        var tasks = new Task[count];
        for (var i = 0; i < count; i++)
        {
            tasks[i] = computer.Run(channels[i].Reader, channels[i + 1].Writer);
        }

        await Task.WhenAll(tasks);
        return await channels[0].Reader.ReadAsync();// last output from last amplifier went back to first channel
    }
}
