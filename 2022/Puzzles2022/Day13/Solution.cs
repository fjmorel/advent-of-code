using System.Text.Json.Nodes;

namespace Puzzles2022.Day13;

public record Solution(List<(JsonNode left, JsonNode right)> _pairs) : ISolution<Solution>
{
    public static Solution Init(string[] lines) =>
        new(lines.Chunk(3).Select(chunk => (JsonNode.Parse(chunk[0])!, JsonNode.Parse(chunk[1])!)).ToList());


    public async ValueTask<long> GetPart1()
    {
        var total = 0L;
        for (var i = 0; i < _pairs.Count; i++)
        {
            if (LessThan(_pairs[i].left, _pairs[i].right) < 0)
                total += i + 1;
        }

        return total;
    }

    public async ValueTask<long> GetPart2()
    {
        var allPackets = _pairs.SelectMany(x => new[] { x.left, x.right }).ToList();
        var newPackets = new JsonArray[]
        {
            new(new JsonArray(JsonValue.Create(2))),
            new(new JsonArray(JsonValue.Create(6))),
        };
        allPackets.AddRange(newPackets);

        allPackets.Sort(LessThan);
        allPackets.Insert(0, new JsonArray());// to offset index by 1

        return allPackets.IndexOf(newPackets[0]) * allPackets.IndexOf(newPackets[1]);
    }

    public static int LessThan(JsonNode left, JsonNode right)
    {
        return left switch
        {
            JsonValue when right is JsonValue => left.GetValue<int>() - right.GetValue<int>(),
            JsonArray lArray when right is JsonArray rArray => NestedLessThan(lArray, rArray),
            JsonValue when right is JsonArray rArray => NestedLessThan(new JsonArray(JsonValue.Create(left.GetValue<int>())), rArray),
            JsonArray lArray when right is JsonValue => NestedLessThan(lArray, new JsonArray(JsonValue.Create(right.GetValue<int>()))),
            _ => 0,
        };
    }

    public static int NestedLessThan(JsonArray left, JsonArray right)
    {
        var common = Math.Min(left.Count, right.Count);
        for (var i = 0; i < common; i++)
        {
            var result = LessThan(left[i]!, right[i]!);
            if (result != 0)
                return result;
        }

        return left.Count - right.Count;
    }
}
