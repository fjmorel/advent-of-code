namespace Puzzles2021.Solutions;

public record Solution19(Solution19.Scanner[] _scanners, Func<Point3d, Point3d>[] _rotationFunctions) : ISolution<Solution19>
{
    private readonly ConcurrentBag<AdjustedScanner> Located = new();

    public static Solution19 Init(string[] lines)
    {
        var scanners = ParseInput(lines).ToArray();
        var rotationFunctions = GetRotations().ToArray();
        return new(scanners, rotationFunctions);
    }

    public async ValueTask<long> GetPart1()
    {
        Located.Add(_scanners[0].Adjust(default, pt => pt));
        var left = new ConcurrentBag<Scanner>(_scanners.Skip(1));

        while (left.Any())
        {
            var todo = left.ToList();
            left.Clear();
            Parallel.ForEach(todo, scanner =>
            {
                AdjustedScanner? maybeAdjusted = null;
                foreach (var origin in Located)
                {
                    if (scanner.Tried.Contains(origin))
                        continue;
                    maybeAdjusted = TryFindOffset3d(origin, scanner);
                    scanner.Tried.Add(origin);
                    if (maybeAdjusted != null)
                        break;
                }

                if (maybeAdjusted is AdjustedScanner adjusted)
                    Located.Add(adjusted);
                else
                    left.Add(scanner);
            });
        }

        return Located.SelectMany(x => x.adjusted).Distinct().Count();
    }

    public async ValueTask<long> GetPart2()
    {
        if (!Located.Any())
            await GetPart1();

        var positions = Located.Select(x => x.position).ToList();
        var pairs = positions.SelectMany(a => positions.Select(b => (a, b))).ToList();
        return pairs.Max(tuple => (tuple.b - tuple.a).GetMagnitude());
    }

    public AdjustedScanner? TryFindOffset3d(AdjustedScanner origin, Scanner other)
    {
        if (other.Distances.Intersect(origin.distances).Count() < 12)
            return null;
        
        // For every origin beacon and other beacon,
        foreach (var func in _rotationFunctions)
        {
            HashSet<Match<Point3d>> matches = new();
            foreach (var (originA, originB) in origin.pairs)
            {
                var offset = originB - originA;
                foreach (var (otherA, otherB) in other.Pairs)
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
                return other.Adjust(position, func);
            }
        }

        return null;
    }

    public readonly record struct Match<T>(T origin, T other);

    public static IEnumerable<Scanner> ParseInput(string[] lines)
    {
        var beacons = new HashSet<Point3d>();
        foreach (var line in lines)
        {
            if (line.StartsWith("---"))
                beacons = new();
            else if (string.IsNullOrWhiteSpace(line))
                yield return new Scanner(beacons, GetPairs(beacons).ToList());
            else
            {
                var split = line.Split(',');
                beacons.Add(new Point3d(split[0].AsSpan(), split[1].AsSpan(), split[2].AsSpan()));
            }
        }

        yield return new Scanner(beacons, GetPairs(beacons).ToList());
    }

    private static IEnumerable<Func<Point3d, Point3d>> GetRotations()
    {
        yield return static pt => pt;
        yield return static pt => new Point3d(pt.x, -pt.y, pt.z);
        yield return static pt => new Point3d(-pt.x, pt.y, pt.z);
        yield return static pt => new Point3d(-pt.x, -pt.y, pt.z);

        yield return static pt => new Point3d(pt.y, pt.x, pt.z);
        yield return static pt => new Point3d(pt.y, -pt.x, pt.z);
        yield return static pt => new Point3d(-pt.y, pt.x, pt.z);
        yield return static pt => new Point3d(-pt.y, -pt.x, pt.z);

        yield return static pt => new Point3d(pt.x, pt.y, -pt.z);
        yield return static pt => new Point3d(pt.x, -pt.y, -pt.z);
        yield return static pt => new Point3d(-pt.x, pt.y, -pt.z);
        yield return static pt => new Point3d(-pt.x, -pt.y, -pt.z);

        yield return static pt => new Point3d(pt.y, pt.x, -pt.z);
        yield return static pt => new Point3d(pt.y, -pt.x, -pt.z);
        yield return static pt => new Point3d(-pt.y, pt.x, -pt.z);
        yield return static pt => new Point3d(-pt.y, -pt.x, -pt.z);


        yield return static pt => new Point3d(pt.x, pt.z, pt.y);
        yield return static pt => new Point3d(pt.x, pt.z, -pt.y);
        yield return static pt => new Point3d(-pt.x, pt.z, pt.y);
        yield return static pt => new Point3d(-pt.x, pt.z, -pt.y);

        yield return static pt => new Point3d(pt.y, pt.z, pt.x);
        yield return static pt => new Point3d(pt.y, pt.z, -pt.x);
        yield return static pt => new Point3d(-pt.y, pt.z, pt.x);
        yield return static pt => new Point3d(-pt.y, pt.z, -pt.x);

        yield return static pt => new Point3d(pt.x, -pt.z, pt.y);
        yield return static pt => new Point3d(pt.x, -pt.z, -pt.y);
        yield return static pt => new Point3d(-pt.x, -pt.z, pt.y);
        yield return static pt => new Point3d(-pt.x, -pt.z, -pt.y);

        yield return static pt => new Point3d(pt.y, -pt.z, pt.x);
        yield return static pt => new Point3d(pt.y, -pt.z, -pt.x);
        yield return static pt => new Point3d(-pt.y, -pt.z, pt.x);
        yield return static pt => new Point3d(-pt.y, -pt.z, -pt.x);


        yield return static pt => new Point3d(pt.z, pt.x, pt.y);
        yield return static pt => new Point3d(pt.z, pt.x, -pt.y);
        yield return static pt => new Point3d(pt.z, -pt.x, pt.y);
        yield return static pt => new Point3d(pt.z, -pt.x, -pt.y);

        yield return static pt => new Point3d(pt.z, pt.y, pt.x);
        yield return static pt => new Point3d(pt.z, pt.y, -pt.x);
        yield return static pt => new Point3d(pt.z, -pt.y, pt.x);
        yield return static pt => new Point3d(pt.z, -pt.y, -pt.x);

        yield return static pt => new Point3d(-pt.z, pt.x, pt.y);
        yield return static pt => new Point3d(-pt.z, pt.x, -pt.y);
        yield return static pt => new Point3d(-pt.z, -pt.x, pt.y);
        yield return static pt => new Point3d(-pt.z, -pt.x, -pt.y);

        yield return static pt => new Point3d(-pt.z, pt.y, pt.x);
        yield return static pt => new Point3d(-pt.z, pt.y, -pt.x);
        yield return static pt => new Point3d(-pt.z, -pt.y, pt.x);
        yield return static pt => new Point3d(-pt.z, -pt.y, -pt.x);
    }

    private static IEnumerable<(Point3d a, Point3d b)> GetPairs(HashSet<Point3d> list)
    {
        foreach (var a in list)
        {
            foreach (var b in list)
            {
                if (a == b)
                    continue;
                yield return (a, b);
            }
        }
    }

    public readonly record struct Scanner(HashSet<Point3d> beacons, List<(Point3d, Point3d)> Pairs)
    {
        public HashSet<long> Distances { get; } = Pairs.Select(x => (x.Item1 - x.Item2).GetMagnitude()).ToHashSet();
        public HashSet<AdjustedScanner> Tried { get; } = new();

        public AdjustedScanner Adjust(Point3d position, Func<Point3d, Point3d> rotationFunction)
        {
            var adj = beacons.Select(rotationFunction).Select(x => x + position).ToHashSet();
            return new(position, adj, GetPairs(adj).ToList(), Distances);
        }
    }

    public readonly record struct AdjustedScanner(Point3d position, HashSet<Point3d> adjusted, List<(Point3d, Point3d)> pairs, HashSet<long> distances);
}
