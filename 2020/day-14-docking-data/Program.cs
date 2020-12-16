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

static long Part1(string[] read)
{
	var masks = new Mask(0, 0);
	var memory = new Dictionary<long, long>();
	foreach (var line in read)
	{
		if (line.StartsWith("mask", StringComparison.Ordinal))
		{
			long orMask = 0, andMask = 0, currentBit = 1;
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
			masks = new Mask(andMask, orMask);
		}
		else
		{
			var equalIndex = line.IndexOf('=');
			memory[long.Parse(line[4..(equalIndex - 2)])] = (long.Parse(line[(equalIndex + 1)..]) & masks.and) | masks.or;
		}
	}

	return memory.Sum(x => x.Value);
}

static long Part2(string[] read)
{
	var masks = new Mask[0];
	var memory = new Dictionary<long, long>();
	foreach (var line in read)
	{
		if (line.StartsWith("mask", StringComparison.Ordinal))
		{
			var baseMask = line[7..].Reverse().ToArray();
			masks = new Mask[1 << baseMask.Count(x => x == 'X')];
			for (var entryIndex = 0; entryIndex < masks.Length; entryIndex++)
			{
				long andMask = 0, orMask = 0, currentBit = 1, xFactor = 1;
				foreach (var bit in baseMask)
				{
					// If X, set based on current iteration of float
					if (bit == 'X')
					{
						xFactor *= 2;
						// half the time, set it to 1, half the time leave it as 0
						if (entryIndex % xFactor >= xFactor / 2)
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
				masks[entryIndex] = new Mask(andMask, orMask);
			};
		}
		else
		{
			var equalIndex = line.IndexOf('=');
			var baseAddress = long.Parse(line[4..(equalIndex - 2)]);
			var value = long.Parse(line[(equalIndex + 1)..]);
			foreach (var x in masks)
				memory[(baseAddress & x.and) | x.or] = value;
		}
	}

	return memory.Sum(x => x.Value);
}

record Mask(long and, long or);
