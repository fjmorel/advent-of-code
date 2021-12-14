namespace Puzzles2020.Solutions;

public class Solution23 : ISolution
{
    private readonly int[] startingCups;

    public Solution23(string[] lines)
    {
        startingCups = lines[0].Select(ch => int.Parse(ch.ToString())).ToArray();
    }

    public async ValueTask<long> GetPart1()
    {
        var pointers = RunSimulation(startingCups, 100, 9);
        var current = pointers[1];
        var result = new StringBuilder().Append(current);
        while (current != 1)
            result.Append(current = pointers[current]);
        return long.Parse(result.Remove(8, 1).ToString());
    }

    public async ValueTask<long> GetPart2()
    {
        var pointers = RunSimulation(startingCups, 10_000_000, 1_000_000);
        return (long)pointers[1] * (long)pointers[pointers[1]];
    }

    private static int[] RunSimulation(IReadOnlyList<int> startingCups, int iterations, int max)
    {
        var removed = new int[3];
        var pointers = new int[max + 1];

        // initialize 10-max in numerical order
        for (var i = 10; i <= max; i++)
            pointers[i] = i + 1;
        // initialize 1-9 in input order
        for (var i = 0; i < startingCups.Count - 1; i++)
            pointers[startingCups[i]] = startingCups[i + 1];

        var current = startingCups[0];

        // If only 9 cups, make last input point to first input
        if (max == startingCups.Count)
            pointers[startingCups[^1]] = current;
        // Otherwise last input => 10 && max >= first input
        else
        {
            pointers[startingCups[^1]] = 10;
            pointers[max] = current;
        }

        for (var i = 0; i < iterations; i++)
        {
            // Store removed cups
            removed[0] = pointers[current];
            removed[1] = pointers[removed[0]];
            removed[2] = pointers[removed[1]];

            // Figure out where they go
            var destination = current - 1;
            while (destination == 0 || removed.Contains(destination))
                destination = destination == 0 ? max : destination - 1;

            // Update cup locations
            pointers[current] = pointers[removed[2]];
            pointers[removed[2]] = pointers[destination];
            pointers[destination] = removed[0];
            current = pointers[current];
        }

        return pointers;
    }
}
