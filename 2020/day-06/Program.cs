using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

var part1 = 0;
var part2 = 0;

var anyChar = new HashSet<char>();
var byCharCounts = new Dictionary<char, int>();
var group = 0;
ResetGroup();
foreach (var line in System.IO.File.ReadLines("input.txt"))
{
	if (string.IsNullOrEmpty(line))
	{
		part1 += anyChar.Count;
		part2 += byCharCounts.Where(x => x.Value == group).Count();
		ResetGroup();
		group = 0;
	}
	else
	{
		group++;
		foreach (var ch in line)
		{
			anyChar.Add(ch);
			byCharCounts[ch] = byCharCounts[ch] + 1;
		}
	}
}

part1 += anyChar.Count;
part2 += byCharCounts.Where(x => x.Value == group).Count();

Console.WriteLine(part1);
Console.WriteLine(part2);

void ResetGroup()
{
	anyChar.Clear();
	for (var ch = 'a'; ch <= 'z'; ch++)
	{
		byCharCounts[ch] = 0;
	}
}