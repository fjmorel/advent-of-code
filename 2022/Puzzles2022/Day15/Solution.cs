namespace Puzzles2022.Day15;

public partial record Solution(List<Solution.Sensor> _lines) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines.ToList(ParseLine));

    private static Sensor ParseLine(string line)
    {
        var captures = GetLineRegex().Match(line).Groups;
        return new(new(captures[1], captures[2]), new(captures[3], captures[4]));
    }

    public async ValueTask<long> GetPart1()
    {
        const int row = 2_000_000;// 10
        var beaconsInRow = _lines.Where(x => x.closestBeacon.y == row).Select(x => x.closestBeacon.x).Distinct().ToList();
        var combinedRanges = GetCoverageForRow(row);
        return combinedRanges.Select(x => (long)(x.max - x.min + 1)).Sum() - beaconsInRow.Count;
    }

    // this was a terrible idea except for the example
    // public async ValueTask<long> GetPart1()
    // {
    //     var beacons = new Dictionary<Point, bool>();
    //     foreach (var sensor in _lines)
    //     {
    //         var maxDistance = sensor.location.ManhattanDistance(sensor.closestBeacon);
    //         beacons[sensor.location] = false;
    //         while (maxDistance > 0)
    //         {
    //             for (var i = 0; i <= maxDistance; i++)
    //             {
    //                 beacons[sensor.location + new Point(-i, i - maxDistance)] = false;
    //                 beacons[sensor.location + new Point(i, i - maxDistance)] = false;
    //                 beacons[sensor.location + new Point(-i, maxDistance - i)] = false;
    //                 beacons[sensor.location + new Point(i, maxDistance - i)] = false;
    //                 beacons[sensor.location + new Point(maxDistance - i, i)] = false;
    //                 beacons[sensor.location + new Point(maxDistance - i, -i)] = false;
    //                 beacons[sensor.location + new Point(i - maxDistance, i)] = false;
    //                 beacons[sensor.location + new Point(i - maxDistance, -i)] = false;
    //             }
    //
    //             maxDistance--;
    //         }
    //     }
    //
    //     foreach (var sensor in _lines)
    //         beacons[sensor.closestBeacon] = true;
    //
    //     var test = beacons.Keys.ToString(x => beacons.ContainsKey(x) ? beacons[x] ? '❌' : '⬜' : '⬛' );
    //
    //     return beacons.Count(x => x.Key.y == 10 && !x.Value);
    // }

    public async ValueTask<long> GetPart2()
    {
        const int size = 4_000_000;// 20
        var solution = 0L;
        Parallel.ForEach(Enumerable.Range(0, size + 1), (y, state) =>
        {
            var coverage = GetCoverageForRow(y).FirstOrDefault(x => x is { min: <= 0, max: <= size });
            if (coverage == default)
                return;

            solution = (coverage.max + 1L) * 4_000_000 + y;
            state.Stop();
        });

        return solution;
    }

    public IEnumerable<(int min, int max)> GetCoverageForRow(int row)
    {
        var sensorsCoveringRow = new Queue<(int min, int max)>(_lines
            .Select(x => x.GetRangeAtY(row))
            .Where(x => x is not null).Select(x => x!.Value)
            .OrderBy(x => x.min).ThenBy(x => x.max));

        var current = sensorsCoveringRow.Dequeue();
        while (sensorsCoveringRow.TryDequeue(out var next))
        {
            if (next.min <= current.max)
                current = (current.min, int.Max(current.max, next.max));
            else
            {
                yield return current;
                current = next;
            }
        }

        yield return current;
    }

    public record Sensor(Point location, Point closestBeacon)
    {
        public readonly int MaxDistance = location.GetManhattanDistance(closestBeacon);

        public (int min, int max)? GetRangeAtY(int y)
        {
            if (location.y + MaxDistance < y)
                return null;
            if (location.y - MaxDistance > y)
                return null;

            var range = MaxDistance - int.Abs(y - location.y);
            return (location.x - range, location.x + range);
        }
    }

    [GeneratedRegex("""Sensor at x=([\-0-9]+), y=([\-0-9]+): closest beacon is at x=([\-0-9]+), y=([\-0-9]+)""")]
    private static partial Regex GetLineRegex();
}
