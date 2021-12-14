namespace Puzzles2020.Solutions;

public class Solution11 : ISolution
{
    private const char FLOOR = '.', EMPTY = 'L', OCCUPIED = '#';
    private readonly char[][] read;
    private readonly Point size;

    public Solution11(string[] lines)
    {
        read = lines.Select(x => x.ToCharArray()).ToArray();
        size = new Point(read.Length, read[0].Length);
    }

    public async ValueTask<long> GetPart1() => FindSteadyState(read, size, 4, false);

    public async ValueTask<long> GetPart2() => FindSteadyState(read, size, 5, true);

    private static int FindSteadyState(char[][] read, Point size, int threshold, bool lineOfSight)
    {
        var positions = read.Select((x, i) => x.Select((_, j) => GetSurroundingPositions(new(i, j), size, read, lineOfSight)).ToArray()).ToArray();
        var write = Iterate(read, threshold, positions);
        while (!read.SelectMany(x => x).SequenceEqual(write.SelectMany(x => x)))
        {
            read = write;
            write = Iterate(read, threshold, positions);
        }

        return write.Sum(x => x.Count(y => y == OCCUPIED));
    }

    private static char[][] Iterate(char[][] read, int threshold, Point[][][] positions)
        => read.Select((row, i) => row.Select((_, j) => FindNewState(i, j, read, threshold, positions[i][j])).ToArray()).ToArray();

    private static char FindNewState(int i, int j, char[][] read, int threshold, Point[] positions)
        => read[i][j] switch
        {
            FLOOR => FLOOR,
            EMPTY => positions.Any(adj => read[adj.x][adj.y] == OCCUPIED) ? EMPTY : OCCUPIED,
            OCCUPIED => positions.Count(adj => read[adj.x][adj.y] == OCCUPIED) >= threshold ? EMPTY : OCCUPIED,
            _ => throw new NotSupportedException(),
        };

    private static Point[] GetSurroundingPositions(Point coord, Point size, char[][] read, bool lineOfSight)
        => new Point[] { new(-1, -1), new(-1, 0), new(-1, 1), new(0, -1), new(0, 1), new(1, -1), new(1, 0), new(1, 1) }
            .Select(inc => lineOfSight ? FindLineOfSight(coord + inc, size, read, inc) : coord + inc)
            .Where(x => x.IsWithin(size)).ToArray();

    private static Point FindLineOfSight(Point adj, Point size, char[][] read, Point inc)
        => (adj.IsWithin(size) && read[adj.x][adj.y] == FLOOR) ? FindLineOfSight(adj + inc, size, read, inc) : adj;
}
