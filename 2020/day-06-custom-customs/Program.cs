using System;
using System.Collections.Generic;
using System.Linq;

var part1 = 0;
var part2 = 0;

var data = string.Join('\n', System.IO.File.ReadAllLines("input.txt"))
	.Split("\n\n")
	.Select(x => x.Split('\n'));
foreach (var group in data)
{
	var charAnswers = new Dictionary<char, int>();
	foreach (var line in group)
		foreach (var ch in line)
			charAnswers[ch] = charAnswers.GetValueOrDefault(ch) + 1;

	part1 += charAnswers.Count();
	part2 += charAnswers.Count(x => x.Value == group.Count());
}

Console.WriteLine(part1);
Console.WriteLine(part2);
