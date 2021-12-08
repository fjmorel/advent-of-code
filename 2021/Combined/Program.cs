var day = args[0];

var timer = Stopwatch.StartNew();
var lines = File.ReadAllLines($"inputs/{day}.txt");
// lines = File.ReadAllLines($"examples/{day}.txt");

var job = GetSolution(day);

Console.WriteLine($"SETUP :: {timer.Elapsed}");// setup time
Console.WriteLine($"{await job.GetPart1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{await job.GetPart2()} :: {timer.Elapsed}");
timer.Stop();

ISolution GetSolution(string day)
{
	var types = typeof(ISolution).Assembly.GetTypes();
	var type = typeof(ISolution).Assembly.GetType("Combined.Solutions.Solution" + day, true);
	var solution = Activator.CreateInstance(type!, new object[] { lines });
	return (ISolution)solution!;
}

interface ISolution
{
	Task<long> GetPart1();
	Task<long> GetPart2();
}