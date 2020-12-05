using System;
using System.Linq;

var seats = System.IO.File.ReadLines("input.txt").Select(line =>
{
	Seat seat = new(0, 127, 0, 7);
	foreach (var letter in line)
	{
		seat = letter switch
		{
			'B' => seat with { rowMin = seat.rowMin + (seat.rowMax - seat.rowMin + 1) / 2 },
			'F' => seat with { rowMax = seat.rowMax - (seat.rowMax - seat.rowMin + 1) / 2 },
			'R' => seat with { colMin = seat.colMin + (seat.colMax - seat.colMin + 1) / 2 },
			'L' => seat with { colMax = seat.colMax - (seat.colMax - seat.colMin + 1) / 2 },
			_ => throw new NotSupportedException(),
		};
	}
	return seat.Id;
}).ToList();

Console.WriteLine(seats.Max());

var missing = Enumerable.Range(0, 127 * 8 + 7).Except(seats);
foreach (var seat in missing)
{
	if (!missing.Contains(seat + 1) && !missing.Contains(seat - 1))
		Console.WriteLine(seat);
}

record Seat(int rowMin, int rowMax, int colMin, int colMax)
{
	public int Id => rowMax * 8 + colMax;
}
