var timer = Stopwatch.StartNew();
var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");

Console.WriteLine($"_ :: {timer.Elapsed}");// setup time
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	var gamma = string.Join("", list[0].Select((_, i) => GetSurpulusOnes(list, i) >= 0 ? '1' : '0'));
	var epsilon = string.Join("", gamma.Select(x => x == '0' ? '1' : '0'));
	return ConvertNum(gamma) * ConvertNum(epsilon);
}

long Part2()
{
	var o2 = GetRating(list, true);
	var co2 = GetRating(list, false);
	return ConvertNum(o2) * ConvertNum(co2);
}

string GetRating(string[] lines, bool useMost)
{
	var i = 0;
	do
	{
		var oneCount = GetSurpulusOnes(lines, i);
		var digit = ((useMost && oneCount >= 0) || (!useMost && oneCount < 0)) ? '1' : '0';
		lines = lines.Where(x => x[i] == digit).ToArray();
	} while (lines.Length > 1 && ++i < lines[0].Length);
	return lines[0];
}

long ConvertNum(string digits) => Convert.ToInt64(digits, 2);

long GetSurpulusOnes(string[] lines, int index) => lines.Aggregate(0, (sum, x) => x[index] == '1' ? ++sum : --sum);
