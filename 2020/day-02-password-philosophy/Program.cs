using System;
using System.Linq;

var lines = System.IO.File.ReadLines("input.txt");
var oldValid = 0;
var reallyValid = 0;

foreach (var line in lines)
{
	var pieces = line.Split(' ');
	var nums = pieces[0].Split('-');
	var a = int.Parse(nums[0]);
	var b = int.Parse(nums[1]);
	var letter = pieces[1][0];
	var password = pieces[2];

	var count = password.Count(x => x == letter);
	if (count <= b && count >= a)
		oldValid++;

	var first = password[a - 1];
	var second = password[b - 1];
	if (first != second && (first == letter || second == letter))
		reallyValid++;
}

Console.WriteLine(oldValid);
Console.WriteLine(reallyValid);
