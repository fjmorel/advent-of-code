using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;


var read = System.IO.File.ReadAllLines("input.txt");
var earliest = int.Parse(read[0]);
var buses = read[1].Split(',').Where(x => x != "x").Select(x => int.Parse(x)).ToList();
var indexedBuses = read[1].Split(',').Select((id, i) => (id, i)).Where(x => x.id != "x").Select(x => (id: long.Parse(x.id), x.i)).ToList();
var times = new Dictionary<int, int>();

foreach (var bus in buses)
	times[bus] = (earliest / bus + 1) * bus;
var kv = times.OrderBy(x => x.Value).First();
WriteLine($"Bus {kv.Key}. Wait {kv.Value - earliest}. Solution: {kv.Key * (kv.Value - earliest)}");

var found = false;
var biggest = indexedBuses.OrderByDescending(x => x.id).First();
var interval = biggest.id;
var offset = biggest.i;
for (long timestamp = interval - offset; !found; timestamp += interval)
{
	if (indexedBuses.All(x => (timestamp + x.i) % x.id == 0))
	{
		WriteLine(timestamp);
		found = true;
	}
}
