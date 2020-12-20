using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var list = System.IO.File.ReadAllLines("input.txt");

var tiles = new List<Tile>();
var adjacent = new Dictionary<int, List<AdjacentTile>>();
for (var i = 0; i < list.Length; i += 12)
	tiles.Add(new(int.Parse(list[i][5..^1]), list[(i + 1)..(i + 11)].Select(x => x.Select(x => x == '#').ToArray()).ToArray()));


var timer = Stopwatch.StartNew();
WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	tiles[0] = tiles[0] with { Transformed = true };
	var queue = new Queue<Tile>();
	queue.Enqueue(tiles[0]);

	while (queue.Any())
	{
		var mainTile = queue.Dequeue();
		var matches = tiles
			.Where(x => x != mainTile)
			.Select(tile => (tile, result: AreAdjacent(mainTile, tile)))
			.Where(x => x.result.side != Side.None)
			.ToList();

		// Store list of adjacent tiles
		adjacent[mainTile.Id] = matches.Select(x => new AdjacentTile(x.tile.Id, x.result.side)).ToList();
		// Transform matches as needed only once, and then find their matches
		foreach (var match in matches)
		{
			var otherTile = match.tile;
			if (!otherTile.Transformed)
			{
				var adj = ApplyTransforms(otherTile.contents, match.result.transforms);
				var index = tiles.IndexOf(otherTile);
				tiles[index] = otherTile with { contents = adj, Transformed = true };
				queue.Enqueue(tiles[index]);
			}
			else
			{
				if (match.result.transforms.Any())
					WriteLine("unexpected transforms");
			}
		}
	}

	return adjacent.Where(x => x.Value.Count == 2).Aggregate(1L, (acc, value) => acc * value.Key);
}

long Part2()
{
	var sea = BuildSea();
	var totalTrues = sea.Sum(x => x.Count(y => y));
	var monster = new string[]
	{
		"                  # ",
		"#    ##    ##    ###",
		" #  #  #  #  #  #   ",
	};
	var monsterPattern = monster
		.SelectMany((line, i) => line.Select((ch, j) => (i, j, isNeeded: ch == '#')))
		.Where(x => x.isNeeded)
		.Select(a => new Coord(a.i, a.j))
		.ToList();
	var monsterTrues = monsterPattern.Count;

	var uniqueTransformSets = new List<Transformation[]>()
	{
		new[]{ Transformation.None, },
		// Top matches
		new[]{ Transformation.FlipVertical },
		new[]{ Transformation.RotateLeft },
		new[]{ Transformation.RotateLeft, Transformation.FlipVertical },
		new[]{ Transformation.FlipVertical, Transformation.FlipHorizontal },
		new[]{ Transformation.FlipHorizontal },
		new[]{ Transformation.RotateLeft, Transformation.FlipHorizontal },
		new[]{ Transformation.RotateRight },
		// Bottom matches
		new[]{ Transformation.FlipVertical },
		new[]{ Transformation.RotateRight, Transformation.FlipHorizontal },
		new[]{ Transformation.RotateLeft },
		new[]{ Transformation.FlipHorizontal },
		new[]{ Transformation.FlipVertical, Transformation.FlipHorizontal },
		new[]{ Transformation.RotateRight },
		new[]{ Transformation.RotateLeft, Transformation.FlipHorizontal },
		// Left matches
		new[]{ Transformation.RotateRight },
		new[]{ Transformation.RotateLeft, Transformation.FlipVertical },
		new[]{ Transformation.FlipHorizontal },
		new[]{ Transformation.RotateRight, Transformation.FlipVertical },
		new[]{ Transformation.RotateLeft },
		new[]{ Transformation.FlipHorizontal, Transformation.FlipVertical },
		new[]{ Transformation.FlipVertical },
		// Right matches
		new[]{ Transformation.RotateLeft, Transformation.FlipVertical },
		new[]{ Transformation.RotateRight },
		new[]{ Transformation.FlipHorizontal },
		new[]{ Transformation.RotateLeft },
		new[]{ Transformation.RotateRight, Transformation.FlipVertical },
		new[]{ Transformation.FlipVertical },
		new[]{ Transformation.FlipHorizontal, Transformation.FlipVertical },
	};

	foreach (var transforms in uniqueTransformSets)
	{
		var adjSea = ApplyTransforms(sea, transforms);
		var numMonsters = FindMonsters(adjSea, monsterPattern);
		if (numMonsters > 0)
			return totalTrues - numMonsters * monsterTrues;
	}
	return 0;
}

long FindMonsters(bool[][] sea, List<Coord> monsterPattern)
{
	var num = 0;
	var monsterWidth = monsterPattern.Max(c => c.X);
	var monsterHeight = monsterPattern.Max(c => c.Y);
	var seaSize = sea.Length;
	for (var x = 0; x < sea[0].Length - monsterWidth; x++)
	{
		for (var y = 0; y < sea.Length - monsterHeight; y++)
		{
			var match = monsterPattern.All(c => sea[x + c.X][y + c.Y]);
			if (match)
				num++;
		}
	}
	return num;
}


bool[][] BuildSea()
{
	var tileGridSize = (int)Math.Sqrt(tiles.Count);
	var tileGrid = Enumerable.Range(0, tileGridSize).Select(x => new int[tileGridSize]).ToArray();

	var corners = adjacent.Where(x => x.Value.Count == 2).ToList();
	var origin = corners.Single(x => x.Value.All(y => y.side == Side.Right || y.side == Side.Bottom));
	tileGrid[0][0] = origin.Key;
	for (var i = 0; i < tileGridSize; i++)
	{
		for (var j = 0; j < tileGridSize; j++)
		{
			if (i == 0 && j == 0)
				continue;
			if (i == 0)
				tileGrid[i][j] = adjacent[tileGrid[i][j - 1]].Single(x => x.side == Side.Right).Id;
			else
				tileGrid[i][j] = adjacent[tileGrid[i - 1][j]].Single(x => x.side == Side.Bottom).Id;
		}
	}

	var dict = tiles.ToDictionary(x => x.Id, x => x.contents.Skip(1).SkipLast(1).Select(x => x.Skip(1).SkipLast(1).ToArray()).ToArray());
	var sea = new List<bool[]>();

	for (var i = 0; i < tileGrid.Length; i++)
	{
		var tiles = tileGrid[i].Select(x => dict[x]).ToList();
		for (var j = 0; j < tiles[0].Length; j++)
		{
			sea.Add(tiles.SelectMany((x, y) => x[j]).ToArray());
		}
	}

	return sea.ToArray();
}

Match AreAdjacent(Tile first, Tile second)
{
	if (first.Top.SequenceEqual(second.Top))
		return new(Side.Top, Transformation.FlipVertical);
	if (first.Top.SequenceEqual(second.Bottom))
		return new(Side.Top);
	if (first.Top.SequenceEqual(second.Left))
		return new(Side.Top, Transformation.RotateLeft);
	if (first.Top.SequenceEqual(second.Right))
		return new(Side.Top, Transformation.RotateLeft, Transformation.FlipVertical);
	if (first.Top.SequenceEqual(second.ReverseTop))
		return new(Side.Top, Transformation.FlipVertical, Transformation.FlipHorizontal);
	if (first.Top.SequenceEqual(second.ReverseBottom))
		return new(Side.Top, Transformation.FlipHorizontal);
	if (first.Top.SequenceEqual(second.ReverseLeft))
		return new(Side.Top, Transformation.RotateLeft, Transformation.FlipHorizontal);
	if (first.Top.SequenceEqual(second.ReverseRight))
		return new(Side.Top, Transformation.RotateRight);

	if (first.Bottom.SequenceEqual(second.Top))
		return new(Side.Bottom);
	if (first.Bottom.SequenceEqual(second.Bottom))
		return new(Side.Bottom, Transformation.FlipVertical);
	if (first.Bottom.SequenceEqual(second.Left))
		return new(Side.Bottom, Transformation.RotateRight, Transformation.FlipHorizontal);
	if (first.Bottom.SequenceEqual(second.Right))
		return new(Side.Bottom, Transformation.RotateLeft);
	if (first.Bottom.SequenceEqual(second.ReverseTop))
		return new(Side.Bottom, Transformation.FlipHorizontal);
	if (first.Bottom.SequenceEqual(second.ReverseBottom))
		return new(Side.Bottom, Transformation.FlipVertical, Transformation.FlipHorizontal);
	if (first.Bottom.SequenceEqual(second.ReverseLeft))
		return new(Side.Bottom, Transformation.RotateRight);
	if (first.Bottom.SequenceEqual(second.ReverseRight))
		return new(Side.Bottom, Transformation.RotateLeft, Transformation.FlipHorizontal);

	if (first.Left.SequenceEqual(second.Top))
		return new(Side.Left, Transformation.RotateRight);
	if (first.Left.SequenceEqual(second.Bottom))
		return new(Side.Left, Transformation.RotateLeft, Transformation.FlipVertical);
	if (first.Left.SequenceEqual(second.Left))
		return new(Side.Left, Transformation.FlipHorizontal);
	if (first.Left.SequenceEqual(second.Right))
		return new(Side.Left);
	if (first.Left.SequenceEqual(second.ReverseTop))
		return new(Side.Left, Transformation.RotateRight, Transformation.FlipVertical);
	if (first.Left.SequenceEqual(second.ReverseBottom))
		return new(Side.Left, Transformation.RotateLeft);
	if (first.Left.SequenceEqual(second.ReverseLeft))
		return new(Side.Left, Transformation.FlipHorizontal, Transformation.FlipVertical);
	if (first.Left.SequenceEqual(second.ReverseRight))
		return new(Side.Left, Transformation.FlipVertical);

	if (first.Right.SequenceEqual(second.Top))
		return new(Side.Right, Transformation.RotateLeft, Transformation.FlipVertical);
	if (first.Right.SequenceEqual(second.Bottom))
		return new(Side.Right, Transformation.RotateRight);
	if (first.Right.SequenceEqual(second.Left))
		return new(Side.Right);
	if (first.Right.SequenceEqual(second.Right))
		return new(Side.Right, Transformation.FlipHorizontal);
	if (first.Right.SequenceEqual(second.ReverseTop))
		return new(Side.Right, Transformation.RotateLeft);
	if (first.Right.SequenceEqual(second.ReverseBottom))
		return new(Side.Right, Transformation.RotateRight, Transformation.FlipVertical);
	if (first.Right.SequenceEqual(second.ReverseLeft))
		return new(Side.Right, Transformation.FlipVertical);
	if (first.Right.SequenceEqual(second.ReverseRight))
		return new(Side.Right, Transformation.FlipHorizontal, Transformation.FlipVertical);

	return new(Side.None);
}

T[][] ApplyTransforms<T>(T[][] original, params Transformation[] transformations)
{
	var adj = original;
	foreach (var transform in transformations)
	{
		adj = (transform switch
		{
			Transformation.None => adj,
			Transformation.FlipVertical => adj.Reverse(),
			Transformation.FlipHorizontal => adj.Select(x => x.Reverse().ToArray()),
			Transformation.RotateLeft => adj.Select((x, i) => adj.Select(y => y[i]).ToArray()).Reverse(),
			Transformation.RotateRight => adj.Reverse().Select((x, i) => adj.Select(y => y[i]).Reverse().ToArray()),
			_ => throw new NotSupportedException(),
		}).ToArray();
	}
	return adj;
}

record Tile(int Id, bool[][] contents, bool Transformed = false)
{
	public IEnumerable<bool> Top => contents.First();
	public IEnumerable<bool> Right => contents.Select(x => x.Last());
	public IEnumerable<bool> Bottom => contents.Last();
	public IEnumerable<bool> Left => contents.Select(x => x.First());
	public IEnumerable<bool> ReverseTop => Top.Reverse();
	public IEnumerable<bool> ReverseRight => Right.Reverse();
	public IEnumerable<bool> ReverseBottom => Bottom.Reverse();
	public IEnumerable<bool> ReverseLeft => Left.Reverse();
}

record Match(Side side, params Transformation[] transforms);
record AdjacentTile(int Id, Side side);
record Coord(int X, int Y);

enum Side { None, Left, Top, Right, Bottom }
enum Transformation { None, FlipVertical, FlipHorizontal, RotateLeft, RotateRight }
