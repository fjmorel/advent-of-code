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
		var pieces = line.Split(" = ");
		if (pieces[0] == "mask")
		{
			orMask = 0;
			andMask = 0;
			var currentBit = 1L;
			foreach (var bit in pieces[1].Reverse())
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
			var val = long.Parse(pieces[1]);
			foreach (var address in masks.Select(x => (baseAddress & x.andMask) | x.orMask))
				memory[address] = val;
		}
	}

	return memory.Sum(x => x.Value);
}

static List<(long andMask, long orMask)> GeneratePlainMasks(string baseMask)
{
	var floatingBits = baseMask.Select((ch, i) => (ch, i)).Where(x => x.ch == 'X').Select(x => x.i).ToArray();
	var floatingCount = floatingBits.Count();
	return Enumerable.Range(0, (int)Math.Pow(2, floatingCount)).Select(entryIndex =>
	{
		var newMask = baseMask.ToCharArray();
		for (var i = 0; i < floatingCount; i++)
		{
			var floatingBit = floatingBits[i];
			var pow = (int)Math.Pow(2, i + 1);
			var mod = entryIndex % pow;
			newMask[floatingBit] = mod >= pow / 2 ? '1' : '2';
		}
		return AsMasks(newMask);
	}).ToList();
}

static (long andMask, long orMask) AsMasks(char[] mask)
{
	long andMask = 0, orMask = 0, currentBit = 1;
	foreach (var bit in mask.Reverse())
	{
		// If 0, set AND mask to 1 in order to get the 0/1 from value
		if (bit == '0')
			andMask |= currentBit;
		// If 1, set OR mask to 1 to make sure we put in 1
		else if (bit == '1')
			orMask |= currentBit;
		// If 2, always set 0 (AND mask applied first covers this)
		currentBit <<= 1;
	}
	return (andMask, orMask);
}
