using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

var part1 = 0;
var part2 = 0;

var seats = System.IO.File.ReadLines("input.txt").Select(line =>
{
	Seat seat = new(0, 127, 0, 7);
	foreach (var letter in line)
	{
		// Front = 0, Left = 0
		seat = letter switch
		{
			'B' => seat with { rowMin = seat.rowMin + (seat.rowMax - seat.rowMin + 1) / 2 },
			'F' => seat with { rowMax = seat.rowMax - (seat.rowMax - seat.rowMin + 1) / 2 },
			'R' => seat with { colMin = seat.colMin + (seat.colMax - seat.colMin + 1) / 2 },
			'L' => seat with { colMax = seat.colMax - (seat.colMax - seat.colMin + 1) / 2 },
			_ => throw new NotSupportedException(),
		};
	}
	return seat;
}).ToList();

Console.WriteLine(seats.Max(x => x.Id));

var grid = new bool[128, 8];
foreach (var seat in seats)
	grid[seat.rowMax, seat.colMax] = true;

var missing = new HashSet<int>();
for (var i = 0; i < 128; i++)
{
	for (var j = 0; j < 8; j++)
	{
		if (!grid[i, j])
			missing.Add(new Seat(i, i, j, j).Id);
	}
}

foreach (var seat in missing)
{
	if (!missing.Contains(seat + 1) && !missing.Contains(seat - 1))
		Console.WriteLine(seat);
}

record Seat(int rowMin, int rowMax, int colMin, int colMax)
{
	public int Id => rowMax * 8 + colMax;
}
