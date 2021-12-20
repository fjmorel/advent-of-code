namespace Puzzles2021.Solutions;

public class Solution19 : ISolution
{
    private readonly Scanner[] _scanners;
    private readonly Func<Point3d, Point3d>[] _rotationFunctions;

    public Solution19(string[] lines)
    {
        _scanners = ParseInput(lines).ToArray();
        _rotationFunctions = GetRotations().ToArray();
    }

    public async ValueTask<long> GetPart1()
    {
        _scanners[0].Adjust(default, pt => pt);
        var located = new List<Scanner>() { _scanners[0] };
        var todo = new Queue<Scanner>(_scanners.Skip(1));

        while (todo.Any())
        {
            var scanner = todo.Dequeue();
            foreach (var origin in located)
            {
                if (scanner.TriedPositions.Contains(origin.Position!.Value))
                    continue;
                var found = TryFindOffset3d(origin, scanner);
                scanner.TriedPositions.Add(origin.Position.Value);
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

        var positions = _scanners.Select(x => x.Position!.Value).ToList();
        var pairs = positions.SelectMany(a => positions.Select(b => (a, b))).ToList();
        return pairs.Max(tuple => (tuple.b - tuple.a).GetMagnitude());
    }

    public bool TryFindOffset3d(Scanner origin, Scanner other)
    {
        var originPairs = GetPairs(origin.adjusted).ToList();
        var otherPairs = GetPairs(other.beacons).ToList();

        // For every origin beacon and other beacon,
        foreach (var func in _rotationFunctions)
        {
            HashSet<Match<Point3d>> matches = new();
            foreach (var (originA, originB) in originPairs)
            {
                var offset = originB - originA;
                foreach (var (otherA, otherB) in otherPairs)
                {
                    // for every rotation of other, find all the matches with origin
                    {
                        if (func(otherB) - func(otherA) == offset)
                        {
                            matches.Add(new(originA, otherA));
                            matches.Add(new(originB, otherB));
                        }
                    }
                    if (matches.Count > 12)
                        break;
                }

                if (matches.Count > 12)
                    break;
            }

            if (matches.Count == 12)
            {
                var match = matches.First();
                var position = match.origin - func(match.other);
                other.Adjust(position, func);
                return true;
            }
        }

        return false;
    }

    public record Match<T>(T origin, T other);

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


        yield return pt => new Point3d(pt.x, pt.z, pt.y);
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
        yield return pt => new Point3d(-pt.x, -pt.z, -pt.y);

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

    private static IEnumerable<(T a, T b)> GetPairs<T>(IReadOnlyCollection<T> list) where T : IEquatable<T>
        => list.SelectMany(a => list.Select(b => (a, b))).Where(tuple => !tuple.a.Equals(tuple.b));

    public record Scanner()
    {
        public Point3d? Position { get; private set; }
        public HashSet<Point3d> TriedPositions { get; } = new();
        public HashSet<Point3d> beacons { get; } = new();
        public HashSet<Point3d> adjusted { get; } = new();

        public void Adjust(Point3d position, Func<Point3d, Point3d> rotationFunction)
        {
            Position = position;
            adjusted.UnionWith(beacons.Select(rotationFunction).Select(x => x + position));
        }
    }
}
