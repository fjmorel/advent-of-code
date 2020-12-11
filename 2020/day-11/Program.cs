using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

var prev = System.IO.File.ReadLines("input.txt").Select(x => x.Select(ParseChar).ToArray()).ToArray();
var size = new Seat(prev.Length, prev[0].Length);

WriteLine(FindSteadyState(prev, size, 4, false));
WriteLine(FindSteadyState(prev, size, 5, true));

static int FindSteadyState(State[][] prev, Seat size, int threshold, bool lineOfSight)
{
	var cur = Iterate(prev, size, threshold, lineOfSight);
	while (!Equal(prev, cur))
	{
		prev = cur;
		cur = Iterate(prev, size, threshold, lineOfSight);
	}
	return cur.SelectMany(x => x.Where(y => y == State.Filled)).Count();
}

static State[][] Iterate(State[][] prev, Seat size, int threshold, bool lineOfSight)
{
	var cur = CloneArray(prev);
	for (var i = 0; i < size.i; i++)
	{
		for (var j = 0; j < size.j; j++)
		{
			cur[i][j] = prev[i][j] switch
			{
				State.Floor => State.Floor,
				State.Empty => MaybeFill(new Seat(i, j), size, prev, lineOfSight),
				State.Filled => MaybeClear(new Seat(i, j), size, prev, threshold, lineOfSight),
				_ => throw new NotSupportedException(),
			};
		}
	}

	return cur;
}

static State MaybeClear(Seat coord, Seat size, State[][] prev, int threshold, bool lineOfSight)
	=> GetSurroundingPositions(coord, size, prev, lineOfSight)
		.Where(adj => prev[adj.i][adj.j] == State.Filled)
		.Count() >= threshold ? State.Empty : State.Filled;

static State MaybeFill(Seat coord, Seat size, State[][] prev, bool lineOfSight)
	=> GetSurroundingPositions(coord, size, prev, lineOfSight)
		.Any(adj => prev[adj.i][adj.j] == State.Filled)
		? State.Empty : State.Filled;

static IEnumerable<Seat> GetSurroundingPositions(Seat coord, Seat size, State[][] prev, bool lineOfSight)
{
	if (!lineOfSight)
		return GetIncrements()
			.Select(x => coord + x)
			.Where(x => x.IsWithin(size));

	var seats = new List<Seat>();
	foreach (var inc in GetIncrements())
	{
		var adj = coord + inc;
		while (adj.IsWithin(size) && prev[adj.i][adj.j] == State.Floor)
			adj = adj + inc;

		if (adj.IsWithin(size))
			seats.Add(adj);
	}
	return seats;
}

static List<Seat> GetIncrements() => new List<Seat>()
{
	new Seat(-1, -1),
	new Seat(-1, 0),
	new Seat(-1, 1),
	new Seat(0, -1),
	new Seat(0, 1),
	new Seat(1, -1),
	new Seat(1, 0),
	new Seat(1, 1),
};

static bool Equal(State[][] prev, State[][] cur)
{
	var width = cur[0].Length;
	var height = cur.Length;
	for (var i = 0; i < height; i++)
	{
		for (var j = 0; j < width; j++)
		{
			if (prev[i][j] != cur[i][j])
				return false;
		}
	}
	return true;
}

static State[][] CloneArray(State[][] grid) => grid.Select(x => (State[])x.Clone()).ToArray();

static State ParseChar(char letter) => letter switch
{
	'.' => State.Floor,
	'L' => State.Empty,
	'#' => State.Filled,
	_ => throw new NotSupportedException(),
};

enum State
{
	Floor,
	Empty,
	Filled,
}

record Seat(int i, int j)
{
	public static Seat operator +(Seat a, Seat b) => new Seat(a.i + b.i, a.j + b.j);

	public bool IsWithin(Seat size) => this.i >= 0 && this.i < size.i && this.j >= 0 && this.j < size.j;
}
