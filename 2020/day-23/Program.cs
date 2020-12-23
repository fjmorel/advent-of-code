using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var list = System.IO.File.ReadAllLines("input.txt");

var startingCups = list[0].Select(ch => int.Parse(ch.ToString())).ToList();

var timer = Stopwatch.StartNew();
WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

string Part1()
{
	var cupList = RunSimulation(startingCups, 100, 9);
	var result = string.Join("", cupList.Select(x => x.ToString()));
	return string.Join("", result.Split('1').Reverse());
}

long Part2()
{
	var cupList = RunSimulation(startingCups, 10_000_000, 1_000_000);
	var star1 = cupList.Find(1).Next;
	return (long)star1.Value * (long)star1.Next.Value;
}

LinkedList<int> RunSimulation(List<int> startingCups, int iterations, int max)
{
	WriteLine($"{timer.Elapsed} :: start");
	var cups = new LinkedList<int>(startingCups.Concat(Enumerable.Range(1, max).Skip(9)));

	WriteLine($"{timer.Elapsed} :: create lookup");
	var lookup = new List<LinkedListNode<int>>(max) { null };
	for(var i = 1; i <= 9; i++)
		lookup.Add(cups.Find(i));

	var adding = cups.Find(10);
	while(adding != null)
	{
		lookup.Add(adding);
		adding = adding.Next;
	}

	var removed = new LinkedListNode<int>[3];

	WriteLine($"{timer.Elapsed} :: iterate");
	for (var i = 0; i < iterations; i++)
	{
		var first = cups.First;

		removed[2] = first.Next;
		removed[1] = removed[2].Next;
		removed[0] = removed[1].Next;
		foreach (var node in removed)
			cups.Remove(node);

		var destination = lookup[first.Value - 1];
		while(destination == null || removed.Contains(destination))
		{
			if(destination == null)
				destination = lookup[max];
			else
				destination = lookup[destination.Value - 1];
		}
		foreach (var node in removed)
			cups.AddAfter(destination, node);

		cups.Remove(first);
		cups.AddLast(first);
	}

	WriteLine($"{timer.Elapsed} :: done");
	return cups;
}
