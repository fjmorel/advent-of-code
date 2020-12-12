using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

const char FLOOR = '.', EMPTY = 'L', OCCUPIED = '#';
var read = System.IO.File.ReadLines("input.txt").Select(x => x.ToCharArray()).ToArray();
var size = new Position(read.Length, read[0].Length);

WriteLine(FindSteadyState(read, size, 4, false));
WriteLine(FindSteadyState(read, size, 5, true));

static int FindSteadyState(char[][] read, Position size, int threshold, bool lineOfSight)
{
	var positions = read.Select((x, i) => x.Select((y, j) => GetSurroundingPositions(new(i, j), size, read, lineOfSight)).ToArray()).ToArray();
	var write = Iterate(read, threshold, positions);
	while (!read.SelectMany(x => x).SequenceEqual(write.SelectMany(x => x)))
	{
		read = write;
		write = Iterate(read, threshold, positions);
	}
	return write.Sum(x => x.Count(y => y == OCCUPIED));
}

static char[][] Iterate(char[][] read, int threshold, Position[][][] positions)
	=> read.Select((row, i) => row.Select((_, j) => FindNewState(i, j, read, threshold, positions[i][j])).ToArray()).ToArray();

static char FindNewState(int i, int j, char[][] read, int threshold, Position[] positions)
	=> read[i][j] switch
	{
		FLOOR => FLOOR,
		EMPTY => positions.Any(adj => read[adj.i][adj.j] == OCCUPIED) ? EMPTY : OCCUPIED,
		OCCUPIED => positions.Where(adj => read[adj.i][adj.j] == OCCUPIED).Count() >= threshold ? EMPTY : OCCUPIED,
		_ => throw new NotSupportedException(),
	};

static Position[] GetSurroundingPositions(Position coord, Position size, char[][] read, bool lineOfSight)
	=> new Position[] { new(-1, -1), new(-1, 0), new(-1, 1), new(0, -1), new(0, 1), new(1, -1), new(1, 0), new(1, 1) }
		.Select(inc => lineOfSight ? FindLineOfSight(coord + inc, size, read, inc) : coord + inc)
		.Where(x => x.IsWithin(size)).ToArray();

static Position FindLineOfSight(Position adj, Position size, char[][] read, Position inc)
	=> (adj.IsWithin(size) && read[adj.i][adj.j] == FLOOR) ? FindLineOfSight(adj + inc, size, read, inc) : adj;

record Position(int i, int j)
{
	public static Position operator +(Position a, Position b) => new(a.i + b.i, a.j + b.j);
	public bool IsWithin(Position size) => i >= 0 && i < size.i && j >= 0 && j < size.j;
}
