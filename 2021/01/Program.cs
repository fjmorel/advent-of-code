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
	return nums.Skip(1).Select((x, i) => x > nums[i]).Where(x => x).Count();
}

long Part2()
{
	var summed = nums.Skip(2).Select((x, i) => x + nums[i] + nums[i + 1]).ToList();
	return summed.Skip(1).Select((x, i) => x > summed[i]).Where(x => x).Count();
}
