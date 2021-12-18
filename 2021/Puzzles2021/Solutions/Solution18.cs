using System.Text.Json.Nodes;

namespace Puzzles2021.Solutions;

public class Solution18 : ISolution
{
    private readonly string[] _lines;

    public Solution18(string[] lines)
    {
        _lines = lines;
    }

    public async ValueTask<long> GetPart1()
    {
        var pairs = _lines.Select(Convert).ToList();
        foreach (var pair in pairs)
        {
            //AnsiConsole.WriteLine("New Pair");
            //Print(pair);
            Simplify(pair);
            //AnsiConsole.WriteLine("Simplified");
            //Print(pair);
        }

        var final = pairs.Skip(1).Aggregate(pairs[0], (left, right) => Simplify(new Pair(left, right)));
        //AnsiConsole.WriteLine("Final");
        //Print(final);
        return GetMagnitude(final);
    }

    public async ValueTask<long> GetPart2()
    {
        var max = 0L;
        var lineCount = _lines.Length;
        var pairs = Enumerable.Range(0, lineCount).SelectMany(a => Enumerable.Range(0, lineCount).Select(b => (a, b)));
        foreach (var (a, b) in pairs)
        {
            if (a == b)
                continue;
            {
                var pairA = Simplify(Convert(_lines[a]));
                var pairB = Simplify(Convert(_lines[b]));
                var total = GetMagnitude(Simplify(new Pair(pairA, pairB)));
                if (total > max)
                    max = total;
            }
            {
                var pairA = Simplify(Convert(_lines[a]));
                var pairB = Simplify(Convert(_lines[b]));
                var total = GetMagnitude(Simplify(new Pair(pairB, pairA)));
                if (total > max)
                    max = total;
            }
        }

        return max;
    }

    private static void Print(Pair pair, int level = 0)
    {
        if (pair.left is Pair left)
            Print(left, level + 1);
        else
            AnsiConsole.WriteLine(new string('-', level) + (pair.left as Value)!.value);
        if (pair.right is Pair right)
            Print(right, level + 1);
        else
            AnsiConsole.WriteLine(new string('-', level) + (pair.right as Value)!.value);
    }

    private static long GetMagnitude(Pair pair)
    {
        var left = pair.left is Value lVal ? lVal.value : GetMagnitude((Pair)pair.left);
        var right = pair.right is Value rVal ? rVal.value : GetMagnitude((Pair)pair.right);

        return 3 * left + 2 * right;
    }

    private static Pair Simplify(Pair pair)
    {
        var didSomething = true;
        while (didSomething)
        {
            didSomething = TryExplode(pair, 1);
            if (!didSomething)
                didSomething = TrySplit(pair);
        }

        return pair;
    }

    private static bool TryExplode(Pair pair, int level)
    {
        // Explode, or keep searching
        if (level > 4 && pair.left is Value lValue && pair.right is Value rValue)
        {
            var parent = pair.Parent!;
            ExplodeLeft(lValue.value, pair.IsRight, parent);
            ExplodeRight(rValue.value, pair.IsRight, parent);
            // Then replace the exploded pair with 0
            if (pair.IsRight)
                parent.right = new Value(0) { Parent = parent, IsRight = true };
            else
                parent.left = new Value(0) { Parent = parent, IsRight = false };
            //AnsiConsole.WriteLine("Explode");
            //Print(pair.GetTopNode());
            return true;
        }

        // Keep searching down the left side, but fall back to the right side
        var subLevel = level + 1;
        return (pair.left is Pair lPair && TryExplode(lPair, subLevel)) || (pair.right is Pair rPair && TryExplode(rPair, subLevel));
    }

    private static void ExplodeRight(int num, bool isRight, Pair? parent)
    {
        // As long as the parent is on the right side, keep going up /
        if (isRight)
        {
            while (parent is { IsRight: true })
                parent = parent.Parent;
            // If no further to go, then the pair was right-most
            // Then do nothing
            if (parent?.Parent == null)
                return;
            parent = parent.Parent;
        }

        // Otherwise go down right
        // If it's a value, go ahead and modify it
        var node = parent!.right;
        if (node is Value rValue)
        {
            parent.right = rValue with { value = rValue.value + num };
            return;
        }

        // Otherwise keep down the left and modify the left-most value
        parent = node as Pair;
        while (parent!.left is Pair child)
            parent = child;

        var lValue = parent.left as Value;
        parent.left = lValue! with { value = lValue.value + num };
    }

    private static void ExplodeLeft(int num, bool isRight, Pair? parent)
    {
        // As long as the parent is on the left side, keep going up /
        if (!isRight)
        {
            while (parent is { IsRight: false })
                parent = parent.Parent;
            // If no further to go, then the pair was left-most
            // Then do nothing
            if (parent?.Parent == null)
                return;
            parent = parent.Parent;
        }

        // Otherwise go down left
        // If it's a value, go ahead and modify it
        var node = parent!.left;
        if (node is Value lValue)
        {
            parent.left = lValue! with { value = lValue.value + num };
            return;
        }

        // Otherwise keep going down right and modify the right-most value
        parent = node as Pair;
        while (parent!.right is Pair child)
            parent = child;

        var rValue = parent.right as Value;
        parent.right = rValue! with { value = rValue.value + num };
    }

    private static bool TrySplit(Pair pair)
    {
        if (pair.left is Value left)
        {
            if (left.value >= 10)
            {
                // Left is rounded down
                // Right is rounded up (so see if it's even first to see if we should add one to get the next value)
                var newLeft = left.value / 2;
                var newRight = (left.value + (left.value % 2 == 0 ? 0 : 1)) / 2;
                pair.left = new Pair(newLeft, newRight);
                pair.left.Parent = pair;
                pair.left.IsRight = false;
                //AnsiConsole.WriteLine("split");
                //Print(pair.GetTopNode());
                return true;
            }
        }
        else
        {
            var found = TrySplit((Pair)pair.left);
            if (found)
                return true;
        }

        if (pair.right is Value right)
        {
            if (right.value >= 10)
            {
                // Left is rounded down
                // Right is rounded up (so see if it's even first to see if we should add one to get the next value)
                var newLeft = right.value / 2;
                var newRight = (right.value + (right.value % 2 == 0 ? 0 : 1)) / 2;
                pair.right = new Pair(newLeft, newRight);
                pair.right.Parent = pair;
                pair.right.IsRight = true;
                //AnsiConsole.WriteLine("split");
                //Print(pair.GetTopNode());
                return true;
            }
        }
        else
        {
            var found = TrySplit((Pair)pair.right);
            if (found)
                return true;
        }

        return false;
    }

    private static Pair Convert(string line) => Convert(JsonNode.Parse(line)!.AsArray());

    private static Pair Convert(JsonArray jsonArray)
    {
        var left = Convert(jsonArray[0]!);
        var right = Convert(jsonArray[1]!);
        return new Pair(left, right);
    }

    private static INode Convert(JsonNode node)
    {
        if (node is JsonValue value)
            return new Value(value.GetValue<int>());
        return Convert(node.AsArray());
    }

    private interface INode
    {
        Pair? Parent { get; set; }
        bool IsRight { get; set; }
    }

    private record Pair : INode
    {
        public Pair(int left, int right) : this(new Value(left), new Value(right)) { }

        public Pair(INode left, INode right)
        {
            this.left = left;
            this.left.IsRight = false;
            this.left.Parent = this;

            this.right = right;
            this.right.Parent = this;
            this.right.IsRight = true;
        }

        public INode left { get; set; }
        public INode right { get; set; }
        public Pair? Parent { get; set; }
        public bool IsRight { get; set; }

        public Pair GetTopNode()
        {
            var pair = this;
            while (pair.Parent != null)
                pair = pair.Parent;
            return pair;
        }
    };

    private record Value(int value) : INode
    {
        public Pair? Parent { get; set; }
        public bool IsRight { get; set; }
    }
}
