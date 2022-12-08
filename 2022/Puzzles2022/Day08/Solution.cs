namespace Puzzles2022.Day08;

public record Solution(Dictionary<Point, long> grid, int width, int height) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var dict = lines.ToGrid<long>(ch => ch - '0');
        return new Solution(dict, lines[0].Length, lines.Length);
    }

    public async ValueTask<long> GetPart1()
    {
        var visible = new HashSet<Point>();

        // check the left
        for (var y = 0; y < height; y++)
        {
            var highestSoFar = -1L;
            for (var x = 0; x < width; x++)
            {
                var pt = new Point(x, y);
                if (grid[pt] > highestSoFar)
                {
                    visible.Add(pt);
                    highestSoFar = grid[pt];
                }
            }
        }

        // Check the right
        for (var y = 0; y < height; y++)
        {
            var highestSoFar = -1L;
            for (var x = width - 1; x >= 0; x--)
            {
                var pt = new Point(x, y);
                if (grid[pt] > highestSoFar)
                {
                    visible.Add(pt);
                    highestSoFar = grid[pt];
                }
            }
        }

        // check the top
        for (var x = 0; x < width; x++)
        {
            var highestSoFar = -1L;
            for (var y = 0; y < height; y++)
            {
                var pt = new Point(x, y);
                if (grid[pt] > highestSoFar)
                {
                    visible.Add(pt);
                    highestSoFar = grid[pt];
                }
            }
        }

        // check the bottom
        for (var x = 0; x < width; x++)
        {
            var highestSoFar = -1L;
            for (var y = height - 1; y >= 0; y--)
            {
                var pt = new Point(x, y);
                if (grid[pt] > highestSoFar)
                {
                    visible.Add(pt);
                    highestSoFar = grid[pt];
                }
            }
        }

        return visible.Count;
    }

    public async ValueTask<long> GetPart2() => grid.Keys.Select(GetVisibility).Max();

    private long GetVisibility(Point pt)
    {
        long treeHeight = grid[pt], north = 0, east = 0, south = 0, west = 0;

        for (var y = pt.y + 1; y < height; y++)
        {
            if (grid[pt with { y = y }] <= treeHeight)
                south++;
            if (grid[pt with { y = y }] >= treeHeight)
                break;
        }

        for (var y = pt.y - 1; y >= 0; y--)
        {
            if (grid[pt with { y = y }] <= treeHeight)
                north++;
            if (grid[pt with { y = y }] >= treeHeight)
                break;
        }

        for (var x = pt.x + 1; x < width; x++)
        {
            if (grid[pt with { x = x }] <= treeHeight)
                east++;
            if (grid[pt with { x = x }] >= treeHeight)
                break;
        }

        for (var x = pt.x - 1; x >= 0; x--)
        {
            if (grid[pt with { x = x }] <= treeHeight)
                west++;
            if (grid[pt with { x = x }] >= treeHeight)
                break;
        }

        return 1L * east * west * north * south;
    }
}
