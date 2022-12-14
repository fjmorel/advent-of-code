namespace Puzzles2022.Day13;

public record Solution(List<Solution.Pair> _pairs) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines.Chunk(3).Select(ParsePair).ToList());

    private static Pair ParsePair(string[] lines)
    {
        ParsePacket(lines[0], out var leftNested);
        ParsePacket(lines[1], out var rightNested);
        Packet left = new() { Nested = leftNested }, right = new() { Nested = rightNested };
        return new(left, right);
    }

    private static int ParsePacket(ReadOnlySpan<char> line, out List<Packet> nested)
    {
        var packets = new List<Packet>();
        var current = new Packet();
        for (var i = 1; i < line.Length; i++)
        {
            if (line[i] == '[')
            {
                var parsed = ParsePacket(line[i..], out var parsedNested);
                i += parsed;
                current.Nested = parsedNested;
            }

            else if (line[i] == ']')
            {
                if (current.Value is not null || current.Nested is not null)
                    packets.Add(current);
                nested = packets;
                return i;
            }

            else if (line[i] == ',')
            {
                packets.Add(current);
                current = new();
            }

            else if (line[i] == ' ')
                continue;

            else
            {
                var start = i;
                while (char.IsDigit(line[i]))
                    i++;
                var value = int.Parse(line[start..i]);
                i--;
                current.Value = value;
            }
        }

        nested = packets;
        return line.Length;
    }

    public async ValueTask<long> GetPart1()
    {
        var total = 0L;
        for (var i = 0; i < _pairs.Count; i++)
        {
            if (LessThan(_pairs[i].left, _pairs[i].right) is true)
                total += i + 1;
        }

        return total;
    }

    public async ValueTask<long> GetPart2()
    {
        var allPackets = _pairs.SelectMany(x => new[] { x.left, x.right }).ToList();
        var newPackets = new Packet[]
        {
            new() { Nested = new() { new() { Value = 2 } } },
            new() { Nested = new() { new() { Value = 6 } } },
        };
        allPackets.AddRange(newPackets);

        allPackets.Sort((a, b) => LessThan(a, b) == true ? -1 : 1);
        allPackets.Insert(0, new Packet());

        return allPackets.IndexOf(newPackets[0]) * allPackets.IndexOf(newPackets[1]);
    }

    public class PacketComparer : IComparer<Packet>
    {
        public int Compare(Packet? x, Packet? y)
        {
            throw new NotImplementedException();
        }
    }


    public static bool? LessThan(Packet left, Packet right)
    {
        if (left.Value is int leftValue && right.Value is int rightValue)
        {
            if (leftValue < rightValue)
                return true;
            if (leftValue > rightValue)
                return false;
            return null;
        }

        if (left.Value is null && right.Value is null)
            return NestedLessThan(left.Nested!, right.Nested!);

        if (left.Value != null && right.Nested is not null)
        {
            var fakeNested = new List<Packet>() { new() { Value = left.Value } };
            return NestedLessThan(fakeNested, right.Nested);
        }

        if (left.Nested is not null && right.Value != null)
        {
            var fakeNested = new List<Packet>() { new() { Value = right.Value } };
            return NestedLessThan(left.Nested, fakeNested);
        }

        return null;
    }

    public static bool? NestedLessThan(List<Packet> left, List<Packet> right)
    {
        var common = Math.Min(left.Count, right.Count);
        for (var i = 0; i < common; i++)
        {
            var result = LessThan(left[i], right[i]);
            if (result is bool sorted)
                return sorted;
        }

        if (left.Count < right.Count)
            return true;
        if (left.Count > right.Count)
            return false;

        return null;
    }

    public record Pair(Packet left, Packet right)
    {
        public override string ToString() => left + Environment.NewLine + right;
    }

    public record Packet
    {
        public int? Value { get; set; }
        public List<Packet>? Nested { get; set; }

        public override string ToString()
        {
            if (Value is int value)
                return value.ToString();
            return "[" + string.Join(", ", Nested!.Select(x => x.ToString())) + "]";
        }
    };
}
