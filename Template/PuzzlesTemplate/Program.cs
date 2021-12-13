using Spectre.Console;
using Spectre.Console.Rendering;

if (!args.Any())
    args = new string[] { Console.ReadLine()! };

foreach (var arg in args)
{
    if (string.IsNullOrWhiteSpace(arg))
        continue;
    if (arg == "all")
        foreach (var day in Enumerable.Range(1, 25))
            await TryRunDay(day.ToString("00"));
    else
        await TryRunDay(arg!);
}

async ValueTask<TimeSpan[]> TryRunDay(string day)
{
    var assembly = typeof(Program).Assembly;
    var timer = Stopwatch.StartNew();
    var folder = "inputs";
    // folder = "examples";
    if (!RunnerUtilities.TryGetData(assembly, folder, day, out var lines))
    {
        AnsiConsole.WriteLine($"Could not get data for day {day}");
        return Array.Empty<TimeSpan>();
    }

    if (!RunnerUtilities.TryGetSolution(assembly, day, lines, out var job))
    {
        AnsiConsole.WriteLine($"Could not get solution class for day {day}");
        return Array.Empty<TimeSpan>();
    }
    var setupTime = timer.Elapsed;

    var table = new Table();
    var times = await AnsiConsole.Live(table).StartAsync(async ctx =>
    {
        table.Title($"Day {day}", new Style(Color.Green, decoration: Decoration.Bold));
        table.AddColumn("Step");
        table.AddColumn("Value", col => { col.Alignment = Justify.Right; });
        table.AddColumn("Time", col => { col.Alignment = Justify.Right; });
        table.BorderStyle = new Style(Color.Red);

        table.AddRow(GetRow("Setup", "", setupTime));
        ctx.Refresh();
        timer.Restart();
        var part1 = await job.GetPart1();
        var part1Time = timer.Elapsed;
        table.AddRow(GetRow("Part 1", part1.ToString(), part1Time));
        ctx.Refresh();
        timer.Restart();
        var part2 = await job.GetPart2();
        var part2Time = timer.Elapsed;
        table.AddRow(GetRow("Part 2", part2.ToString(), part2Time));
        ctx.Refresh();
        timer.Stop();

        return new[] { setupTime, part1Time, part2Time };
    });
    return times;
}

IRenderable[] GetRow(string step, string value, TimeSpan elapsed)
{
    var ms = elapsed.TotalMilliseconds;
    var digits = ms switch
    {
        >= 1_000 => 0,
        >= 100 => 1,
        >= 10 => 2,
        _ => 3,
    };
    var text = ms.ToString("F" + digits);
    var color = ms switch
    {
        < 1 => Color.Cyan2,
        < 10 => Color.SpringGreen1,
        < 20 => Color.Green1,
        < 50 => Color.GreenYellow,
        < 100 => Color.Orange1,
        < 1_000 => Color.DarkOrange,
        _ => Color.OrangeRed1,
    };
    return new IRenderable[]
    {
        new Text(step, new Style(decoration: Decoration.Bold)),
        new Text(value),
        new Text($"{text} ms", new Style(color)),
    };
}
