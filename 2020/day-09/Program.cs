using System;
using System.Collections.Generic;
using System.Linq;

var nums = System.IO.File.ReadLines("input.txt").Select(x => long.Parse(x)).ToList();
long invalid = 0;
var preamble = 25;

for (var i = preamble; i < nums.Count; i++)
{
	var range = nums.Skip(i - preamble).Take(preamble).ToArray();
	invalid = nums[i];

	var sums = range.SelectMany(x => range.Where(y => y != x).Select(y => x + y)).ToList();
	if (!sums.Contains(invalid))
	{
		Console.WriteLine(invalid);
		break;
	}
}

for (var i = 0; i < nums.Count; i++)
{
	if (nums[i] == invalid)
		continue;
	long sum = 0;
	int j = 0;
	while (sum < invalid && (i + j) < nums.Count)
	{
		sum += nums[i + j];
		j++;
	}
	if (sum == invalid)
	{
		var range = nums.Skip(i).Take(j).ToList();
		Console.WriteLine(range.Min() + range.Max());
		break;
	}
}
