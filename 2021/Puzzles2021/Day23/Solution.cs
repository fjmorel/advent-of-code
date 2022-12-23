namespace Puzzles2021.Day23;

public record Solution(string[] _lines) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines);

/*
Example: 12521
Input: 10607
D	2
C	5
D	3
D	4
A	3
C	3
B	2
A	5
B	3
B	4
C	7
A	3
A	6
*/
    public async ValueTask<long> GetPart1() => FindBestSolution();

    public async ValueTask<long> GetPart2()
    {
        // Insert
        // #D#C#B#A#
        // #D#B#A#C#
        // between the top and bottom starting positions
        // then solve again
        return FindBestSolution();
    }

    public int FindBestSolution()
    {
        // while any pods not in the right place:
        // move one pod. they can move:
        // - from wrong rooms, along hallway and into hallway / correct room
        // - back into correct room (if it contains no wrong ones)
        // They cannot move in hallway twice, block rooms, or move past each other
        return 0;
    }

    public static int GetAddedScore(char type, int movement) => movement * type switch
    {
        'A' => 1,
        'B' => 10,
        'C' => 100,
        'D' => 1000,
    };
}
