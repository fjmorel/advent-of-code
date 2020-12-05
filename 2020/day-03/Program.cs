using System;
using System.Linq;

var lines = System.IO.File.ReadAllLines("input.txt");
var width = lines[0].Length;

Console.WriteLine(new[]
{
	HowManyHits(1, 1),
	HowManyHits(3, 1),
	HowManyHits(5, 1),
	HowManyHits(7, 1),
	HowManyHits(1, 2),
}.Aggregate((long)1, (acc, x) => acc * x));

long HowManyHits(int right, int down)
{
	long treesHit = 0;
	for (var i = 0; (i * down) < lines.Length; i++)
	{
		var symbol = lines[i * down][(i * right) % width];
		if (symbol == '#')
			treesHit++;
	}
	Console.WriteLine(treesHit);
	return treesHit;
}
