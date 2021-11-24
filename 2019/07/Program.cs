var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");
long[] opCodes = list[0].Split(',').Select(x => long.Parse(x)).ToArray();

var timer = Stopwatch.StartNew();
Console.WriteLine($"{await Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{await Part2()} :: {timer.Elapsed}");
timer.Stop();

async Task<long> Part1()
{
	long max = 0;
	foreach (var settings in GetSettings(0, 4))
	{
		var output = await GetSelfThrust(settings);
		max = Math.Max(max, output);
	}
	return max;
}

async Task<long> Part2()
{
	long max = 0;
	foreach (var settings in GetSettings(5, 9))
	{
		var output = await GetFeedbackThrust(settings);
		max = Math.Max(max, output);
	}
	return max;
}

IEnumerable<int[]> GetSettings(int start, int end)
{
	for (var i = start; i <= end; i++)
	{
		for (var j = start; j <= end; j++)
		{
			if (i == j)
				continue;

			for (var k = start; k <= end; k++)
			{
				if (i == k || j == k)
					continue;
				for (var l = start; l <= end; l++)
				{
					if (i == l || j == l || k == l)
						continue;
					for (var m = start; m <= end; m++)
					{
						if (i == m || j == m || k == m || l == m)
							continue;
						yield return new[] { i, j, k, l, m };
					}
				}
			}
		}
	}
}

async Task<long> GetSelfThrust(int[] settings)
{
	long output = 0;
	foreach (var setting in settings)
	{
		var channel = Channel.CreateUnbounded<long>();
		var writer = channel.Writer;
		await writer.WriteAsync(setting);
		await writer.WriteAsync(output);
		var reader = channel.Reader;
		await Compute(opCodes, channel.Reader, channel.Writer);
		output = await reader.ReadAsync();
	}
	return output;
}

async Task<long> GetFeedbackThrust(int[] settings)
{
	var count = settings.Length;

	// Set up initial data
	var channels = Enumerable.Range(1, count).Select(x => Channel.CreateUnbounded<long>()).ToList();
	channels.Add(channels[0]);// for easier logic in for loop
	for (var i = 0; i < count; i++)
	{
		var writer = channels[i].Writer;
		await writer.WriteAsync(settings[i]);
		if (i == 0)
			await writer.WriteAsync(0);

	}
	// read from current channel, but write into next computer's channel
	var tasks = new Task[count];
	for (var i = 0; i < count; i++)
	{
		tasks[i] = Compute(opCodes, channels[i].Reader, channels[i + 1].Writer);
	}
	await Task.WhenAll(tasks);
	return await channels[0].Reader.ReadAsync();// last output from last amplifier went back to first channel
}

async Task Compute(long[] original, ChannelReader<long> reader, ChannelWriter<long> writer)
{
	long[] copy = new long[original.Length];
	original.CopyTo(copy, 0);

	long i = 0;
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
			3 => await Input(copy, i, modes, reader),
			4 => await Output(copy, i, modes, writer),
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
			writer.TryComplete();
			return;
		}

		// Go to next instruction (instruction parameter count + 1 for Opcode)
		// If > 0 so that jump instructions can override this
		if (parameterCount != 0)
			i += 1 + parameterCount;
	}

}

IEnumerable<int> GetDigits(long num, int minLength)
{
	var returned = 0;
	while (num > 0)
	{
		var digit = num % 10;
		yield return (int)digit;
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
static int Add(long[] codes, long i, int[] modes)
{
	long iA = codes.GetIndex(i + 1, modes[0]), iB = codes.GetIndex(i + 2, modes[1]), iOut = codes.GetIndex(i + 3, 0);
	codes[iOut] = codes[iA] + codes[iB];
	return 3;
}

/// <summary>
/// Get the values indicated by the pointers, and set product in position in pointer.
/// </summary>
static int Multiply(long[] codes, long i, int[] modes)
{
	long iA = codes.GetIndex(i + 1, modes[0]), iB = codes.GetIndex(i + 2, modes[1]), iOut = codes.GetIndex(i + 3, 0);
	codes[iOut] = codes[iA] * codes[iB];
	return 3;
}

/// <summary>
/// Takes a single integer as input and saves it to the position given by its only parameter.
/// </summary>
static async Task<int> Input(long[] codes, long i, int[] modes, ChannelReader<long> data)
{
	codes[codes.GetIndex(i + 1, 0)] = await data.ReadAsync();
	return 1;
}

/// <summary>
/// Outputs the value of its only parameter.
/// </summary>
static async Task<int> Output(long[] codes, long i, int[] modes, ChannelWriter<long> data)
{
	await data.WriteAsync(codes[codes.GetIndex(i + 1, modes[0])]);
	return 1;
}

/// <summary>
/// If the first parameter is non-zero,
/// it sets the instruction pointer to the value from the second parameter.
/// Otherwise, it does nothing.
/// </summary>
static int JumpIfTrue(long[] codes, ref long i, int[] modes)
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
static int JumpIfFalse(long[] codes, ref long i, int[] modes)
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
static int LessThan(long[] codes, long i, int[] modes)
{
	long iA = codes.GetIndex(i + 1, modes[0]), iB = codes.GetIndex(i + 2, modes[1]), iOut = codes.GetIndex(i + 3, 0);
	codes[iOut] = codes[iA] < codes[iB] ? 1 : 0;
	return 3;
}

/// <summary>
/// If the first parameter is equal to the second parameter,
/// it stores 1 in the position given by the third parameter.
/// Otherwise, it stores 0.
/// </summary>
static int Equals(long[] codes, long i, int[] modes)
{
	long iA = codes.GetIndex(i + 1, modes[0]), iB = codes.GetIndex(i + 2, modes[1]), iOut = codes.GetIndex(i + 3, 0);
	codes[iOut] = codes[iA] == codes[iB] ? 1 : 0;
	return 3;
}

public static class Extensions
{
	public static long GetIndex(this long[] codes, long index, int mode)
	{
		return mode switch
		{
			0 => codes[index],// position mode
			1 => index,// immediate mode
			_ => throw new NotSupportedException("Unexpected mode: " + mode),
		};
	}
}