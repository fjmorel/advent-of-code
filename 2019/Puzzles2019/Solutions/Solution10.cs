namespace Puzzles2019.Solutions;

public record Solution10(
    KeyValuePair<Point, HashSet<Point>> best,
    int height,
    int width,
    HashSet<Point> asteroids,
    HashSet<Point> slopes
) : ISolution<Solution10>
{
    public static Solution10 Init(string[] lines)
    {
        var grid = lines.Select(line => line.Select(x => x == '#').ToArray()).ToArray();
        var height = grid.Length;
        var width = grid[0].Length;
        var slopes = GetSlopes(width, height).ToHashSet();
        var asteroids = GetAsteroids(grid, width, height).ToHashSet();
        var visiblePerAsteroid = asteroids.ToDictionary(x => x, x => GetVisibleAsteroids(x, asteroids, slopes, width, height));
        var best = visiblePerAsteroid.MaxBy(x => x.Value.Count);

        return new(best, height, width, asteroids, slopes);
    }

    public async ValueTask<long> GetPart1()
    {
        return best.Value.Count;
    }

    public async ValueTask<long> GetPart2()
    {
        // Identify station and initial visible asteroids
        var station = best.Key;
        var visible = best.Value;

        var leftover = new HashSet<Point>(asteroids);
        var destroyed = new List<Point>();
        while (leftover.Count > 1 && destroyed.Count < 200)
        {
            // Destroy the visible asteroids, then find new ones to destroy
            destroyed.AddRange(visible.OrderBy(x => GetAngle(station, x)));
            leftover.ExceptWith(visible);
            visible = GetVisibleAsteroids(station, leftover, slopes, width, height);
        }

        var theOne = destroyed[199];
        return theOne.x * 100 + theOne.y;
    }

    private static IEnumerable<Point> GetAsteroids(bool[][] grid, int width, int height)
    {
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (grid[y][x])
                    yield return new(x, y);
            }
        }
    }

    private static IEnumerable<Point> GetSlopes(int width, int height)
    {
        yield return new(0, 1);
        yield return new(0, -1);
        yield return new(1, 0);
        yield return new(-1, 0);

        yield return new(1, 1);
        yield return new(1, -1);
        yield return new(-1, 1);
        yield return new(-1, -1);

        for (var x = 1; x < width; x++)
        {
            for (var y = 1; y < height; y++)
            {
                if (x < 2 && y < 2)
                {
                    yield return new(x, y);
                    yield return new(x, -y);
                    yield return new(-x, y);
                    yield return new(-x, -y);
                }
                else
                {
                    var multiple = false;
                    for (var test = int.Max(x, y); test > 1; test--)
                    {
                        if (x % test == 0 && y % test == 0)
                        {
                            multiple = true;
                            break;
                        }
                    }

                    if (!multiple)
                    {
                        yield return new(x, y);
                        yield return new(x, -y);
                        yield return new(-x, y);
                        yield return new(-x, -y);
                    }
                }
            }
        }
    }

    public static double GetAngle(Point station, Point somewhere)
    {
        var fromOrigin = somewhere - station;
        fromOrigin = fromOrigin with { y = -fromOrigin.y };// invert y to fix above/below
        var hypotenuse = double.Sqrt(double.Pow(fromOrigin.x, 2) + double.Pow(fromOrigin.y, 2));

        double theta;
        if (fromOrigin.y == 0)
        {
            var ah = fromOrigin.x / hypotenuse;
            theta = double.Acos(ah);
        }
        else
        {
            var oh = fromOrigin.y / hypotenuse;
            theta = double.Asin(oh);

            // Make sure to correctly convert -x to left side of circle
            if (fromOrigin.x < 0)
            {
                // top-right to top-left
                if (theta > 0)
                    theta = double.Pi - theta;
                // bottom-right to bottom-left
                else if (theta < 0)
                    theta = -double.Pi - theta;
            }
        }

        var degrees = (180 / double.Pi) * theta;

        // turn normal unit circle into 0 at top going clockwise
        var adj = degrees switch
        {
            90 => 0,
            0 => 90,
            -90 => 180,
            -180 => 270,
            180 => 270,

            > 0 and < 90 => (90 - degrees),// reverse it
            > 90 and < 180 => (180 - degrees + 270),// reverse it and bump it up to last quadrant

            > -180 and < -90 => (-degrees + 90),// right order, make it positive and bump up to 3rd quadrant
            > -90 and < 0 => (-degrees + 90),// right order, make it positive and bump up to 2nd quadrant

            _ => throw new NotSupportedException("Unexpected degree value: " + degrees),
        };

        return adj;
    }

    public static HashSet<Point> GetVisibleAsteroids(Point station, HashSet<Point> asteroids, HashSet<Point> slopes, int width, int height)
    {
        var detected = new HashSet<Point>();
        // Explore along each possible line to find an asteroid within the grid
        foreach (var slope in slopes)
        {
            var pt = station + slope;
            while (pt.x >= 0 && pt.x < width && pt.y >= 0 && pt.y < height)
            {
                if (asteroids.Contains(pt))
                {
                    detected.Add(pt);
                    break;
                }

                pt += slope;
            }
        }

        return detected;
    }
}
