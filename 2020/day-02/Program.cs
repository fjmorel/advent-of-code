using System;
using System.IO;
using System.Linq;


var lines = File.ReadAllLines("input.txt");
var entries = lines.Select(x =>
{
	var pieces = x.Split(' ');
	var nums = pieces[0].Split('-');
	return new Password(int.Parse(nums[0]), int.Parse(nums[1]), pieces[1][0], pieces[2]);
});

#region Part 1

var validCount = 0;
foreach (var entry in entries)
{
	var count = entry.password.Where(x => x == entry.letter).Count();
	if (count <= entry.second && count >= entry.first)
		validCount++;
}

Console.WriteLine(validCount);

#endregion

#region Part 2

var validCount2 = 0;
foreach (var entry in entries)
{
	var first = entry.password.Skip(entry.first - 1).FirstOrDefault();
	var second = entry.password.Skip(entry.second - 1).FirstOrDefault();
	if (first != second && (first == entry.letter || second == entry.letter))
		validCount2++;
}

Console.WriteLine(validCount2);

#endregion

record Password(int first, int second, char letter, string password);