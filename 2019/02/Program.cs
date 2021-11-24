var list = File.ReadAllLines("input.txt");
// var list = File.ReadAllLines("example.txt");
var opCodes = list[0].Split(',').Select(x => int.Parse(x)).ToList();

var timer = Stopwatch.StartNew();
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	var copy = new int[opCodes.Count];
	opCodes.CopyTo(copy);
	copy[1] = 12;
	copy[2] = 2;
	return Compute(copy);
}

long Part2()
{
	for (var i = 0; i < 100; i++)
	{
		for (var j = 0; j < 100; j++)
		{
			var copy = new int[opCodes.Count];
			opCodes.CopyTo(copy);
			copy[1] = i;
			copy[2] = j;
			var result = Compute(copy);
			if (result == 19690720)
				return 100 * i + j;
		}
	}
	return -1;
}

int Compute(int[] codes)
{
	var i = 0;
	while (true)
	{
		switch (codes[i])
		{
			case 1:
				i += Add(codes, i);
				break;
			case 2:
				i += Multiply(codes, i);
				break;
			case 99:
				return codes[0];
			default:
				throw new ArgumentException("Unexpected op: " + codes[i]);
		}
	}
	throw new ArgumentException("Never hit 99");
}

int Add(int[] codes, int index)
{
	codes[codes[index + 3]] = codes[codes[index + 1]] + codes[codes[index + 2]];
	return 4;
}

int Multiply(int[] codes, int index)
{
	codes[codes[index + 3]] = codes[codes[index + 1]] * codes[codes[index + 2]];
	return 4;
}