using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

var list = System.IO.File.ReadAllLines("input.txt");

// Tile state (true == black)
var tiles = new Dictionary<Coordinate, bool>();

var timer = Stopwatch.StartNew();
WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	foreach (var line in list)
	{
		var position = ParseLine(line).Aggregate(Coordinate.Origin, (pos, dir) => pos.WithMove(dir));
		tiles[position] = !tiles.GetValueOrDefault(position, false);
	}
	return tiles.Count(x => x.Value);
}

long Part2()
{
	var blackTiles = tiles.Where(x => x.Value).Select(x => x.Key).ToHashSet();
	for (var i = 1; i <= 100; i++)
	{
		blackTiles = blackTiles
			// Add all neighbors as possible new black tiles
			.Concat(blackTiles.SelectMany(x => x.GetNeighbors()))
			// Only check each tile once and in parallel
			.Distinct().AsParallel()
			// Check if the tile should now be black
			.Where(x => x.ShouldBeBlack(blackTiles))
			.ToHashSet();
	}
	return blackTiles.Count;
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
/// https://www.redblobgames.com/grids/hexagons/ Cube coordinates
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

	public IEnumerable<Coordinate> GetNeighbors()
	{
		yield return this.WithMove(Direction.E);
		yield return this.WithMove(Direction.SE);
		yield return this.WithMove(Direction.NE);
		yield return this.WithMove(Direction.W);
		yield return this.WithMove(Direction.SW);
		yield return this.WithMove(Direction.NW);
	}

	public bool ShouldBeBlack(HashSet<Coordinate> allCurrentlyBlack)
	{
		var blackNeighbors = GetNeighbors().Count(x => allCurrentlyBlack.Contains(x));
		return (blackNeighbors == 2 || (blackNeighbors == 1 && allCurrentlyBlack.Contains(this)));
	}
}

enum Direction { E, SE, NE, W, SW, NW }
