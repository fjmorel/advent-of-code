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
	return Compute(opCodes, 12, 2);
}

long Part2()
{
	for (var i = 0; i < 100; i++)
	{
		for (var j = 0; j < 100; j++)
		{
			var result = Compute(opCodes, i, j);
			if (result == 19690720)
				return 100 * i + j;
		}
	}
	return -1;
}

int Compute(List<int> original, int noun, int verb)
{
	var copy = new int[original.Count];
	original.CopyTo(copy);
	copy[1] = noun;
	copy[2] = verb;


	var i = 0;
	while (true)
	{
		// Run the instruction at the current pointer and find out how many parameters it had
		var parameterCount = copy[i] switch
		{
			1 => Add(copy, i),
			2 => Multiply(copy, i),
			99 => -1,
			_ => throw new NotSupportedException("Unexpected Opcode: " + copy[i]),
		};
		// -1 is special value to indicate we should halt
		if (parameterCount < 0)
			return copy[0];

		// Go to next instruction (instruction parameter count + 1 for Opcode)
		i += 1 + parameterCount;
	}
}

/// <summary>
/// Get the values indicated by the pointers, and set sum in position in pointer
/// </summary>
int Add(int[] codes, int index)
{
	codes[codes[index + 3]] = codes[codes[index + 1]] + codes[codes[index + 2]];
	return 3;
}

/// <summary>
/// Get the values indicated by the pointers, and set product in position in pointer
/// </summary>
int Multiply(int[] codes, int index)
{
	codes[codes[index + 3]] = codes[codes[index + 1]] * codes[codes[index + 2]];
	return 3;
}