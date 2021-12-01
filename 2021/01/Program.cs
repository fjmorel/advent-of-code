var timer = Stopwatch.StartNew();
var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");
var nums = list.Select(x => int.Parse(x)).ToList();

Console.WriteLine($"_ :: {timer.Elapsed}");// setup time
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	return nums.Skip(1).Select((x, i) => x > nums[i]).Count(x => x);
}

long Part2()
{
	return nums.Skip(3).Select((x, i) => x > nums[i]).Count(x => x);
}
