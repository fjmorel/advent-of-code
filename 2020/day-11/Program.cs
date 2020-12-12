using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

var read = System.IO.File.ReadLines("input.txt").Select(x => x.Select(ParseChar).ToArray()).ToArray();
var size = new Position(read.Length, read[0].Length);

WriteLine(FindSteadyState(read, size, 4, false));
WriteLine(FindSteadyState(read, size, 5, true));

static int FindSteadyState(State[][] read, Position size, int threshold, bool lineOfSight)
{
	var write = Iterate(read, size, threshold, lineOfSight);
	while (!read.SelectMany(x => x).SequenceEqual(write.SelectMany(x => x)))
	{
		read = write;
		write = Iterate(read, size, threshold, lineOfSight);
	}
	return write.Sum(x => x.Count(y => y == State.Occupied));
}

static State[][] Iterate(State[][] read, Position size, int threshold, bool lineOfSight)
	=> read.Select((row, i) => row.Select((State, j) => FindNewState(new Position(i, j), read, size, threshold, lineOfSight)).ToArray()).ToArray();

static State FindNewState(Position seat, State[][] read, Position size, int threshold, bool lineOfSight)
	=> read[seat.i][seat.j] switch
	{
		State.Floor => State.Floor,
		State.Empty => MaybeFill(seat, size, read, lineOfSight),
		State.Occupied => MaybeClear(seat, size, read, threshold, lineOfSight),
		_ => throw new NotSupportedException(),
	};

static State MaybeClear(Position seat, Position size, State[][] read, int threshold, bool lineOfSight)
	=> GetSurroundingPositions(seat, size, read, lineOfSight)
		.Where(adj => read[adj.i][adj.j] == State.Occupied)
		.Count() >= threshold ? State.Empty : State.Occupied;

static State MaybeFill(Position seat, Position size, State[][] read, bool lineOfSight)
	=> GetSurroundingPositions(seat, size, read, lineOfSight)
		.Any(adj => read[adj.i][adj.j] == State.Occupied)
		? State.Empty : State.Occupied;

static IEnumerable<Position> GetSurroundingPositions(Position coord, Position size, State[][] prev, bool lineOfSight)
{
	IEnumerable<Position> seats = new List<Position>()
	{
		new Position(-1, -1), new Position(-1, 0), new Position(-1, 1),
		new Position(0, -1), new Position(0, 1),
		new Position(1, -1), new Position(1, 0), new Position(1, 1),
	};
	if (!lineOfSight)
		seats = seats.Select(x => coord + x);
	else
		seats = seats.Select(inc => FindLineOfSight(coord, size, prev, inc));
	return seats.Where(x => x.IsWithin(size));
}

static Position FindLineOfSight(Position coord, Position size, State[][] prev, Position inc)
{
	var adj = coord + inc;
	while (adj.IsWithin(size) && prev[adj.i][adj.j] == State.Floor)
		adj = adj + inc;
	return adj;
}

static State ParseChar(char letter) => letter switch
{
	'.' => State.Floor,
	'L' => State.Empty,
	'#' => State.Occupied,
	_ => throw new NotSupportedException(),
};

enum State { Floor, Empty, Occupied }

record Position(int i, int j)
{
	public static Position operator +(Position a, Position b) => new Position(a.i + b.i, a.j + b.j);

	public bool IsWithin(Position size) => this.i >= 0 && this.i < size.i && this.j >= 0 && this.j < size.j;
}
