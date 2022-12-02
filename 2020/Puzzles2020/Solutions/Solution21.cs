namespace Puzzles2020.Solutions;

public record Solution21(List<Solution21.Food> foods, Dictionary<string, string?> allergens) : ISolution<Solution21>
{
    public static Solution21 Init(string[] lines)
    {
        var foods = lines
            .Select(x => x.Split(" (contains "))
            .Select(x => new Food(x[0].Split(' ').ToHashSet(), x[1][0..^1].Split(", ").ToHashSet()))
            .ToList();
        var allergens = foods.SelectMany(x => x.Allergens).Distinct().ToDictionary(x => x, _ => (string?)null);
        return new(foods, allergens);
    }

    public async ValueTask<long> GetPart1()
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

    public async ValueTask<string> GetPart2String()
    {
        return string.Join(',', allergens.OrderBy(x => x.Key).Select(x => x.Value));
    }

    public record Food(HashSet<string> Ingredients, HashSet<string> Allergens);
}
