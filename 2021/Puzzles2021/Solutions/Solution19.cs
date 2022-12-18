namespace Puzzles2021.Solutions;

public record Solution19(Solution19.Scanner[] _scanners) : ISolution<Solution19>
{
    private static readonly Func<Point3d, Point3d>[] _rotationFunctions = GetRotations().ToArray();
    private readonly ConcurrentBag<AdjustedScanner> Located = new();

    public static Solution19 Init(string[] lines)
    {
        var scanners = new List<Scanner>();
        var beacons = new HashSet<Point3d>();
        foreach (var line in lines)
        {
            if (line.StartsWith("---"))
                beacons = new();
            else if (string.IsNullOrWhiteSpace(line))
                scanners.Add(new Scanner(beacons));
            else
            {
                beacons.Add(new Point3d(line.ParseCsv<int>()));
            }
        }

        scanners.Add(new Scanner(beacons));
        return new(scanners.ToArray());
    }

    public async ValueTask<long> GetPart1()
    {
        if (!Located.Any())
            AlignScanners(_scanners);

        return Located.SelectMany(x => x.adjusted).Distinct().Count();
    }

    public async ValueTask<long> GetPart2()
    {
        if (!Located.Any())
            AlignScanners(_scanners);

        var positions = Located.Select(x => x.position).ToList();
        return positions.SelectMany(a => positions.Select(b => (b - a).GetMagnitude())).Max();
    }

    private void AlignScanners(Scanner[] scanners)
    {
        Located.Add(scanners[0].Adjust(default, pt => pt));
        var left = new ConcurrentBag<Scanner>(scanners.Skip(1));

        int tries = 0;
        while (left.Any() && tries < scanners.Length)
        {
            var todo = left.ToList();
            left.Clear();
            Parallel.ForEach(todo, scanner =>
            {
                AdjustedScanner? maybeAdjusted = null;
                foreach (var origin in Located)
                {
                    if (scanner.Tried.Contains(origin.position))
                        continue;
                    maybeAdjusted = TryFindOffset3d(origin, scanner);
                    scanner.Tried.Add(origin.position);
                    if (maybeAdjusted != null)
                        break;
                }

                if (maybeAdjusted is AdjustedScanner adjusted)
                    Located.Add(adjusted);
                else
                    left.Add(scanner);
            });
            tries++;
        }

        if (left.Any())
            throw new UnreachableException("Stuck in infinite loop");
    }

    public static AdjustedScanner? TryFindOffset3d(AdjustedScanner origin, Scanner other)
    {
        if (other.Distances.Intersect(origin.distances).Count() < 12)
            return null;

        // For every origin beacon and other beacon,
        foreach (var func in _rotationFunctions)
        {
            HashSet<(Point3d origin, Point3d other)> matches = new();
            foreach (var (originA, originB) in origin.pairs)
            {
                var offset = originB - originA;
                foreach (var (otherA, otherB) in other.Pairs)
                {
                    // for every rotation of other, find all the matches with origin
                    {
                        if (func(otherB) - func(otherA) == offset)
                        {
                            matches.Add((originA, otherA));
                            matches.Add((originB, otherB));
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

    private static IEnumerable<Func<Point3d, Point3d>> GetRotations()
    {
        yield return static pt => pt;
        yield return static pt => new Point3d(pt.x, -pt.y, pt.z);
        yield return static pt => new Point3d(pt.x, pt.y, -pt.z);
        yield return static pt => new Point3d(pt.x, -pt.y, -pt.z);
        yield return static pt => new Point3d(-pt.x, pt.y, pt.z);
        yield return static pt => new Point3d(-pt.x, pt.y, -pt.z);
        yield return static pt => new Point3d(-pt.x, -pt.y, pt.z);
        yield return static pt => new Point3d(-pt.x, -pt.y, -pt.z);

        yield return static pt => new Point3d(pt.y, pt.x, pt.z);
        yield return static pt => new Point3d(pt.y, -pt.x, pt.z);
        yield return static pt => new Point3d(pt.y, pt.x, -pt.z);
        yield return static pt => new Point3d(pt.y, -pt.x, -pt.z);
        yield return static pt => new Point3d(-pt.y, pt.x, pt.z);
        yield return static pt => new Point3d(-pt.y, pt.x, -pt.z);
        yield return static pt => new Point3d(-pt.y, -pt.x, pt.z);
        yield return static pt => new Point3d(-pt.y, -pt.x, -pt.z);

        yield return static pt => new Point3d(pt.x, pt.z, pt.y);
        yield return static pt => new Point3d(pt.x, pt.z, -pt.y);
        yield return static pt => new Point3d(pt.x, -pt.z, pt.y);
        yield return static pt => new Point3d(pt.x, -pt.z, -pt.y);
        yield return static pt => new Point3d(-pt.x, pt.z, pt.y);
        yield return static pt => new Point3d(-pt.x, pt.z, -pt.y);
        yield return static pt => new Point3d(-pt.x, -pt.z, pt.y);
        yield return static pt => new Point3d(-pt.x, -pt.z, -pt.y);

        yield return static pt => new Point3d(pt.y, pt.z, pt.x);
        yield return static pt => new Point3d(pt.y, pt.z, -pt.x);
        yield return static pt => new Point3d(pt.y, -pt.z, pt.x);
        yield return static pt => new Point3d(pt.y, -pt.z, -pt.x);
        yield return static pt => new Point3d(-pt.y, pt.z, pt.x);
        yield return static pt => new Point3d(-pt.y, pt.z, -pt.x);
        yield return static pt => new Point3d(-pt.y, -pt.z, pt.x);
        yield return static pt => new Point3d(-pt.y, -pt.z, -pt.x);

        yield return static pt => new Point3d(pt.z, pt.x, pt.y);
        yield return static pt => new Point3d(pt.z, pt.x, -pt.y);
        yield return static pt => new Point3d(pt.z, -pt.x, pt.y);
        yield return static pt => new Point3d(pt.z, -pt.x, -pt.y);
        yield return static pt => new Point3d(-pt.z, pt.x, pt.y);
        yield return static pt => new Point3d(-pt.z, pt.x, -pt.y);
        yield return static pt => new Point3d(-pt.z, -pt.x, pt.y);
        yield return static pt => new Point3d(-pt.z, -pt.x, -pt.y);

        yield return static pt => new Point3d(pt.z, pt.y, pt.x);
        yield return static pt => new Point3d(pt.z, pt.y, -pt.x);
        yield return static pt => new Point3d(pt.z, -pt.y, pt.x);
        yield return static pt => new Point3d(pt.z, -pt.y, -pt.x);
        yield return static pt => new Point3d(-pt.z, pt.y, pt.x);
        yield return static pt => new Point3d(-pt.z, pt.y, -pt.x);
        yield return static pt => new Point3d(-pt.z, -pt.y, pt.x);
        yield return static pt => new Point3d(-pt.z, -pt.y, -pt.x);
    }

    public readonly record struct Scanner
    {
        public Scanner(HashSet<Point3d> beacons)
        {
            Beacons = beacons;
            Pairs = GetPairs(beacons).ToList();
            Distances = Pairs.Select(x => (x.Item1 - x.Item2).GetMagnitude()).ToHashSet();
        }

        private HashSet<Point3d> Beacons { get; }
        public List<(Point3d, Point3d)> Pairs { get; }
        public HashSet<long> Distances { get; }
        public HashSet<Point3d> Tried { get; } = new();

        public AdjustedScanner Adjust(Point3d position, Func<Point3d, Point3d> rotationFunction)
        {
            var adj = Beacons.Select(rotationFunction).Select(x => x + position).ToHashSet();
            return new(position, adj, GetPairs(adj).ToList(), Distances);
        }

        private static IEnumerable<(Point3d a, Point3d b)> GetPairs(IReadOnlyCollection<Point3d> beacons) =>
            from a in beacons from b in beacons where a != b select (a, b);
    }

    public readonly record struct AdjustedScanner(Point3d position, HashSet<Point3d> adjusted, List<(Point3d, Point3d)> pairs, HashSet<long> distances);
}
