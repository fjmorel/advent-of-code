﻿using System;
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
	var oneNode = cupList.Find(1);
	return (long)oneNode.Next.Value * (long)oneNode.Next.Next.Value;
}

LinkedList<int> RunSimulation(List<int> startingCups, int iterations, int max)
{
	var cups = new LinkedList<int>(startingCups.Concat(Enumerable.Range(1, max).Skip(9)));

	var lookup = new Dictionary<int, LinkedListNode<int>>();
	var adding = cups.First;
	do
	{
		lookup[adding.Value] = adding;
		adding = adding.Next;
	} while (adding != null);

	for (var i = 0; i < iterations; i++)
	{
		var first = cups.First;
		var current = first.Value;
		var removed = new List<LinkedListNode<int>>()
				{
						first.Next,
						first.Next.Next,
						first.Next.Next.Next,
				};
		foreach (var node in removed)
			cups.Remove(node);

		var destinationValue = current - 1;
		while (true)
		{
			if (destinationValue < 1)
				destinationValue = max;
			else if (removed.Any(x => x.Value == destinationValue))
				destinationValue--;
			else
			{
				var destination = lookup[destinationValue];
				foreach (var node in removed)
				{
					cups.AddAfter(destination, node);
					destination = node;
				}
				cups.RemoveFirst();
				cups.AddLast(first);
				break;
			}
		}

	}

	return cups;
}