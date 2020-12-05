using System;
using System.IO;
using System.Linq;

var lines = File.ReadAllLines("input.txt");
var nums = lines.Select(x => int.Parse(x)).ToList();

var two = nums
.SelectMany(x => nums.Where(y => y != x).Select(y => new[] { x, y }))
.Where(list => list.Sum() == 2020 && list.Distinct().Count() == list.Count())
.Select(list => list.Aggregate(1, (acc, x) => acc * x))
.First();

var three = nums
.SelectMany(x => nums.Where(y => y != x).Select(y => new[] { x, y }))
.SelectMany(list => nums.Where(z => !list.Contains(z)).Select(z => new[] { list[0], list[1], z }))
.Where(list => list.Sum() == 2020 && list.Distinct().Count() == list.Count())
.Select(list => list.Aggregate(1, (acc, x) => acc * x))
.First();

Console.WriteLine(two);
Console.WriteLine(three);
