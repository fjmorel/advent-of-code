namespace Puzzles2021.Solutions;

public record Solution25(
    Dictionary<Point, char> _start,
    int _minX,
    int _maxX,
    int _minY,
    int _maxY
) : ISolution<Solution25>
{
    private const char EMPTY = '.';
    private const char RIGHT = '>';
    private const char DOWN = 'v';

    public static Solution25 Init(string[] lines)
    {
        var start = lines.ToGrid(x => x);
        return new(start, 0, lines[0].Length - 1, 0, lines.Length - 1);
    }

    public async ValueTask<long> GetPart1()
    {
        var changed = 1L;
        var steps = 0;
        var grid = _start;
        while (changed != 0)
        {
            steps++;
            (grid, changed) = Move(grid);
        }

        return steps;
    }

    public async ValueTask<long> GetPart2() => -1;

    public (Dictionary<Point, char>, long) Move(Dictionary<Point, char> before)
    {
        var changed = 0;
        var middle = new Dictionary<Point, char>();
        for (var y = _minY; y <= _maxY; y++)
        {
            for (var x = _maxX; x >= _minX; x--)
            {
                var pt = new Point(x, y);
                var value = before.GetValueOrDefault(pt, EMPTY);
                switch (value)
                {
                    case RIGHT:
                    {
                        var next = new Point(x == _maxX ? 0 : x + 1, y);
                        if (before.GetValueOrDefault(next, EMPTY) == EMPTY)
                        {
                            middle[next] = RIGHT;
                            middle[pt] = EMPTY;
                            changed++;
                        }
                        else
                        {
                            middle[pt] = RIGHT;
                        }

                        break;
                    }
                    case DOWN:
                        middle[pt] = DOWN;
                        break;
                    case EMPTY:
                        continue;
                }
            }
        }

        var after = new Dictionary<Point, char>();
        for (var x = _minX; x <= _maxX; x++)
        for (var y = _maxY; y >= _minX; y--)
        {
            {
                var pt = new Point(x, y);
                var value = middle.GetValueOrDefault(pt, EMPTY);
                switch (value)
                {
                    case DOWN:
                    {
                        var next = new Point(x, y == _maxY ? 0 : y + 1);
                        if (middle.GetValueOrDefault(next, EMPTY) == EMPTY)
                        {
                            after[next] = DOWN;
                            after[pt] = EMPTY;
                            changed++;
                        }
                        else
                        {
                            after[pt] = DOWN;
                        }

                        break;
                    }
                    case RIGHT:
                        after[pt] = RIGHT;
                        break;
                    case EMPTY:
                        continue;
                }
            }
        }

        return (after, changed);
    }
}
