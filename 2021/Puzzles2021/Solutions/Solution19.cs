namespace Puzzles2021.Solutions;

public class Solution19 : ISolution
{
    private readonly List<Scanner> _scanners;

    public Solution19(string[] lines)
    {
        _scanners = ParseInput(lines).ToList();
    }

    public async ValueTask<long> GetPart1()
    {
        _scanners[0].Adjust(default, pt => pt);
        var located = new List<Scanner>() { _scanners[0] };
        var todo = new Queue<Scanner>(_scanners.Skip(1));
        while (todo.Any())
        {
            var scanner = todo.Dequeue();
            foreach (var origin in located.Reverse<Scanner>())
            {
                var found = TryFindOffset3d(origin, scanner);
                if (found)
                    break;
            }

            if (scanner.Position == null)
                todo.Enqueue(scanner);
            else
                located.Add(scanner);
        }

        var beacons = _scanners.SelectMany(x => x.adjusted).ToHashSet();
        // 611 too high
        return beacons.Count;
    }

    public async ValueTask<long> GetPart2()
    {
        if (_scanners[0].Position == null)
            await GetPart1();

        var positions = _scanners.Select(x => x.Position.Value).ToList();
        var pairs = positions.SelectMany(a => positions.Select(b => (a, b))).ToList();
        return pairs.Max(tuple => (tuple.b - tuple.a).GetMagnitude());
    }

    public bool TryFindOffset3d(Scanner origin, Scanner other)
    {
        if (other.Position != null)
            return true;

        HashSet<Match<Point3d>> matches = new();
        var originPairs = origin.GetAdjustedPairs().ToList();
        var otherPairs = other.GetPairs().ToList();
        var rotationFunctions = GetRotations().ToList();

        // For every origin beacon and other beacon,
        foreach (var (originA, originB) in originPairs)
        foreach (var (otherA, otherB) in otherPairs)
        {
            var offset = originB - originA;
            // for every rotation of other, find all the matches with origin
            for (var r = 0; r < rotationFunctions.Count; r++)
            {
                var func = rotationFunctions[r];
                if (func(otherB) - func(otherA) == offset)
                {
                    matches.Add(new(r, originA, otherA));
                    matches.Add(new(r, originB, otherB));
                }
            }
        }

        var grouped = matches
            .GroupBy(x => x.rotation)
            .Select(x => new { Rotation = x.Key, Match = x.First(), Count = x.Count() })
            .ToList();
        var group = grouped.FirstOrDefault(x => x.Count == 12);

        if (group != null)
        {
            var match = group.Match;
            // don't include origin.Position!.Value. It's already implicit in the adjusted points
            var position = match.origin - rotationFunctions[match.rotation](match.other);
            other.Adjust(position, rotationFunctions[match.rotation]);
            return true;
        }

        return false;
    }

    public record Match<T>(int rotation, T origin, T other);

    public static IEnumerable<Scanner> ParseInput(string[] lines)
    {
        Scanner scanner = new();
        foreach (var line in lines)
        {
            if (line.StartsWith("---"))
                scanner = new Scanner();
            else if (string.IsNullOrWhiteSpace(line))
                yield return scanner;
            else
            {
                var split = line.Split(',');
                scanner.beacons.Add(new Point3d(split[0].AsSpan(), split[1].AsSpan(), split[2].AsSpan()));
            }
        }

        yield return scanner;
    }

    private static IEnumerable<Func<Point3d, Point3d>> GetRotations()
    {
        yield return pt => new Point3d(pt.x, pt.y, pt.z);
        yield return pt => new Point3d(pt.x, pt.y, pt.z);
        yield return pt => new Point3d(pt.x, -pt.y, pt.z);
        yield return pt => new Point3d(-pt.x, pt.y, pt.z);
        yield return pt => new Point3d(-pt.x, -pt.y, pt.z);

        yield return pt => new Point3d(pt.y, pt.x, pt.z);
        yield return pt => new Point3d(pt.y, -pt.x, pt.z);
        yield return pt => new Point3d(-pt.y, pt.x, pt.z);
        yield return pt => new Point3d(-pt.y, -pt.x, pt.z);

        yield return pt => new Point3d(pt.x, pt.y, -pt.z);
        yield return pt => new Point3d(pt.x, -pt.y, -pt.z);
        yield return pt => new Point3d(-pt.x, pt.y, -pt.z);
        yield return pt => new Point3d(-pt.x, -pt.y, -pt.z);

        yield return pt => new Point3d(pt.y, pt.x, -pt.z);
        yield return pt => new Point3d(pt.y, -pt.x, -pt.z);
        yield return pt => new Point3d(-pt.y, pt.x, -pt.z);
        yield return pt => new Point3d(-pt.y, -pt.x, -pt.z);


        yield return pt => new Point3d(pt.x, pt.z, pt.y);//
        yield return pt => new Point3d(pt.x, pt.z, -pt.y);
        yield return pt => new Point3d(-pt.x, pt.z, pt.y);
        yield return pt => new Point3d(-pt.x, pt.z, -pt.y);

        yield return pt => new Point3d(pt.y, pt.z, pt.x);
        yield return pt => new Point3d(pt.y, pt.z, -pt.x);
        yield return pt => new Point3d(-pt.y, pt.z, pt.x);
        yield return pt => new Point3d(-pt.y, pt.z, -pt.x);

        yield return pt => new Point3d(pt.x, -pt.z, pt.y);
        yield return pt => new Point3d(pt.x, -pt.z, -pt.y);
        yield return pt => new Point3d(-pt.x, -pt.z, pt.y);
        yield return pt => new Point3d(-pt.x, -pt.z, -pt.y);//

        yield return pt => new Point3d(pt.y, -pt.z, pt.x);
        yield return pt => new Point3d(pt.y, -pt.z, -pt.x);
        yield return pt => new Point3d(-pt.y, -pt.z, pt.x);
        yield return pt => new Point3d(-pt.y, -pt.z, -pt.x);


        yield return pt => new Point3d(pt.z, pt.x, pt.y);
        yield return pt => new Point3d(pt.z, pt.x, -pt.y);
        yield return pt => new Point3d(pt.z, -pt.x, pt.y);
        yield return pt => new Point3d(pt.z, -pt.x, -pt.y);

        yield return pt => new Point3d(pt.z, pt.y, pt.x);
        yield return pt => new Point3d(pt.z, pt.y, -pt.x);
        yield return pt => new Point3d(pt.z, -pt.y, pt.x);
        yield return pt => new Point3d(pt.z, -pt.y, -pt.x);

        yield return pt => new Point3d(-pt.z, pt.x, pt.y);
        yield return pt => new Point3d(-pt.z, pt.x, -pt.y);
        yield return pt => new Point3d(-pt.z, -pt.x, pt.y);
        yield return pt => new Point3d(-pt.z, -pt.x, -pt.y);

        yield return pt => new Point3d(-pt.z, pt.y, pt.x);
        yield return pt => new Point3d(-pt.z, pt.y, -pt.x);
        yield return pt => new Point3d(-pt.z, -pt.y, pt.x);
        yield return pt => new Point3d(-pt.z, -pt.y, -pt.x);
    }

    public record Scanner()
    {
        public Point3d? Position { get; private set; }
        public HashSet<Point3d> beacons { get; } = new();
        public HashSet<Point3d> adjusted { get; } = new();

        public IEnumerable<(Point3d a, Point3d b)> GetAdjustedPairs()
        {
            return adjusted.SelectMany(a => adjusted.Select(b => (a, b))).Where((tuple => tuple.a != tuple.b));
        }

        public IEnumerable<(Point3d a, Point3d b)> GetPairs()
        {
            return beacons.SelectMany(a => beacons.Select(b => (a, b))).Where((tuple => tuple.a != tuple.b));
        }

        public void Adjust(Point3d position, Func<Point3d, Point3d> rotationFunction)
        {
            Position = position;
            adjusted.UnionWith(beacons.Select(rotationFunction).Select(x => x + position));
        }
    }
}
