using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Console;

var prev = System.IO.File.ReadLines("input.txt").Select(x => x.Select(ParseChar).ToArray()).ToArray();
var size = new Point(prev.Length, prev[0].Length);
var cur = Iterate(prev, size);
while (!Equal(prev, cur))
{
	prev = cur;
	cur = Iterate(prev, size);
}

WriteLine(cur.SelectMany(x => x.Where(y => y == State.Filled)).Count());

static State[][] Iterate(State[][] prev, Point size)
{
	var cur = CloneArray(prev);
	for (var i = 0; i < size.i; i++)
	{
		for (var j = 0; j < size.j; j++)
		{
			var value = prev[i][j];
			cur[i][j] = value switch
			{
				State.Floor => State.Floor,
				State.Empty => MaybeFill(value, new Point(i, j), size, prev),
				State.Filled => MaybeClear(value, new Point(i, j), size, prev),
				_ => throw new NotSupportedException(),
			};
		}
	}

	return cur;
}

static State MaybeClear(State value, Point coord, Point size, State[][] prev)
	=> GetSurroundingPositions(coord, size)
		.Where(adj => prev[adj.i][adj.j] == State.Filled)
		.Count() >= 4 ? State.Empty : value;

static State MaybeFill(State value, Point coord, Point size, State[][] prev)
	=> GetSurroundingPositions(coord, size)
		.Any(adj => prev[adj.i][adj.j] == State.Filled)
		? State.Empty : State.Filled;

static IEnumerable<Point> GetSurroundingPositions(Point coord, Point size)
{
	var points = new List<Point>()
	{
		coord with { i = coord.i - 1, j = coord.j - 1 },
		coord with { i = coord.i, j = coord.j - 1 },
		coord with { i = coord.i + 1, j = coord.j - 1 },

		coord with { i = coord.i + -1 },
		coord with { i = coord.i + 1 },

		coord with { i = coord.i - 1, j = coord.j + 1 },
		coord with { i = coord.i, j = coord.j + 1 },
		coord with { i = coord.i + 1, j = coord.j + 1 },
	};
	return points.Where(x => x.i >= 0 && x.i < size.i && x.j >= 0 && x.j < size.j);
}

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

record Point(int i, int j);
