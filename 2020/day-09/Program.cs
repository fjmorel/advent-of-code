using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var nums = System.IO.File.ReadLines("input.txt").Select(x => long.Parse(x)).ToList();
long invalid = 0;
var preamble = 25;

Parallel.ForEach(nums.Skip(preamble), (item, state, i) =>
{
	var range = nums.Skip((int)i).Take(preamble);

	if (!range.SelectMany(x => range.Where(y => y != x).Select(y => x + y)).Contains(item))
	{
		invalid = item;
		state.Stop();
	}
});
WriteLine(invalid);

Parallel.ForEach(nums, (item, state, i) =>
{
	if (item == invalid)
		return;
	long sum = 0;
	int j = 0;
	while (sum < invalid && (i + j) < nums.Count)
	{
		sum += nums[(int)i + j];
		j++;
	}
	if (sum == invalid)
	{
		var range = nums.Skip((int)i).Take(j).ToList();
		WriteLine(range.Min() + range.Max());
		state.Stop();
	}
});
