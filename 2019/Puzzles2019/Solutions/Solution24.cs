namespace Puzzles2019.Solutions;

public record Solution24(string _start) : ISolution<Solution24>
{
    public static readonly Point ORIGIN = new Point(0, 0);
    public static readonly Point OUTER_EDGE = new Point(4, 4);
    public const char ALIVE = '#';
    public const char DEAD = '.';
    public const int WIDTH = 5;
    public const int HEIGHT = 5;

    public static readonly int[] _biodiversityOrder = new Point[]
    {
        new(0, 0),
        new(1, 0),
        new(2, 0),
        new(3, 0),
        new(4, 0),
        new(4, 1),
        new(3, 1),
        new(2, 1),
        new(1, 1),
        new(0, 1),
        new(0, 2),
        new(1, 2),
        new(2, 2),
        new(3, 2),
        new(4, 2),
        new(4, 3),
        new(3, 3),
        new(2, 3),
        new(1, 3),
        new(0, 3),
        new(0, 4),
        new(1, 4),
        new(2, 4),
        new(3, 4),
        new(4, 4),
    }.Select(ToIndex).ToArray();

    public static Solution24 Init(string[] lines) => new(string.Join("", lines));

    public async ValueTask<long> GetPart1()
    {
        var soFar = new HashSet<string>();
        var current = _start;
        do
        {
            soFar.Add(current);
            current = Iterate(current);
        } while (!soFar.Contains(current));

        return GetBioDiversity(current);
    }

    public async ValueTask<long> GetPart2()
    {
        return 0;
    }

    public string Iterate(string start)
    {
        var newString = new StringBuilder(WIDTH * HEIGHT);
        for (var y = 0; y < HEIGHT; y++)
        {
            for (var x = 0; x < WIDTH; x++)
            {
                var point = new Point(x, y);
                var index = ToIndex(point);
                var wasAlive = start[index] == ALIVE;
                var neighbors = point.GetOrthogonal().Where(pt => pt.IsWithinInclusive(ORIGIN, OUTER_EDGE)).ToList();
                var activeNeighbors = neighbors.Select(ToIndex).Count(i => start[i] == ALIVE);
                newString.Append((wasAlive && activeNeighbors == 1) || (!wasAlive && activeNeighbors is 1 or 2) ? ALIVE : DEAD);
            }
        }

        return newString.ToString();
    }

    public static long GetBioDiversity(string grid) =>
        _biodiversityOrder.Where(index => grid[index] == ALIVE).Sum(index => (long)Math.Pow(2, index));

    public static int ToIndex(Point point) => point.y * WIDTH + point.x;
}
