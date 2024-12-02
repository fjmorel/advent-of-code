namespace Puzzles2021.Day23;

public record Solution(ImmutableStack<int>[] _start) : ISolution<Solution>
{
    private static readonly int[] _factor = [1, 10, 100, 1000];
    private static readonly int[] _roomY = [3, 5, 7, 9];
    private static readonly int[] _hallwayY = [1, 2, 4, 6, 8, 10, 11];

    public static Solution Init(string[] lines) =>
        new(_roomY.Select(y => ImmutableStack.Create(lines[2][y] - 'A', lines[3][y] - 'A')).ToArray());

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
    public async ValueTask<long> GetPart1() => FindBestSolution(CloneRooms(_start), ImmutableDictionary<int, int>.Empty, 2, 0);

    // 56243 is too low (wasn't valid anyway)
    public async ValueTask<long> GetPart2()
    {
        // Insert
        // #D#C#B#A#
        // #D#B#A#C#
        // between the top and bottom starting positions
        var start = CloneRooms(_start);
        start[0] = start[0].Pop(out var value).Push(3).Push(3).Push(value);
        start[1] = start[1].Pop(out value).Push(1).Push(2).Push(value);
        start[2] = start[2].Pop(out value).Push(0).Push(1).Push(value);
        start[3] = start[3].Pop(out value).Push(1).Push(0).Push(value);

        return FindBestSolution(start, ImmutableDictionary<int, int>.Empty, 4, 0);
    }

    public static int FindBestSolution(ImmutableStack<int>[] rooms, ImmutableDictionary<int, int> hallway, int roomSize, int score)
    {
        if (IsSolved(rooms, hallway))
            return score;

        int best = int.MaxValue;

        // Try to move hallway things back into their proper room
        foreach (var (position, thing) in hallway)
        {
            if (CanMoveToRoom(rooms, thing, roomSize) && CanMoveToHallwayPosition(hallway, position, thing)) ;
            {
                var newRooms = CloneRooms(rooms);
                var newScore = score + _factor[thing] * (int.Abs(thing - position) + (roomSize - newRooms[thing].Count()));
                newRooms[thing] = newRooms[thing].Push(thing);
                var newHallway = hallway.Remove(position);
                if (newScore < best)
                    best = int.Min(best, FindBestSolution(newRooms, newHallway, roomSize, newScore));
            }
        }

        // Try to move things from each room to other rooms / hallway
        for (var i = 0; i < rooms.Length; i++)
        {
            var fromY = _roomY[i];
            if (rooms[i].Any(x => x != i) && rooms[i].Peek() != i)
            {
                var newRooms = CloneRooms(rooms);
                newRooms[i] = newRooms[i].Pop(out var toY);

                if (CanMoveToRoom(newRooms, toY, roomSize) && CanMoveToHallwayPosition(hallway, fromY, toY)) ;
                {
                    // move up out of room, over, and down into new room
                    var newScore = score + _factor[toY] * (int.Abs(toY - fromY) + (roomSize - newRooms[toY].Count()) + (roomSize - newRooms[i].Count()));
                    newRooms[toY] = newRooms[toY].Push(toY);
                    if (newScore < best)
                        best = int.Min(best, FindBestSolution(newRooms, hallway, roomSize, newScore));
                }
            }

            // loop through each hallway position and move there
            foreach (var y in _hallwayY)
            {
                if (y != fromY && CanMoveToHallwayPosition(hallway, fromY, y))
                {
                    var newRooms = CloneRooms(rooms);
                    newRooms[i] = newRooms[i].Pop(out var thing);
                    // move up out of room and over
                    var newScore = score + _factor[thing] * (int.Abs(y - fromY) + (roomSize - newRooms[thing].Count()));
                    var newHallway = hallway.SetItem(y, thing);
                    if (newScore < best)
                        best = int.Min(best, FindBestSolution(newRooms, newHallway, roomSize, newScore));
                }
            }
        }

        return 0;
    }

    public static bool IsSolved(ImmutableStack<int>[] rooms, ImmutableDictionary<int, int> hallway)
    {
        if (!hallway.IsEmpty)
            return false;
        for (var i = 0; i < rooms.Length; i++)
        {
            foreach (var thing in rooms[i])
                if (thing != i)
                    return false;
        }

        return true;
    }

    public static bool CanMoveToRoom(ImmutableStack<int>[] rooms, int room, int roomSize) =>
        rooms[room].All(x => x == room) && rooms[room].Count() != roomSize;

    public static bool CanMoveToHallwayPosition(ImmutableDictionary<int, int> hallway, int from, int to)
    {
        if (from < to && hallway.Any(x => x.Key > from && x.Key <= to))
            return false;
        if (from > to && hallway.Any(x => x.Key >= to && x.Key < from))
            return false;
        return true;
    }

    public static ImmutableStack<int>[] CloneRooms(ImmutableStack<int>[] rooms)
    {
        var clone = new ImmutableStack<int>[4];
        rooms.CopyTo(clone.AsSpan());
        return clone;
    }
}
