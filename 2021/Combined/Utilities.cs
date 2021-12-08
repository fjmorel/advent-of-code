using System.Diagnostics.CodeAnalysis;

namespace Combined;

public class Utilities
{
    /// <summary>
    /// Current folder of Combined assembly, to find the input files
    /// </summary>
    private static string _root = Path.GetDirectoryName(typeof(Utilities).Assembly.Location)!;

    public static bool TryGetData(string folder, string day, [NotNullWhen(true)] out string[]? lines)
    {
        var inputPath = Path.Combine(_root, folder, $"{day}.txt");
        if (!File.Exists(inputPath))
        {
            lines = null;
            return false;
        }
        lines = File.ReadAllLines(inputPath);
        return true;
    }
    public static bool TryGetSolution(string day, string[] input, [NotNullWhen(true)] out ISolution? solution)
    {
        var type = typeof(ISolution).Assembly.GetType("Combined.Solutions.Solution" + day);
        if (type == null)
        {
            solution = null;
            return false;
        }

        var obj = Activator.CreateInstance(type, new object[] { input });
        if (obj is ISolution job)
        {
            solution = job;
            return true;
        }
        else
        {
            solution = null;
            return false;
        }
    }
}
