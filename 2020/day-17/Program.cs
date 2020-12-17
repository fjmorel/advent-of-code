using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var list = System.IO.File.ReadAllLines("input.txt");

var timer = Stopwatch.StartNew();
WriteLine($"{Part1(list)} :: {timer.Elapsed}");
timer.Restart();
WriteLine($"{Part2(list)} :: {timer.Elapsed}");
timer.Stop();

static long Part1(string[] list)
{
	var write = new ConcurrentDictionary<Cube, bool>();
	for (var i = 0; i < list.Length; i++)
		for (var j = 0; j < list[i].Length; j++)
			write[new Cube(i - 1, j - 1, 0)] = list[i][j] == '#';

	for (var cycle = 1; cycle <= 6; cycle++)
	{
		var read = new Dictionary<Cube, bool>(write);
		Parallel.ForEach(read.SelectMany(x => x.Key.GetAdjacent(true)).ToHashSet(), point =>
		{
			var adjActive = point.GetAdjacent().Count(x => read.GetValueOrDefault(x, false));
			write[point] = adjActive == 3 || (adjActive == 2 && read.GetValueOrDefault(point, false));
		});
	}

	return write.Count(x => x.Value);
}

static long Part2(string[] list)
{
	var write = new ConcurrentDictionary<Hypercube, bool>();
	for (var i = 0; i < list.Length; i++)
		for (var j = 0; j < list[i].Length; j++)
			write[new Hypercube(0, i - 1, j - 1, 0)] = list[i][j] == '#';

	for (var cycle = 1; cycle <= 6; cycle++)
	{
		var read = new Dictionary<Hypercube, bool>(write);
		Parallel.ForEach(read.SelectMany(x => x.Key.GetAdjacent(true)).ToHashSet(), point =>
		{
			var adjActive = point.GetAdjacent().Count(x => read.GetValueOrDefault(x, false));
			write[point] = adjActive == 3 || (adjActive == 2 && read.GetValueOrDefault(point, false));
		});
	}

	return write.Count(x => x.Value);
}

record Cube(int x, int y, int z)
{
	public IEnumerable<Cube> GetAdjacent(bool includeItself = false)
	{
		foreach (var i in Enumerable.Range(-1, 3))
			foreach (var j in Enumerable.Range(-1, 3))
				foreach (var k in Enumerable.Range(-1, 3))
					if (!(i == 0 && j == 0 && k == 0 && !includeItself))
						yield return new Cube(this.x + i, this.y + j, this.z + k);
	}
}

record Hypercube(int w, int x, int y, int z)
{
	public IEnumerable<Hypercube> GetAdjacent(bool includeItself = false)
	{
		foreach (var h in Enumerable.Range(-1, 3))
			foreach (var i in Enumerable.Range(-1, 3))
				foreach (var j in Enumerable.Range(-1, 3))
					foreach (var k in Enumerable.Range(-1, 3))
						if (!(h == 0 && i == 0 && j == 0 && k == 0 && !includeItself))
							yield return new Hypercube(this.w + h, this.x + i, this.y + j, this.z + k);
	}
}
