var timer = Stopwatch.StartNew();
var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");

var pattern = new int[] { 0, 1, 0, -1 };
var patternLength = pattern.Length;

Console.WriteLine($"_ :: {timer.Elapsed}");// setup time
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

string Part1()
{
	var offset = 0;
	var digits = ParseLine(list[0]);
	for (var phase = 1; phase <= 100; phase++)
	{
		// too slow for part 2
		for (var i = offset + 1; i <= digits.Length; i++)
		{
			var sum = 0;
			for (var j = i - 1; j < digits.Length; j++)
			{
				switch (pattern[((j + 1) / i) % patternLength])
				{
					case 1:
						sum += digits[j];
						break;
					case -1:
						sum -= digits[j];
						break;
					default:
						j += i - 1;
						break;
				}
			}
			digits[i - 1] = Math.Abs(sum % 10);
		}
	}
	return GetMessage(digits, offset);
}

string Part2()
{
	const int repeated = 10_000;//10_000;
	var baseDigits = ParseLine(list[0]);
	var originalLength = baseDigits.Length;
	var digits = new int[originalLength * repeated];
	// 650 digits, repeated 10_000 times = 6_500_000 digits

	for (var i = 0; i < originalLength; i++)
	{
		for (var j = 0; j < repeated; j++)
		{
			digits[i + (j * originalLength)] = baseDigits[i];
		}
	}

	// 5_970_807. Far into the last half, so we can rely on the pattern being just 1 for digits starting at that point
	var offset = int.Parse(string.Join("", baseDigits.Take(7).Select(x => x.ToString())));

	for (var phase = 1; phase <= 100; phase++)
	{
		// skip last because it never changes
		// only works for offsets in 2nd half of number (part 2)
		for (var i = digits.Length - 2; i >= offset; i--)
		{
			digits[i] = (digits[i] + digits[i + 1]) % 10;
		}
	}
	return GetMessage(digits, offset);
}

int[] ParseLine(string line) => line.Select(x => int.Parse(new string(x, 1))).ToArray();

string GetMessage(int[] final, int offset)
{
	return string.Join("", final.Skip(offset).Take(8).Select(x => x.ToString()));
}
