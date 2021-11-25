var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");
List<long> opCodes = list[0].Split(',').Select(x => long.Parse(x)).ToList();

var timer = Stopwatch.StartNew();
Console.WriteLine($"{await Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{await Part2()} :: {timer.Elapsed}");
timer.Stop();

async Task<long> Part1()
{
	var grid = await RunPaintJob(0);
	return grid.Count;
}

async Task<long> Part2()
{
	var grid = await RunPaintJob(1);

	var minX = grid.Keys.MinBy(p => p.x).x;
	var maxX = grid.Keys.MaxBy(p => p.x).x;

	var minY = grid.Keys.MinBy(p => p.y).y;
	var maxY = grid.Keys.MaxBy(p => p.y).y;

	// still not sure why this axis was inverted
	for (var y = maxY; y >= minY; y--)
	{
		for (var x = minX; x <= maxX; x++)
		{
			var color = grid.GetValueOrDefault(new Point(x, y));
			Console.Write(color == 1 ? '#' : ' ');
		}
		Console.WriteLine();
	}
	return -1;
}

async Task<Dictionary<Point, long>> RunPaintJob(long startColor)
{

	var sensorChannel = Channel.CreateUnbounded<long>();
	var botChannel = Channel.CreateUnbounded<long>();
	var writer = sensorChannel.Writer;
	var reader = sensorChannel.Reader;

	var compute = Compute(opCodes, sensorChannel.Reader, botChannel.Writer);
	var paint = Paint(botChannel.Reader, sensorChannel.Writer, startColor);

	await Task.WhenAll(compute, paint);
	sensorChannel.Writer.TryComplete();

	return await paint;
}

async Task<Dictionary<Point, long>> Paint(ChannelReader<long> reader, ChannelWriter<long> writer, long startColor)
{
	var grid = new Dictionary<Point, long>();

	var current = default(Point);
	var direction = Direction.Up;

	await writer.WriteAsync(startColor);
	while (await reader.WaitToReadAsync())
	{
		var newColor = await reader.ReadAsync();
		grid[current] = newColor;

		var turn = await reader.ReadAsync();
		direction = (direction, turn) switch
		{
			(Direction.Up, 1) => Direction.Right,
			(Direction.Right, 1) => Direction.Down,
			(Direction.Down, 1) => Direction.Left,
			(Direction.Left, 1) => Direction.Up,

			(Direction.Up, 0) => Direction.Left,
			(Direction.Left, 0) => Direction.Down,
			(Direction.Down, 0) => Direction.Right,
			(Direction.Right, 0) => Direction.Up,
			_ => throw new NotSupportedException("Unexpected data"),
		};
		current = direction switch
		{
			Direction.Up => current with { y = current.y + 1 },
			Direction.Down => current with { y = current.y - 1 },

			Direction.Right => current with { x = current.x + 1 },
			Direction.Left => current with { x = current.x - 1 },
			_ => throw new NotSupportedException("Unexpected data"),
		};
		await writer.WriteAsync(grid.GetValueOrDefault(current, startColor));
	}

	return grid;
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
public readonly record struct Point(int x, int y);
public enum Direction { Up, Right, Down, Left }