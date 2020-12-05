using System;
using System.IO;
using System.Linq;


// #region Part 1

// var lines = File.ReadAllLines("input.txt");
// var nums = lines.Select(x => int.Parse(x)).ToList();

// for (var i = 0; i < nums.Count; i++)
// {
// 	for (var j = 0; j < nums.Count; j++)
// 	{
// 		if (nums[i] + nums[j] == 2020 && i != j)
// 		{
// 			Console.WriteLine(nums[i] * nums[j]);
// 		}
// 	}
// }

// #endregion

#region Part 2

var lines = File.ReadAllLines("input.txt");
var nums = lines.Select(x => int.Parse(x)).ToList();

for (var i = 0; i < nums.Count; i++)
{
	for (var j = 0; j < nums.Count; j++)
	{
		for (var k = 0; k < nums.Count; k++)
		{
			if (nums[i] + nums[j] + nums[k] == 2020 && i != j && i != k && j != k)
			{
				Console.WriteLine(nums[i] * nums[j] * nums[k]);
			}
		}
	}
}

#endregion