using System;
using System.Collections.Generic;
using System.Linq;

var lines = System.IO.File.ReadLines("input.txt");
var nums = lines.Select(x => new int[] { int.Parse(x) }).ToList();

Console.WriteLine(FindProduct(2));
Console.WriteLine(FindProduct(3));

long FindProduct(int numValues)
{
	IEnumerable<IEnumerable<int>> combinations = nums;
	for (var i = 2; i <= numValues; i++)
		combinations = combinations.SelectMany(list => nums.Where(z => !list.Contains(z[0])).Select(z => list.Concat(z)));

	return combinations
	.Where(list => list.Sum() == 2020)
	.Select(list => list.Aggregate(1, (acc, x) => acc * x))
	.First();
}
