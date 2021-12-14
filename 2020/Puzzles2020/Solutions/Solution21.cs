namespace Puzzles2020.Solutions;

public class Solution21 : ISolution
{
    private readonly List<Food> foods;
    private readonly Dictionary<string, string?> allergens;

    public Solution21(string[] lines)
    {
        foods = lines
            .Select(x => x.Split(" (contains "))
            .Select(x => new Food(x[0].Split(' ').ToHashSet(), x[1][0..^1].Split(", ").ToHashSet()))
            .ToList();
        allergens = foods.SelectMany(x => x.Allergens).Distinct().ToDictionary(x => x, _ => (string?)null);
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

    private record Food(HashSet<string> Ingredients, HashSet<string> Allergens);
}
