using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static System.Console;

var read = System.IO.File.ReadLines("input.txt").Select(x => (x[0], int.Parse(x.Substring(1)))).ToList();
var vector = new Vector(Direction.E, 0, 0);

foreach (var line in read)
{
	vector = GetNewPosition(line.Item1, line.Item2, vector);
}

WriteLine(Math.Abs(vector.X) + Math.Abs(vector.Y));

Vector GetNewPosition(char dir, int adj, Vector vector) => dir switch
{
	'N' => vector with { Y = vector.Y + adj },
	'S' => vector with { Y = vector.Y - adj },

	'E' => vector with { X = vector.X + adj },
	'W' => vector with { X = vector.X - adj },

	'L' => vector with { Direction = GetNewDirection(vector.Direction, adj) },
	'R' => vector with { Direction = GetNewDirection(vector.Direction, 360 - adj) },

	'F' => GetNewPosition(vector.Direction.ToString()[0], adj, vector),
	_ => throw new NotSupportedException(),
};

Direction GetNewDirection(Direction current, int adj) => (Direction)(((int)current + adj) % 360);

enum Direction { E = 0, N = 90, W = 180, S = 270 }

record Vector(Direction Direction, int X, int Y);
