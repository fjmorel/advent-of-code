namespace Puzzles2022.Day10;

public record Solution(Computer cpu) : ISolution<Solution>
{
    private const int WIDTH = 40;
    private const int HEIGHT = 6;

    public static Solution Init(string[] lines) => new(new Computer(lines));

    public async ValueTask<long> GetPart1()
    {
        var channel = Computer.CreateScreenChannel();
        var cpuTask = cpu.Run(null, channel.Writer);

        long sum = 0, cycle = 0;
        await foreach (var value in channel.Reader.ReadAllAsync())
        {
            cycle++;
            if ((cycle - 20) % 40 == 0)
                sum += value.X * cycle;

        }

        await cpuTask;
        return sum;
    }

    public async ValueTask<string> GetPart2String()
    {
        var channel = Computer.CreateScreenChannel();
        var cpuTask = cpu.Run(null, channel.Writer);
        var screens = await cpu.PrintScreen(channel.Reader).ToListAsync();
        await cpuTask;
        return screens[0];
    }
}
