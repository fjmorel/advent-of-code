using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Shared;

public record Runner(Assembly assembly)
{
    public async ValueTask Run(string[] args, string folder)
    {
        var days = GetDays(args).ToList();

        // Set up rendering table
        var table = new Table();
        table.Title("Days", new Style(Color.Green, decoration: Decoration.Bold));
        table.AddColumn("Step");
        table.AddColumn("Value", col => { col.Alignment = Justify.Right; });
        table.AddColumn("Time", col => { col.Alignment = Justify.Right; });
        table.BorderStyle = new Style(Color.Red, decoration: Decoration.Bold);

        await AnsiConsole.Live(table).StartAsync(async ctx =>
        {
            foreach (var day in days)
                await TryRunDay(folder, day, ctx, table);

            table.AddEmptyRow();
        });
    }

    /// <summary>
    /// Parse which days to run from launch arguments (or console input if there were none)
    /// </summary>
    private static IEnumerable<string> GetDays(string[] args)
    {
        if (!args.Any())
            args = new string[] { AnsiConsole.Prompt(new TextPrompt<string>("Days to run:")) };

        var inputs = args.SelectMany(x => x.Split(',')).ToList();
        foreach (var input in inputs)
        {
            if (input == "all")
                foreach (var day in Enumerable.Range(1, 25))
                    yield return day.ToString("00");
            else
                yield return input;
        }
    }

    private async ValueTask<TimeSpan[]> TryRunDay(string folder, string day, LiveDisplayContext ctx, Table table)
    {
        var timer = Stopwatch.StartNew();
        if (!TryGetData(folder, day, out var lines))
        {
            AnsiConsole.WriteLine($"Could not get data for day {day}");
            return Array.Empty<TimeSpan>();
        }

        if (!TryGetSolution(day, lines, out var job))
        {
            AnsiConsole.WriteLine($"Could not get solution class for day {day}");
            return Array.Empty<TimeSpan>();
        }

        var setupTime = timer.Elapsed;

        table.AddEmptyRow();
        table.AddRow(new StepInfo($"Day {day} Setup", "", setupTime).GetTableCells());
        ctx.Refresh();
        timer.Restart();
        var part1 = await job.GetPart1String();
        var part1Time = timer.Elapsed;
        table.AddRow(new StepInfo("Part 1", part1, part1Time).GetTableCells());
        ctx.Refresh();
        timer.Restart();
        var part2 = await job.GetPart2String();
        var part2Time = timer.Elapsed;
        table.AddRow(new StepInfo("Part 2", part2, part2Time).GetTableCells());
        ctx.Refresh();
        timer.Stop();

        return new[] { setupTime, part1Time, part2Time };
    }

    public bool TryGetData(string folder, string day, [NotNullWhen(true)] out string[]? lines)
    {
        var inputPath = Path.Combine(Path.GetDirectoryName(assembly.Location)!, folder, $"{day}.txt");
        if (!File.Exists(inputPath))
            inputPath = Path.Combine(Path.GetDirectoryName(assembly.Location)!, $"Day{day}", $"{folder}.txt");

        if (!File.Exists(inputPath))
        {
            lines = null;
            return false;
        }

        lines = File.ReadAllLines(inputPath);
        return true;
    }

    public bool TryGetSolution(string day, string[] input, [NotNullWhen(true)] out ISolution? solution)
    {
        var dayName = "Day" + day;
        var solutionName = "Solution" + day;
        var type = assembly.GetTypes().FirstOrDefault(type => type.Name == solutionName || (type.Namespace?.Contains(dayName) ?? false));

        if (type == null)
        {
            solution = null;
            return false;
        }

        var initializer = type.GetMethod(nameof(ISolution<FakeSolution>.Init), BindingFlags.Static | BindingFlags.Public);
        var obj = initializer!.Invoke(null, new object?[] { input });
        if (obj is ISolution job)
        {
            solution = job;
            return true;
        }

        solution = null;
        return false;
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private record FakeSolution : ISolution<FakeSolution>
    {
        public static FakeSolution Init(string[] lines) => throw new UnreachableException();
    }
}
