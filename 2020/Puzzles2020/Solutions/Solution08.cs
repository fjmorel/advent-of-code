namespace Puzzles2020.Solutions;

using static Action;

public class Solution08 : ISolution
{
    private readonly List<Instruction> _instructions;

    public Solution08(string[] lines)
    {
        _instructions = lines.Select(x => x.Split(' '))
            .Select(x => new Instruction(Enum.Parse<Action>(x[0]), int.Parse(x[1])))
            .ToList();
    }

    public async ValueTask<long> GetPart1() => Run().sum;

    public async ValueTask<long> GetPart2()
    {
        var indexesToSwitch = new Queue<int>(_instructions.Select((rule, i) => (rule, i)).Where(x => x.rule.act == nop || x.rule.act == jmp).Select(x => x.i));
        while (indexesToSwitch.Any())
        {
            var toSwitch = indexesToSwitch.Dequeue();
            SwitchInstruction(toSwitch);
            var (finished, sum) = Run();
            if (finished)
            {
                return sum;
            }

            SwitchInstruction(toSwitch);
        }

        return -1;
    }

    (bool finished, int sum) Run()
    {
        var ran = new HashSet<int>();
        var sum = 0;
        var index = 0;
        while (index < _instructions.Count)
        {
            if (ran.Contains(index))
                return (false, sum);

            var (jump, add) = _instructions[index].act switch
            {
                acc => (1, _instructions[index].value),
                jmp => (_instructions[index].value, 0),
                nop => (1, 0),
                _ => throw new NotImplementedException(),
            };
            ran.Add(index);
            index += jump;
            sum += add;
        }

        return (true, sum);
    }

    void SwitchInstruction(int i)
    {
        _instructions[i] = _instructions[i] with { act = _instructions[i].act == nop ? jmp : nop };
    }

    record Instruction(Action act, int value);
}

enum Action
{
    nop,
    jmp,
    acc,
}
