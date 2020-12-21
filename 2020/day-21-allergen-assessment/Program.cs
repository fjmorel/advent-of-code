using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var list = System.IO.File.ReadAllLines("input.txt");
var foods = list
	.Select(x => x.Split(" (contains "))
	.Select(x => new Food(x[0].Split(' ').ToHashSet(), x[1][0..^1].Split(", ").ToHashSet()))
	.ToList();
var allergens = foods.SelectMany(x => x.Allergens).Distinct().ToDictionary(x => x, x => (string)null);

var timer = Stopwatch.StartNew();
WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	while (allergens.Values.Any(x => x == null))
	{
		foreach (var allergen in allergens.Keys)
		{
			if (allergens[allergen] != null)
				continue;

			var definitelyContain = foods.Where(x => x.Allergens.Contains(allergen));
			var union = new HashSet<string>(definitelyContain.First().Ingredients);
			foreach (var item in definitelyContain.Skip(1))
				union.IntersectWith(item.Ingredients);

			if (union.Count == 1)
			{
				allergens[allergen] = union.First();
				foreach (var item in foods)
					item.Ingredients.Remove(union.First());
			}
		}
	}

	return foods.SelectMany(x => x.Ingredients).Count();
}

string Part2()
{
	return string.Join(',', allergens.OrderBy(x => x.Key).Select(x => x.Value));
}

record Food(HashSet<string> Ingredients, HashSet<string> Allergens);
