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
			WriteLine($"AND: {ToBinary(andMask)}");
			WriteLine($"OR:  {ToBinary(orMask)}");
		}
		else
		{
			var val = long.Parse(pieces[1]);
			WriteLine($"VAL: {ToBinary(val)}");
			WriteLine($"ADJ: {ToBinary((val & andMask) | orMask)}");
			memory[long.Parse(pieces[0][4..^1])] = (long.Parse(pieces[1]) & andMask) | orMask;
		}
	}

	return memory.Sum(x => x.Value);
}

long Part2(string[] read)
{
	var masks = new List<char[]>();
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
			var baseAddress = ToBinary(int.Parse(pieces[0][4..^1]));
			foreach (var address in masks.Select(x => ApplyMaskToAddress(baseAddress, x)))
				memory[address] = int.Parse(pieces[1]);
		}
	}

	return memory.Sum(x => x.Value);
}

static long ApplyMaskToAddress(string baseAddress, char[] mask)
	=> FromBinary(baseAddress.Zip(mask).Select(x => x.Second switch
	{
		'0' => x.First,
		'1' => '1',
		'2' => '0',
		_ => throw new NotSupportedException(),
	}).ToArray().AsSpan().ToString());

static List<char[]> GeneratePlainMasks(string mask)
{
	var floating = mask.Select((ch, i) => (ch, i)).Where(x => x.ch == 'X').Select(x => x.i).ToArray();
	var count = floating.Count();
	return Enumerable.Range(0, (int)Math.Pow(2, count)).Select(entryIndex =>
	{
		var newMask = mask.ToCharArray();
		for (var i = 0; i < count; i++)
		{
			var floatVal = floating[i];
			var pow = (int)Math.Pow(2, i + 1);
			var mod = entryIndex % pow;
			newMask[floatVal] = mod >= pow / 2 ? '1' : '2';
		}
		return newMask;
	}).ToList();
}

static long FromBinary(string value) => Convert.ToInt64(value, 2);
static string ToBinary(long value) => Convert.ToString(value, 2).PadLeft(36, '0');
