using Combined;

var arg = args.FirstOrDefault();
if (string.IsNullOrWhiteSpace(arg))
    arg = Console.ReadLine();

if (arg == "all")
    await RunAllDays();
else
    await TryRunDay(arg!);

async Task RunAllDays()
{
    for (var i = 1; i <= 25; i++)
    {
        var day = i.ToString("00");
        if (!await TryRunDay(day))
            Console.WriteLine($"Failed to run Day {day}.");
    }
}

async Task<bool> TryRunDay(string day)
{
    var timer = Stopwatch.StartNew();

    var folder = "inputs";
    // folder = "examples";

    if (!Utilities.TryGetData(folder, day, out var lines))
        return false;

    if (!Utilities.TryGetSolution(day, lines, out var job))
        return false;

    Console.WriteLine($"Day {day}");
    Console.WriteLine($"SETUP :: {timer.Elapsed}");// setup time
    Console.WriteLine($"{await job.GetPart1()} :: {timer.Elapsed}");
    timer.Restart();
    Console.WriteLine($"{await job.GetPart2()} :: {timer.Elapsed}");
    timer.Stop();
    Console.WriteLine();
    return true;
}
