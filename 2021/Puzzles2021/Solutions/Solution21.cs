namespace Puzzles2021.Solutions;

public class Solution21 : ISolution
{
    private readonly int[] _probabilities = { 0, 0, 0, 1, 3, 6, 7, 6, 3, 1 };//3-9
    private readonly Player _p1;
    private readonly Player _p2;


    public Solution21(string[] lines)
    {
        var position1 = int.Parse(new string(lines[0][^1], 1));
        _p1 = new Player(position1, IsP1: true);

        var position2 = int.Parse(new string(lines[1][^1], 1));
        _p2 = new Player(position2);
    }

    public async ValueTask<long> GetPart1()
    {
        var (next, previous) = (_p1, _p2);
        int rolls = 0, die = 0;

        do
        {
            var total = 0;
            for (var i = 1; i <= 3; i++)
            {
                rolls++;
                die++;
                if (die > 100)
                    die -= 100;
                total += die;
            }

            (next, previous) = (previous, next.WithMove(total));
        } while (previous.Score < 1000 & next.Score < 1000);

        return rolls * Math.Min(previous.Score, next.Score);
    }

    public async ValueTask<long> GetPart2()
    {
        var wins = FindWins(_p1, _p2, 1);
        return Math.Max(wins.p1, wins.p2);
    }

    public (long p1, long p2) FindWins(Player next, Player previous, long multiplier)
    {
        long p1 = 0L, p2 = 0L;
        for (var i = 3; i <= 9; i++)
        {
            var wins = FindWinsForRoll(i, next, previous, multiplier * _probabilities[i]);
            p1 += wins.p1;
            p2 += wins.p2;
        }

        return (p1, p2);
    }

    private (long p1, long p2) FindWinsForRoll(int roll, Player next, Player previous, long multiplier)
    {
        var moved = next.WithMove(roll);
        if (moved.Score >= 21)
            return moved.IsP1 ? (multiplier, 0) : (0, multiplier);
        return FindWins(previous, moved, multiplier);
    }

    public readonly record struct Player(int Position, int Score = 0, bool IsP1 = false)
    {
        public Player WithMove(int add)
        {
            var newPosition = Position + add;
            while (newPosition > 10)
                newPosition -= 10;
            return this with { Position = newPosition, Score = Score + newPosition };
        }
    }
}
