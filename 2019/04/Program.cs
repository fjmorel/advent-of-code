var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");
var nums = list[0].Split('-').Select(int.Parse);
var MIN = nums.First();
var MAX = nums.Last();



var timer = Stopwatch.StartNew();
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	// 476878 too high
	return Enumerable.Range(MIN, MAX - MIN).Where(x => IsPossible(x, false)).Count();
}

long Part2()
{
	return Enumerable.Range(MIN, MAX - MIN).Where(x => IsPossible(x, true)).Count();
}

bool IsPossible(int num, bool streakLimit)
{
	// Within range
	if (num < MIN || num > MAX)
		return false;

	var digits = GetDigits(num).ToList();

	// Digits are ascending
	if (!digits.OrderByDescending(x => x).SequenceEqual(digits))
		return false;

	// Has repeated digit (with optional limit)
	var streaks = new List<int>();
	var streak = 1;
	for (var i = 1; i < digits.Count; i++)
	{
		if (digits[i] == digits[i - 1])
		{
			streak++;
		}
		else
		{
			streaks.Add(streak);
			streak = 1;
		}
	}
	streaks.Add(streak);

	return streaks.Any(x => streakLimit ? x == 2 : x >= 2);
}

IEnumerable<int> GetDigits(int num)
{
	while (num > 0)
	{
		var digit = num % 10;
		yield return digit;
		num = (num - digit) / 10;
	}
}