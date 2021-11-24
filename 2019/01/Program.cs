var list = System.IO.File.ReadAllLines("input.txt");
// var list = System.IO.File.ReadAllLines("example.txt");
var ints = list.Select(x => long.Parse(x)).ToList();

var timer = Stopwatch.StartNew();
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	return ints.Select(GetFuel).Sum();
}

long Part2()
{
	return ints.Select(GetFuel).Select(GetFuelForFuel).Sum();
}

long GetFuel(long mass) => mass / 3 - 2;

long GetFuelForFuel(long original)
{
	var addedFuel = GetFuel(original);
	var totalFuel = original;
	while (addedFuel > 0)
	{
		totalFuel += addedFuel;
		addedFuel = GetFuel(addedFuel);
	}
	return totalFuel;
}