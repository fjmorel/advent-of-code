using System;
using System.Collections.Generic;
using System.Linq;

var part1 = 0;
var part2 = 0;

var anyAnswered = new HashSet<char>();
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
			anyAnswered.Add(ch);
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
	anyAnswered.Clear();
	for (var ch = 'a'; ch <= 'z'; ch++)
	{
		charAnswers[ch] = 0;
	}
}

void CountGroup()
{
	part1 += anyAnswered.Count;
	part2 += charAnswers.Where(x => x.Value == groupSize).Count();
}
