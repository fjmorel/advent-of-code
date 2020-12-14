using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var read = System.IO.File.ReadAllLines("input.txt");

// Part 1
var earliest = int.Parse(read[0]);
var firstBus = read[1]
	.Split(',')
	.Where(x => x != "x")
	.Select(x => int.Parse(x))
	.Select(x => (id: x, delay: (earliest / x + 1) * x))
	.OrderBy(x => x.delay)
	.First();
WriteLine(firstBus.id * (firstBus.delay - earliest));

// Part 2
var desc = read[1]
	.Split(',')
	.Select((id, i) => (id, i))
	.Where(x => x.id != "x")
	.Select(x => (id: long.Parse(x.id), index: x.i))
	.OrderByDescending(x => x.id)
	.ToList();
var biggest = desc.First();
long ts = biggest.id - biggest.index;
long factor = 1;
foreach (var x in desc)
{
	while ((ts + x.index) % x.id != 0)
		ts += factor;
	factor *= x.id;
}
WriteLine(ts);
