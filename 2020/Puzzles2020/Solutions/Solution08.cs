namespace Puzzles2020.Solutions;

public record Solution08(List<Solution08.Instruction> _instructions) : ISolution<Solution08>
{
    public static Solution08 Init(string[] lines)
    {
        var instructions = lines.Select(x => x.Split(' '))
            .Select(x => new Instruction(Enum.Parse<Action>(x[0]), int.Parse(x[1])))
            .ToList();
        return new(instructions);
    }

    public async ValueTask<long> GetPart1() => Run().sum;

    public async ValueTask<long> GetPart2()
    {
        var indexesToSwitch = new Queue<int>(_instructions.Select((rule, i) => (rule, i)).Where(x => x.rule.act is Action.nop or Action.jmp).Select(x => x.i));
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

    private (bool finished, int sum) Run()
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
                Action.acc => (1, _instructions[index].value),
                Action.jmp => (_instructions[index].value, 0),
                Action.nop => (1, 0),
                _ => throw new NotSupportedException(),
            };
            ran.Add(index);
            index += jump;
            sum += add;
        }

        return (true, sum);
    }

    private void SwitchInstruction(int i)
    {
        _instructions[i] = _instructions[i] with { act = _instructions[i].act == Action.nop ? Action.jmp : Action.nop };
    }

    public record Instruction(Action act, int value);

    public enum Action
    {
        nop,
        jmp,
        acc,
    }
}
