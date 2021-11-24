var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");
var opCodes = list[0].Split(',').Select(x => int.Parse(x)).ToList();

var timer = Stopwatch.StartNew();
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	return Compute(opCodes, 1);
}

long Part2()
{
	return Compute(opCodes, 5);
}

int Compute(List<int> original, int input)
{
	var copy = new int[original.Count];
	original.CopyTo(copy);

	var outputs = new List<int>();
	int i = 0;
	while (true)
	{
		var instruction = copy[i];
		var modes = GetDigits(instruction / 100, 3).ToArray();
		var opcode = instruction % 100;

		// Run the instruction at the current pointer and find out how many parameters it had
		var parameterCount = opcode switch
		{
			1 => Add(copy, i, modes),
			2 => Multiply(copy, i, modes),
			3 => Input(copy, i, modes, input),
			4 => Output(copy, i, modes, outputs),
			5 => JumpIfTrue(copy, ref i, modes),
			6 => JumpIfFalse(copy, ref i, modes),
			7 => LessThan(copy, i, modes),
			8 => Equals(copy, i, modes),
			99 => -1,
			_ => throw new NotSupportedException("Unexpected Opcode: " + copy[i]),
		};
		// -1 is special value to indicate we should halt
		if (parameterCount < 0)
		{
			if (!outputs.SkipLast(1).All(x => x == 0))
				throw new InvalidOperationException("Some outputs were not 0!");
			return outputs.Last();
		}

		// Go to next instruction (instruction parameter count + 1 for Opcode)
		// If > 0 so that jump instructions can override this
		if (parameterCount != 0)
			i += 1 + parameterCount;
	}

}

IEnumerable<int> GetDigits(int num, int minLength)
{
	var returned = 0;
	while (num > 0)
	{
		var digit = num % 10;
		yield return digit;
		returned++;
		num = (num - digit) / 10;
	}
	while (returned < minLength)
	{
		yield return 0;
		returned++;
	}
}

/// <summary>
/// Get the values indicated by the pointers, and set sum in position in pointer.
/// </summary>
static int Add(int[] codes, int i, int[] modes)
{
	int iA = codes.GetIndex(i + 1, modes[0]), iB = codes.GetIndex(i + 2, modes[1]), iOut = codes.GetIndex(i + 3, 0);
	codes[iOut] = codes[iA] + codes[iB];
	return 3;
}

/// <summary>
/// Get the values indicated by the pointers, and set product in position in pointer.
/// </summary>
static int Multiply(int[] codes, int i, int[] modes)
{
	int iA = codes.GetIndex(i + 1, modes[0]), iB = codes.GetIndex(i + 2, modes[1]), iOut = codes.GetIndex(i + 3, 0);
	codes[iOut] = codes[iA] * codes[iB];
	return 3;
}

/// <summary>
/// Takes a single integer as input and saves it to the position given by its only parameter.
/// </summary>
static int Input(int[] codes, int i, int[] modes, int input)
{
	codes[codes.GetIndex(i + 1, 0)] = input;
	return 1;
}

/// <summary>
/// Outputs the value of its only parameter.
/// </summary>
static int Output(int[] codes, int i, int[] modes, List<int> outputs)
{
	outputs.Add(codes[codes.GetIndex(i + 1, modes[0])]);
	return 1;
}

/// <summary>
/// If the first parameter is non-zero,
/// it sets the instruction pointer to the value from the second parameter.
/// Otherwise, it does nothing.
/// </summary>
static int JumpIfTrue(int[] codes, ref int i, int[] modes)
{
	if (codes[codes.GetIndex(i + 1, modes[0])] != 0)
	{
		i = codes[codes.GetIndex(i + 2, modes[1])];
		return 0;// 0 to skip incrementing and let this ref change take precedence
	}
	else
	{
		return 2;
	}
}

/// <summary>
/// If the first parameter is zero,
/// it sets the instruction pointer to the value from the second parameter.
/// Otherwise, it does nothing.
/// </summary>
static int JumpIfFalse(int[] codes, ref int i, int[] modes)
{
	if (codes[codes.GetIndex(i + 1, modes[0])] == 0)
	{
		i = codes[codes.GetIndex(i + 2, modes[1])];
		return 0;// 0 to skip incrementing and let this ref change take precedence
	}
	else
	{
		return 2;
	}
}

/// <summary>
/// If the first parameter is less than the second parameter,
/// it stores 1 in the position given by the third parameter.
/// Otherwise, it stores 0.
/// </summary>
static int LessThan(int[] codes, int i, int[] modes)
{
	int iA = codes.GetIndex(i + 1, modes[0]), iB = codes.GetIndex(i + 2, modes[1]), iOut = codes.GetIndex(i + 3, 0);
	codes[iOut] = codes[iA] < codes[iB] ? 1 : 0;
	return 3;
}

/// <summary>
/// If the first parameter is equal to the second parameter,
/// it stores 1 in the position given by the third parameter.
/// Otherwise, it stores 0.
/// </summary>
static int Equals(int[] codes, int i, int[] modes)
{
	int iA = codes.GetIndex(i + 1, modes[0]), iB = codes.GetIndex(i + 2, modes[1]), iOut = codes.GetIndex(i + 3, 0);
	codes[iOut] = codes[iA] == codes[iB] ? 1 : 0;
	return 3;
}

public static class Extensions
{
	public static int GetIndex(this int[] codes, int index, int mode)
	{
		return mode switch
		{
			0 => codes[index],// position mode
			1 => index,// immediate mode
			_ => throw new NotSupportedException("Unexpected mode: " + mode),
		};
	}
}