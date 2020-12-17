using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var list = System.IO.File.ReadAllLines("input.txt");

var timer = Stopwatch.StartNew();
WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	var write = new Dictionary<Cube, bool>();
	for (var i = 0; i < list.Length; i++)
	{
		for (var j = 0; j < list[i].Length; j++)
		{
			write[new Cube(i - 1, j - 1, 0)] = list[i][j] == '#';
		}
	}

	for (var cycle = 1; cycle <= 6; cycle++)
	{
		var read = new Dictionary<Cube, bool>(write);
		write.Clear();
		var points = read.Select(x => x.Key).SelectMany(x => x.GetAdjacent(true)).ToHashSet();
		foreach (var point in points)
		{
			var adjActive = point.GetAdjacent().Select(x => read.GetValueOrDefault(x, false)).Count(x => x);
			var currentlyActive = read.GetValueOrDefault(point, false);
			var newState = adjActive == 3 || (adjActive == 2 && currentlyActive);
			write[point] = newState;
		}
		var active = write.Count(x => x.Value);
	}

	return write.Count(x => x.Value);
}

long Part2()
{
	var write = new Dictionary<Hypercube, bool>();
	for (var i = 0; i < list.Length; i++)
	{
		for (var j = 0; j < list[i].Length; j++)
		{
			write[new Hypercube(0, i - 1, j - 1, 0)] = list[i][j] == '#';
		}
	}

	for (var cycle = 1; cycle <= 6; cycle++)
	{
		var read = new Dictionary<Hypercube, bool>(write);
		write.Clear();
		var points = read.Select(x => x.Key).SelectMany(x => x.GetAdjacent(true)).ToHashSet();
		foreach (var point in points)
		{
			var adjActive = point.GetAdjacent().Select(x => read.GetValueOrDefault(x, false)).Count(x => x);
			var currentlyActive = read.GetValueOrDefault(point, false);
			var newState = adjActive == 3 || (adjActive == 2 && currentlyActive);
			write[point] = newState;
		}
		var active = write.Count(x => x.Value);
	}

	return write.Count(x => x.Value);
}

record Cube(int x, int y, int z)
{
	public List<Cube> GetAdjacent(bool includeItself = false)
	{
		var points = new List<Cube>();
		foreach (var i in Enumerable.Range(-1, 3))
			foreach (var j in Enumerable.Range(-1, 3))
				foreach (var k in Enumerable.Range(-1, 3))
					points.Add(new Cube(this.x + i, this.y + j, this.z + k));

		if (!includeItself)
		{
			var toRemove = points.FirstOrDefault(x => x.Equals(this));
			points.Remove(toRemove);
		}
		return points;
	}
}

record Hypercube(int w, int x, int y, int z)
{
	public List<Hypercube> GetAdjacent(bool includeItself = false)
	{
		var points = new List<Hypercube>();
		foreach (var h in Enumerable.Range(-1, 3))
			foreach (var i in Enumerable.Range(-1, 3))
				foreach (var j in Enumerable.Range(-1, 3))
					foreach (var k in Enumerable.Range(-1, 3))
						points.Add(new Hypercube(this.w + h, this.x + i, this.y + j, this.z + k));

		if (!includeItself)
		{
			var toRemove = points.FirstOrDefault(x => x.Equals(this));
			points.Remove(toRemove);
		}
		return points;
	}
}