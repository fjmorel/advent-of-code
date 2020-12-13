using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

var desc = indexedBuses.OrderByDescending(x => x.id);
var biggest = desc.First();
// var interval = biggest.id;
// var offset = biggest.i;
// Parallel.ForEach(YieldTime(interval - offset, interval), (timestamp, state, index) =>
// {
// 	foreach (var x in desc)
// 	{
// 		if ((timestamp + x.i) % x.id != 0)
// 			return;
// 	}
// 	WriteLine(timestamp);
// 	state.Break();
// });

// IEnumerable<long> YieldTime(long start, long interval)
// {
// 	var value = start;
// 	while (value > 0)
// 	{
// 		value += interval;
// 		yield return value;
// 	}
// }

long ts = biggest.id - biggest.i;
long factor = 1;
foreach (var x in desc)
{
	long mod = x.id;
	long value = mod - x.i;
	if (value == mod)
		value = 0;
	while (ts % mod != value)
		ts += factor;
	factor *= mod;
	WriteLine(ts);
}
WriteLine(ts);
