namespace Puzzles2020.Solutions;

public class Solution22 : ISolution
{
    private readonly string[] list;

    public Solution22(string[] lines)
    {
        list = lines;
    }

    public async ValueTask<long> GetPart1()
    {
        var (p1Deck, p2Deck) = GetOriginalDecks();
        while (p1Deck.Any() && p2Deck.Any())
        {
            int p1 = p1Deck.Dequeue(), p2 = p2Deck.Dequeue();
            AddCardsToWinner(p1Deck, p2Deck, p1, p2, GetWinner(p1, p2));
        }

        return GetScore(p1Deck, p2Deck);
    }

    public async ValueTask<long> GetPart2()
    {
        var (p1Deck, p2Deck) = GetOriginalDecks();
        (p1Deck, p2Deck) = PlayRecursive(p1Deck, p2Deck);
        return GetScore(p1Deck, p2Deck);
    }

    long GetScore(IEnumerable<int> p1, IEnumerable<int> p2)
        => p1.Concat(p2).Reverse().Select((x, i) => x * (i + 1)).Sum();

    (Queue<int> p1Deck, Queue<int> p2Deck) PlayRecursive(Queue<int> p1Deck, Queue<int> p2Deck)
    {
        var states = new HashSet<string>();

        while (p1Deck.Any() && p2Deck.Any())
        {
            var state = string.Join(',', p1Deck) + '-' + string.Join(',', p2Deck);
            if (states.Contains(state))
                return (p1Deck, new Queue<int>());
            states.Add(state);

            int p1 = p1Deck.Dequeue(), p2 = p2Deck.Dequeue();
            if (p1 <= p1Deck.Count && p2 <= p2Deck.Count)
            {
                var (p1Sub, p2Sub) = PlayRecursive(new(p1Deck.Take(p1)), new(p2Deck.Take(p2)));
                AddCardsToWinner(p1Deck, p2Deck, p1, p2, GetWinner(p1Sub.Count, p2Sub.Count));
            }
            else
            {
                AddCardsToWinner(p1Deck, p2Deck, p1, p2, GetWinner(p1, p2));
            }
        }

        return (p1Deck, p2Deck);
    }

    int GetWinner(int p1Value, int p2Value) => p1Value > p2Value ? 1 : p2Value > p1Value ? 2 : 0;

    void AddCardsToWinner(Queue<int> p1Deck, Queue<int> p2Deck, int p1Card, int p2Card, int winner)
    {
        switch (winner)
        {
            case 1:
                p1Deck.Enqueue(p1Card);
                p1Deck.Enqueue(p2Card);
                break;
            case 2:
                p2Deck.Enqueue(p2Card);
                p2Deck.Enqueue(p1Card);
                break;
            default:
                p1Deck.Enqueue(p1Card);
                p2Deck.Enqueue(p2Card);
                break;
        }
    }

    (Queue<int> p1Deck, Queue<int> p2Deck) GetOriginalDecks()
    {
        Queue<int> p1deck = new(), p2deck = new();
        var i = 1;
        for (; list[i] != ""; i++)
            p1deck.Enqueue(int.Parse(list[i]));

        i++;
        i++;
        for (; i < list.Length; i++)
            p2deck.Enqueue(int.Parse(list[i]));
        return (p1deck, p2deck);
    }

    record Memory(int[] p1, int[] p2);
}
