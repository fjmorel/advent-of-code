using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;


var read = System.IO.File.ReadAllLines("input.txt");
var earliest = int.Parse(read[0]);
var buses = read[1].Split(',').Where(x => x != "x").Select(x => int.Parse(x)).ToList();
var times = new Dictionary<int, int>();

foreach (var bus in buses)
{
	times[bus] = FindEarliestAfter(earliest, bus);
}

var kv = times.OrderBy(x => x.Value).First();
WriteLine($"Bus {kv.Key}. Wait {kv.Value - earliest}. Solution: {kv.Key * (kv.Value - earliest)}");

int FindEarliestAfter(int earliest, int bus)
{
	return (earliest / bus + 1) * bus;
}