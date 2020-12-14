using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var read = System.IO.File.ReadAllLines("input.txt");

Part1(read);
WriteLine("");
Part2(read);

void Part1(string[] read)
{
	var memory = new Dictionary<long, string>();
	ReadOnlySpan<char> mask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
	foreach (var line in read)
	{
		var pieces = line.Split(" = ");
		if (pieces[0] == "mask")
		{
			mask = pieces[1];//mask.ToArray().Zip(pieces[1]).Select(x => x.Second == 'X' ? x.First : x.Second).ToArray();
		}
		else
		{
			var index = int.Parse(pieces[0][4..^1]);
			var value = int.Parse(pieces[1]);
			var str = Convert.ToString(value, 2).PadLeft(mask.Length, '0');
			memory[index] = new string(str.Zip(mask.ToArray()).Select(x => x.Second != 'X' ? x.Second : x.First).ToArray());
		}
	}

	WriteLine(memory.Select(x => Convert.ToInt64(x.Value.ToString(), 2)).Sum());
}

void Part2(string[] read)
{
	var mask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
	var zero = string.Intern(Convert.ToString(0, 2).PadLeft(mask.Length, '0'));
	var memory = new Dictionary<long, long>();
	foreach (var line in read)
	{
		var pieces = line.Split(" = ");
		if (pieces[0] == "mask")
		{
			mask = pieces[1];//mask.ToArray().Zip(pieces[1]).Select(x => x.Second == 'X' ? x.First : x.Second).ToArray();
		}
		else
		{
			var index = int.Parse(pieces[0][4..^1]);
			var value = int.Parse(pieces[1]);
			var valueStr = Convert.ToString(value, 2).PadLeft(mask.Length, '0');
			var baseAddress = Convert.ToString(index, 2).PadLeft(mask.Length, '0');
			var masks = GeneratePlainMasks(mask);
			var addresses = masks.Select(x => ApplyMaskToAddress(baseAddress, x)).ToList();
			foreach (var address in addresses)
				memory[address] = value;

			//memory[index] = new string(str.Zip(mask.ToArray()).Select(x => x.Second == '0' ? x.First : x.Second : x.First).ToArray());
		}
	}

	WriteLine(memory.Sum(x => x.Value));
}

long ApplyMaskToAddress(string baseAddress, string mask)
{
	var str = baseAddress.Zip(mask).Select(x => x.Second switch
	{
		'0' => x.First,
		'1' => '1',
		'2' => '0',
		_ => throw new NotSupportedException(),
	}).ToArray().AsSpan().ToString();
	return Convert.ToInt64(str, 2);
}

List<string> GeneratePlainMasks(string mask)
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
		return newMask.AsSpan().ToString();
	}).ToList();
}