using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

var read = System.IO.File.ReadLines("input.txt").Select(x => (x[0], int.Parse(x.Substring(1)))).ToList();

var vector = read.Aggregate(new Vector(Direction.E, 0, 0), (vector, line) => GetNewPosition(line.Item1, line.Item2, vector));
WriteLine(Math.Abs(vector.X) + Math.Abs(vector.Y));

var waypoint = new WaypointNav(0, 0, 10, 1);
foreach (var line in read)
	waypoint = GetNewPositionWaypoint(line.Item1, line.Item2, waypoint);
WriteLine(Math.Abs(waypoint.ShipX) + Math.Abs(waypoint.ShipY));

Vector GetNewPosition(char dir, int adj, Vector vector) => dir switch
{
	'N' => vector with { Y = vector.Y + adj },
	'S' => vector with { Y = vector.Y - adj },

	'E' => vector with { X = vector.X + adj },
	'W' => vector with { X = vector.X - adj },

	'L' => vector with { Direction = (Direction)(((int)vector.Direction + adj) % 360) },
	'R' => vector with { Direction = (Direction)(((int)vector.Direction + 360 - adj) % 360) },

	'F' => GetNewPosition(vector.Direction.ToString()[0], adj, vector),
	_ => throw new NotSupportedException(),
};

WaypointNav GetNewPositionWaypoint(char dir, int adj, WaypointNav pt) => dir switch
{
	'N' => pt with { WaypointY = pt.WaypointY + adj },
	'S' => pt with { WaypointY = pt.WaypointY - adj },

	'E' => pt with { WaypointX = pt.WaypointX + adj },
	'W' => pt with { WaypointX = pt.WaypointX - adj },

	'L' => TurnWaypoint(pt, adj),
	'R' => TurnWaypoint(pt, 360 - adj),

	'F' => pt with { ShipX = pt.ShipX + adj * pt.WaypointX, ShipY = pt.ShipY + adj * pt.WaypointY },
	_ => throw new NotSupportedException(),
};

WaypointNav TurnWaypoint(WaypointNav pt, int adj) => adj switch
{
	0 => pt,
	90 => pt with { WaypointX = -pt.WaypointY, WaypointY = pt.WaypointX },
	180 => pt with { WaypointX = -pt.WaypointX, WaypointY = pt.WaypointY },
	270 => pt with { WaypointX = pt.WaypointY, WaypointY = -pt.WaypointX },
	_ => throw new NotSupportedException(),
};

enum Direction { E = 0, N = 90, W = 180, S = 270 }

record Vector(Direction Direction, int X, int Y);
record WaypointNav(int ShipX, int ShipY, int WaypointX, int WaypointY);
