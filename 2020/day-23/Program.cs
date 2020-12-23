using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
	var pointers = RunSimulation(startingCups, 100, 9);
	var result = new StringBuilder();
	var current = pointers[1];
	while (current != 1)
	{
		result.Append(current);
		current = pointers[current];
	}
	return result.ToString();
}

long Part2()
{
	var pointers = RunSimulation(startingCups, 10_000_000, 1_000_000);
	return (long)pointers[1] * (long)pointers[pointers[1]];
}

int[] RunSimulation(List<int> startingCups, int iterations, int max)
{
	WriteLine($"{timer.Elapsed} :: create lookup");

	var pointers = new int[max + 1];

	// initialize 10-max in numerical order
	for (var i = 10; i <= max; i++)
		pointers[i] = i + 1;
	// initialize 1-9 in input order
	for (var i = 0; i < startingCups.Count - 1; i++)
		pointers[startingCups[i]] = startingCups[i + 1];

	var current = startingCups[0];

	// If only 9 cups, make last input point to first input
	if (max == startingCups.Count)
		pointers[startingCups.Last()] = current;
	// Otherwise last input => 10 && max >= first input
	else
	{
		pointers[startingCups.Last()] = 10;
		pointers[max] = current;
	}

	var removed = new int[3];

	WriteLine($"{timer.Elapsed} :: iterate");
	for (var i = 0; i < iterations; i++)
	{
		removed[0] = pointers[current];
		removed[1] = pointers[removed[0]];
		removed[2] = pointers[removed[1]];
		pointers[current] = pointers[removed[2]];

		var destination = current - 1;
		while (destination == 0 || removed.Contains(destination))
		{
			if (destination == 0)
				destination = max;
			else
				destination--;
		}
		pointers[removed[2]] = pointers[destination];
		pointers[destination] = removed[0];

		current = pointers[current];
	}

	WriteLine($"{timer.Elapsed} :: done");
	return pointers;
}
