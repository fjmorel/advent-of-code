using System;
using System.Collections.Generic;
using System.Linq;

var part1 = 0;
var part2 = 0;

var charAnswers = new Dictionary<char, int>();
var groupSize = 0;
ResetGroup();
foreach (var line in System.IO.File.ReadLines("input.txt"))
{
	if (string.IsNullOrEmpty(line))
	{
		CountGroup();
		ResetGroup();
	}
	else
	{
		groupSize++;
		foreach (var ch in line)
		{
			charAnswers[ch] = charAnswers[ch] + 1;
		}
	}
}
CountGroup();

Console.WriteLine(part1);
Console.WriteLine(part2);

void ResetGroup()
{
	groupSize = 0;
	for (var ch = 'a'; ch <= 'z'; ch++)
	{
		charAnswers[ch] = 0;
	}
}

void CountGroup()
{
	part1 += charAnswers.Count(x => x.Value > 0);
	part2 += charAnswers.Count(x => x.Value == groupSize);
}
