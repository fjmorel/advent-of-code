using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

var list = System.IO.File.ReadAllLines("input.txt");

// Set up initial tiles (true if black)
var tiles = new Dictionary<Coordinate, bool>();
foreach (var line in list)
{
	var position = GetCoordinate(ParseLine(line));
	tiles[position] = !tiles.GetValueOrDefault(position, false);
}

var timer = Stopwatch.StartNew();
WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	return tiles.Count(x => x.Value);
}

long Part2()
{
	for (var i = 1; i <= 100; i++)
	{
		var currentBlack = tiles.Where(x => x.Value).Select(x => x.Key).ToHashSet();
		var positionsToCheck = tiles.Keys.SelectMany(x => x.GetNeighbors(true)).ToHashSet();
		foreach (var position in positionsToCheck)
		{
			var isBlack = tiles.GetValueOrDefault(position, false);
			var blackNeighbors = position.GetNeighbors().Count(x => currentBlack.Contains(x));
			if (isBlack)
				tiles[position] = !(blackNeighbors == 0 || blackNeighbors > 2);
			else
				tiles[position] = blackNeighbors == 2;
		}
	}
	return tiles.Count(x => x.Value);
}

Coordinate GetCoordinate(IEnumerable<Direction> directions)
	=> directions.Aggregate(Coordinate.Origin, (pos, dir) => pos.WithMove(dir));

// old awful way of turning long list of movements into shortest possible move
string CondenseDirections(IEnumerable<Direction> directions)
{
	var ALL_DIRECTIONS = new Direction[] { Direction.E, Direction.SE, Direction.NE, Direction.W, Direction.SW, Direction.NW };
	var sums = directions
		.GroupBy(x => x)
		.Select(x => new { Key = x.Key, Count = x.Count() })
		.ToDictionary(x => x.Key, x => x.Count);
	foreach (var direction in ALL_DIRECTIONS)
		sums[direction] = sums.GetValueOrDefault(direction, 0);


	while (sums[Direction.E] > 0 && sums[Direction.NW] > 0)
	{
		sums[Direction.E]--;
		sums[Direction.NW]--;
		sums[Direction.NE]++;
	}
	while (sums[Direction.E] > 0 && sums[Direction.SW] > 0)
	{
		sums[Direction.E]--;
		sums[Direction.SW]--;
		sums[Direction.SE]++;
	}
	while (sums[Direction.SE] > 0 && sums[Direction.NE] > 0)
	{
		sums[Direction.SE]--;
		sums[Direction.NE]--;
		sums[Direction.E]++;
	}
	while (sums[Direction.W] > 0 && sums[Direction.NE] > 0)
	{
		sums[Direction.W]--;
		sums[Direction.NE]--;
		sums[Direction.NW]++;
	}
	while (sums[Direction.W] > 0 && sums[Direction.SE] > 0)
	{
		sums[Direction.W]--;
		sums[Direction.SE]--;
		sums[Direction.SW]++;
	}
	while (sums[Direction.SW] > 0 && sums[Direction.NW] > 0)
	{
		sums[Direction.SW]--;
		sums[Direction.NW]--;
		sums[Direction.W]++;
	}
	while (sums[Direction.E] > 0 && sums[Direction.W] > 0)
	{
		sums[Direction.E]--;
		sums[Direction.W]--;
	}
	while (sums[Direction.SW] > 0 && sums[Direction.NE] > 0)
	{
		sums[Direction.NE]--;
		sums[Direction.SW]--;
	}
	while (sums[Direction.SE] > 0 && sums[Direction.NW] > 0)
	{
		sums[Direction.SE]--;
		sums[Direction.NW]--;
	}

	while (sums[Direction.E] > 0 && sums[Direction.NW] > 0)
	{
		sums[Direction.E]--;
		sums[Direction.NW]--;
		sums[Direction.NE]++;
	}
	while (sums[Direction.E] > 0 && sums[Direction.SW] > 0)
	{
		sums[Direction.E]--;
		sums[Direction.SW]--;
		sums[Direction.SE]++;
	}
	while (sums[Direction.SE] > 0 && sums[Direction.NE] > 0)
	{
		sums[Direction.SE]--;
		sums[Direction.NE]--;
		sums[Direction.E]++;
	}
	while (sums[Direction.W] > 0 && sums[Direction.NE] > 0)
	{
		sums[Direction.W]--;
		sums[Direction.NE]--;
		sums[Direction.NW]++;
	}
	while (sums[Direction.W] > 0 && sums[Direction.SE] > 0)
	{
		sums[Direction.W]--;
		sums[Direction.SE]--;
		sums[Direction.SW]++;
	}
	while (sums[Direction.SW] > 0 && sums[Direction.NW] > 0)
	{
		sums[Direction.SW]--;
		sums[Direction.NW]--;
		sums[Direction.W]++;
	}
	while (sums[Direction.E] > 0 && sums[Direction.W] > 0)
	{
		sums[Direction.E]--;
		sums[Direction.W]--;
	}
	while (sums[Direction.SW] > 0 && sums[Direction.NE] > 0)
	{
		sums[Direction.NE]--;
		sums[Direction.SW]--;
	}
	while (sums[Direction.SE] > 0 && sums[Direction.NW] > 0)
	{
		sums[Direction.SE]--;
		sums[Direction.NW]--;
	}

	var position = new StringBuilder();
	foreach (var direction in ALL_DIRECTIONS)
		for (var i = 0; i < sums[direction]; i++)
			position.Append(direction.ToString());
	return position.ToString();

}

IEnumerable<Direction> ParseLine(string line)
{
	for (var i = 0; i < line.Length; i++)
	{
		if (line[i] == 'e')
			yield return Direction.E;
		else if (line[i] == 'w')
			yield return Direction.W;
		else
		{
			yield return line[i..(i + 2)] switch
			{
				"se" => Direction.SE,
				"sw" => Direction.SW,
				"ne" => Direction.NE,
				"nw" => Direction.NW,
				_ => throw new NotSupportedException(),
			};
			i++;
		}
	}
}

/// <summary>
/// X = W<-->E, Y = NW<-->SE, Z = NE<-->SW. x+y+z==0
/// </summary>
record Coordinate(int X, int Y, int Z)
{
	public static Coordinate Origin = new(0, 0, 0);

	public Coordinate WithMove(Direction direction) => direction switch
	{
		Direction.E => this with { X = X + 1, Y = Y - 1 },
		Direction.W => this with { X = X - 1, Y = Y + 1 },

		Direction.SE => this with { Z = Z + 1, Y = Y - 1 },
		Direction.NW => this with { Z = Z - 1, Y = Y + 1 },

		Direction.NE => this with { X = X + 1, Z = Z - 1 },
		Direction.SW => this with { X = X - 1, Z = Z + 1 },

		_ => throw new NotSupportedException(),
	};

	public IEnumerable<Coordinate> GetNeighbors(bool includeSelf = false)
	{
		if (includeSelf)
			yield return this;
		yield return this.WithMove(Direction.E);
		yield return this.WithMove(Direction.SE);
		yield return this.WithMove(Direction.NE);
		yield return this.WithMove(Direction.W);
		yield return this.WithMove(Direction.SW);
		yield return this.WithMove(Direction.NW);
	}
}

enum Direction { E, SE, NE, W, SW, NW }
