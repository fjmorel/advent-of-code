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
	var items = foods.Select(x => new Food(new HashSet<string>(x.Ingredients), new HashSet<string>(x.Allergens))).ToList();


	while (allergens.Values.Any(x => x == null))
	{
		foreach (var allergen in allergens.Keys)
		{
			if (allergens[allergen] != null)
				continue;

			var definitelyContain = items.Where(x => x.Allergens.Contains(allergen)).ToList();
			if (definitelyContain.Count == 1)
			{
				if (definitelyContain[0].Ingredients.Count == 1)
				{
					var ingredient = definitelyContain[0].Ingredients.First();
					allergens[allergen] = ingredient;
					foreach (var item in items)
					{
						if (item.Ingredients.Contains(ingredient))
						{
							item.Allergens.Add(allergen);
							item.Ingredients.Remove(ingredient);
						}
					}
				}
			}
			else
			{
				for (var i = 0; i < definitelyContain.Count; i++)
				{
					var union = new HashSet<string>(definitelyContain[i].Ingredients);
					foreach (var item in definitelyContain)
						union.IntersectWith(item.Ingredients);

					if (union.Count == 1)
					{
						var ingredient = union.First();
						allergens[allergen] = ingredient;
						foreach (var item in items)
						{
							if (item.Ingredients.Contains(ingredient))
							{
								item.Allergens.Add(allergen);
								item.Ingredients.Remove(ingredient);
							}
						}
					}
				}
			}
		}
	}

	return items.SelectMany(x => x.Ingredients).Count();
}

string Part2()
{
	return string.Join(',', allergens.OrderBy(x => x.Key).Select(x => x.Value));
}

record Food(HashSet<string> Ingredients, HashSet<string> Allergens)
{
	public bool Tainted => Allergens.Any();
}
