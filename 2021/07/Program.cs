var timer = Stopwatch.StartNew();
var lines = File.ReadAllLines("input.txt");
// lines = File.ReadAllLines("example.txt");

var nums = lines[0].Split(',').Select(int.Parse).ToList();
var min = nums.Min();
var max = nums.Max();

Console.WriteLine($"SETUP :: {timer.Elapsed}");
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1() => Enumerable.Range(min, max - min + 1).Min(i => nums.Select(x => Math.Abs(x - i)).Sum());

long Part2() => Enumerable.Range(min, max - min + 1).Min(i => nums.Select(x => Math.Abs(x - i)).Select(x => x * (x + 1) / 2).Sum());
