using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var read = System.IO.File.ReadAllLines("input.txt")[0].Split(',').Select(x => int.Parse(x)).ToList();

var timer = Stopwatch.StartNew();

var i = read.Count;
//var limit = 2_020;
var limit = 30_000_000;
while (i < limit)
{
	var last = read.Last();
	read.RemoveAt(read.Count - 1);
	var pos = read.LastIndexOf(last);
	read.Add(last);
	if (pos > -1)
		read.Add(read.Count - pos - 1);
	else
		read.Add(0);
	i++;
}
// foreach (var num in read)
// 	WriteLine(num);
WriteLine($"{read.Last()} :: {timer.Elapsed}");
timer.Restart();
// WriteLine($"{Part2(read)} :: {timer.Elapsed}");
// timer.Stop();

// static long Part1(string[] read)
// {
// 	return 0;
// }

// static long Part2(string[] read)
// {
// 	return 0;
// }
