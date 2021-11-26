// CANNOT RUN IN 'internalConsole' - USE 'integratedTerminal'
var timer = Stopwatch.StartNew();
var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");
List<long> opCodes = list[0].Split(',').Select(x => long.Parse(x)).ToList();


Console.WriteLine($"_ :: {timer.Elapsed}");// setup time
Console.WriteLine($"{await Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{await Part2()} :: {timer.Elapsed}");
timer.Stop();

async Task<long> Part1()
{
	var screenChannel = Channel.CreateBounded<long>(new BoundedChannelOptions(3)
	{
		FullMode = BoundedChannelFullMode.Wait,
		SingleReader = true,
		SingleWriter = true,
	});
	var inputChannel = Channel.CreateBounded<long>(new BoundedChannelOptions(1)
	{
		FullMode = BoundedChannelFullMode.DropOldest,
		SingleReader = true,
		SingleWriter = true,
	});
	var screen = new Dictionary<Point, TileState>();
	var compute = Compute(opCodes, inputChannel.Reader, screenChannel.Writer);
	var paint = Paint(screen, screenChannel.Reader, inputChannel.Writer);

	await Task.WhenAll(compute, paint);

	return screen.Values.Count(x => x == TileState.Block);
}

async Task<long> Part2()
{
	opCodes[0] = 2;// "play for free"
	var screenChannel = Channel.CreateBounded<long>(new BoundedChannelOptions(3)
	{
		FullMode = BoundedChannelFullMode.Wait,
		SingleReader = true,
		SingleWriter = true,
	});
	var inputChannel = Channel.CreateBounded<long>(new BoundedChannelOptions(1)
	{
		FullMode = BoundedChannelFullMode.DropOldest,
		SingleReader = true,
		SingleWriter = true,
	});

	var screen = new Dictionary<Point, TileState>();
	var compute = Compute(opCodes, inputChannel.Reader, screenChannel.Writer);
	var paint = Paint(screen, screenChannel.Reader, inputChannel.Writer);

	await Task.WhenAll(compute, paint);
	inputChannel.Writer.TryComplete();

	return await paint;
}

async Task<long> Paint(Dictionary<Point, TileState> screen, ChannelReader<long> output, ChannelWriter<long> input)
{
	bool enableOutput = true;
	try
	{
		Console.Clear();
	}
	catch (Exception)
	{
		enableOutput = false;
		Console.WriteLine("Console manipulation has been disabled due to redirection");
	}
	long score = 0, paddleX = 0, paddleY = 0;
	await input.WriteAsync(0);
	while (await output.WaitToReadAsync())
	{
		var x = await output.ReadAsync();
		var y = await output.ReadAsync();
		var state = await output.ReadAsync();

		if (x == -1 && y == 0)
		{
			score = state;
			if (enableOutput)
			{
				Console.SetCursorPosition(0, 0);
				Console.Write("Score: " + score);
			}
		}
		else
		{
			var tile = (TileState)state;
			if (tile == TileState.HorizontalPaddle)
			{
				paddleX = x;
				paddleY = y;
			}
			if (tile == TileState.Ball)
			{
				var num = 0;
				if (x > paddleX)
					num = 1;
				else if (x < paddleX)
					num = -1;
				await input.WriteAsync(num);
			}
			screen[new(x, y)] = tile;

			if (enableOutput)
			{
				Console.SetCursorPosition((int)x, (int)y + 1);
				var ch = tile switch
				{
					TileState.Empty => " ",
					TileState.Ball => "O",
					TileState.Block => "=",
					TileState.HorizontalPaddle => "=",
					TileState.Wall => "*",
					_ => throw new NotSupportedException("Unexpected tile state"),
				};
				Console.Write(ch);
			}
		}
	}

	if (enableOutput)
		Console.SetCursorPosition(0, (int)paddleY + 3);
	return score;
}


async Task Compute(List<long> original, ChannelReader<long> reader, ChannelWriter<long> writer)
{
	var memory = original.Select((value, i) => (value, (long)i)).ToDictionary(x => x.Item2, x => x.value);
	var state = new ComputerState(memory, reader, writer);
	long i = 0;
	while (true)
	{
		var instruction = state.Get(i, Mode.Immediate);
		var modes = GetModes(instruction / 100, 3).ToArray();
		var opcode = instruction % 100;

		// Run the instruction at the current pointer and find out how many parameters it had
		var parameterCount = opcode switch
		{
			1 => Add(state, i, modes),
			2 => Multiply(state, i, modes),
			3 => await Input(state, i, modes),
			4 => await Output(state, i, modes),
			5 => JumpIfTrue(state, ref i, modes),
			6 => JumpIfFalse(state, ref i, modes),
			7 => LessThan(state, i, modes),
			8 => Equals(state, i, modes),
			9 => RelativeBaseOffset(state, i, modes),
			99 => -1,
			_ => throw new NotSupportedException("Unexpected Opcode: " + opcode),
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

IEnumerable<Mode> GetModes(long num, int minLength)
{
	var returned = 0;
	while (num > 0)
	{
		var digit = num % 10;
		yield return (Mode)digit;
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
static long Add(ComputerState state, long i, Mode[] modes)
{
	state.Set(i + 3, modes[2], state.Get(i + 1, modes[0]) + state.Get(i + 2, modes[1]));
	return 3;
}

/// <summary>
/// Get the values indicated by the pointers, and set product in position in pointer.
/// </summary>
static long Multiply(ComputerState state, long i, Mode[] modes)
{
	state.Set(i + 3, modes[2], state.Get(i + 1, modes[0]) * state.Get(i + 2, modes[1]));
	return 3;
}

/// <summary>
/// Takes a single integer as input and saves it to the position given by its only parameter.
/// </summary>
static async Task<long> Input(ComputerState state, long i, Mode[] modes)
{
	state.Set(i + 1, modes[0], await state.input.ReadAsync());
	return 1;
}

/// <summary>
/// Outputs the value of its only parameter.
/// </summary>
static async Task<long> Output(ComputerState state, long i, Mode[] modes)
{
	await state.output.WriteAsync(state.Get(i + 1, modes[0]));
	return 1;
}

/// <summary>
/// If the first parameter is non-zero,
/// it sets the instruction pointer to the value from the second parameter.
/// Otherwise, it does nothing.
/// </summary>
static long JumpIfTrue(ComputerState state, ref long i, Mode[] modes)
{
	if (state.Get(i + 1, modes[0]) != 0)
	{
		i = state.Get(i + 2, modes[1]);
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
static long JumpIfFalse(ComputerState state, ref long i, Mode[] modes)
{
	if (state.Get(i + 1, modes[0]) == 0)
	{
		i = state.Get(i + 2, modes[1]);
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
static long LessThan(ComputerState state, long i, Mode[] modes)
{
	state.Set(i + 3, modes[2], state.Get(i + 1, modes[0]) < state.Get(i + 2, modes[1]) ? 1 : 0);
	return 3;
}

/// <summary>
/// If the first parameter is equal to the second parameter,
/// it stores 1 in the position given by the third parameter.
/// Otherwise, it stores 0.
/// </summary>
static long Equals(ComputerState state, long i, Mode[] modes)
{
	state.Set(i + 3, modes[2], state.Get(i + 1, modes[0]) == state.Get(i + 2, modes[1]) ? 1 : 0);
	return 3;
}

/// <summary>
/// Adjusts the relative base by the value of its only parameter.
/// The relative base increases (or decreases, if the value is negative) by the value of the parameter.
/// </summary>
static long RelativeBaseOffset(ComputerState state, long i, Mode[] modes)
{
	state.RelativeBase += state.Get(i + 1, modes[0]);
	return 1;
}

public record ComputerState(Dictionary<long, long> memory, ChannelReader<long> input, ChannelWriter<long> output)
{
	public long RelativeBase = 0;
	public void Set(long index, Mode mode, long value)
	{
		if (mode == Mode.Immediate)
			throw new NotSupportedException("Unexpected mode for write");
		var realIndex = GetIndex(index, mode);
		memory[realIndex] = value;
	}
	private long Get(long index) => memory.GetValueOrDefault(index);
	public long Get(long index, Mode mode)
	{
		var realIndex = GetIndex(index, mode);
		return Get(realIndex);
	}
	public long GetIndex(long index, Mode mode)
	{
		return mode switch
		{
			Mode.Position => Get(index),// position mode
			Mode.Immediate => index,// immediate mode
			Mode.Relative => RelativeBase + Get(index),// relative mode
			_ => throw new NotSupportedException("Unexpected mode: " + mode),
		};
	}
}

public enum Mode
{
	Position = 0,
	Immediate = 1,
	Relative = 2,
}
public readonly record struct Point(long x, long y);

public enum TileState
{
	/// <summary>Nothing</summary>
	Empty = 0,
	/// <summary>Indestructable</summary>
	Wall = 1,
	/// <summary>Can be broken by the ball</summary>
	Block = 2,
	/// <summary>Indestructable</summary>
	HorizontalPaddle = 3,
	/// <summary>Moves diagonally and bounces off objects</summary>
	Ball = 4,
}
