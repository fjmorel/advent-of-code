using Spectre.Console.Rendering;

namespace Shared;

internal readonly record struct StepInfo(string step, string value, TimeSpan elapsed)
{
    public IRenderable[] GetTableCells()
    {
        var ms = elapsed.TotalMilliseconds;
        var digits = ms switch
        {
            >= 1_000 => 0,
            >= 100 => 1,
            >= 10 => 2,
            _ => 3,
        };
        var text = ms.ToString("F" + digits);
        var color = ms switch
        {
            < 1 => Color.Cyan2,
            < 10 => Color.SpringGreen1,
            < 20 => Color.Green1,
            < 50 => Color.GreenYellow,
            < 100 => Color.Orange1,
            < 1_000 => Color.DarkOrange,
            _ => Color.OrangeRed1,
        };
        return
        [
            new Text(step, new Style(decoration: Decoration.Bold)),
            new Text(value),
            new Text($"{text} ms", new Style(color)),
        ];
    }
}
