using System.Diagnostics.CodeAnalysis;
using System.Reflection;

public class RunnerUtilities
{
    public static bool TryGetData(Assembly assembly, string folder, string day, [NotNullWhen(true)] out string[]? lines)
    {
        var inputPath = Path.Combine(Path.GetDirectoryName(assembly.Location)!, folder, $"{day}.txt");
        if (!File.Exists(inputPath))
        {
            lines = null;
            return false;
        }

        lines = File.ReadAllLines(inputPath);
        return true;
    }

    public static bool TryGetSolution(Assembly assembly, string day, string[] input, [NotNullWhen(true)] out ISolution? solution)
    {
        var type = assembly.GetType("Combined.Solutions.Solution" + day);
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
