using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var read = System.IO.File.ReadAllLines("input.txt");

WriteLine(Part1(read));
WriteLine(Part2(read));

long Part1(string[] read)
{
	var memory = new Dictionary<long, char[]>();
	var mask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
	foreach (var line in read)
	{
		var pieces = line.Split(" = ");
		if (pieces[0] == "mask")
		{
			mask = pieces[1];
		}
		else
		{
			var valueStr = ToBinary(int.Parse(pieces[1]));
			memory[int.Parse(pieces[0][4..^1])] = valueStr.Zip(mask).Select(x => x.Second != 'X' ? x.Second : x.First).ToArray();
		}
	}

	return memory.Sum(x => FromBinary(new string(x.Value)));
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
