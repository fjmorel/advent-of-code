using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var read = System.IO.File.ReadAllLines("input.txt");
var timer = Stopwatch.StartNew();
WriteLine($"{Part1(read)} :: {timer.Elapsed}");
timer.Restart();
WriteLine($"{Part2(read)} :: {timer.Elapsed}");
timer.Stop();

long Part1(string[] read)
{
	long orMask = 0L, andMask = 0L;
	var memory = new Dictionary<long, long>();
	foreach (var line in read)
	{
		if (line.StartsWith("mask", StringComparison.Ordinal))
		{
			orMask = 0;
			andMask = 0;
			var currentBit = 1L;
			foreach (var bit in line[7..].Reverse())
			{
				// If X, set AND mask to 1 in order to get the 0/1 from value
				if (bit == 'X')
					andMask |= currentBit;
				// If 1, set OR mask to 1 to make sure we put in 1
				else if (bit == '1')
					orMask |= currentBit;
				// If 0, always set 0 (AND mask applied first covers this)
				currentBit <<= 1;
			}
		}
		else
		{
			var pieces = line.Split(" = ");
			memory[long.Parse(pieces[0][4..^1])] = (long.Parse(pieces[1]) & andMask) | orMask;
		}
	}

	return memory.Sum(x => x.Value);
}

long Part2(string[] read)
{
	var masks = new List<(long andMask, long orMask)>();
	var memory = new Dictionary<long, long>();
	foreach (var line in read)
	{
		var pieces = line.Split(" = ");
		if (pieces[0] == "mask")
		{
			masks = GeneratePlainMasks(pieces[1]);
		}
		else
		{
			var baseAddress = long.Parse(pieces[0][4..^1]);
			var value = long.Parse(pieces[1]);
			foreach (var address in masks.Select(x => (baseAddress & x.andMask) | x.orMask))
				memory[address] = value;
		}
	}

	return memory.Sum(x => x.Value);
}

static List<(long andMask, long orMask)> GeneratePlainMasks(string baseMask)
{
	return Enumerable.Range(1, 1 << baseMask.Count(x => x == 'X')).Select(entryIndex =>
	{
		long andMask = 0, orMask = 0, currentBit = 1;
		long xFactor = 1;
		foreach (var bit in baseMask.Reverse())
		{
			// If X, set based on current iteration of float
			if (bit == 'X')
			{
				xFactor *= 2;
				// half the time, set it to 1, half the time leave it as 0
				if ((entryIndex - 1) % xFactor >= xFactor / 2)
				{
					orMask |= currentBit;
				}
			}
			// If 1, set OR mask to 1 to make sure we put in 1
			else if (bit == '1')
				orMask |= currentBit;
			// If 0, always set AND mask to keep original value
			else if (bit == '0')
				andMask |= currentBit;
			currentBit <<= 1;
		}
		return (andMask, orMask);
	}).ToList();
}
