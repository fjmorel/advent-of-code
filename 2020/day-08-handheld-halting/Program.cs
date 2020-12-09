using System;
using System.Collections.Generic;
using System.Linq;
using static Action;

var instructions = System.IO.File.ReadAllLines("input.txt")
	.Select(x => x.Split(' '))
	.Select(x => new Instruction(Enum.Parse<Action>(x[0]), int.Parse(x[1])))
	.ToList();

Console.WriteLine(Run().sum);

var indexesToSwitch = new Queue<int>(instructions.Select((rule, i) => (rule, i)).Where(x => x.rule.act == nop || x.rule.act == jmp).Select(x => x.i));
while (indexesToSwitch.Any())
{
	var toSwitch = indexesToSwitch.Dequeue();
	SwitchInstruction(toSwitch);
	var (finished, sum) = Run();
	if (finished)
	{
		Console.WriteLine(sum);
		break;
	}
	SwitchInstruction(toSwitch);
}

(bool finished, int sum) Run()
{
	var ran = new HashSet<int>();
	var sum = 0;
	var index = 0;
	while (index < instructions.Count)
	{
		if (ran.Contains(index))
			return (false, sum);

		var (jump, add) = instructions[index].act switch
		{
			acc => (1, instructions[index].value),
			jmp => (instructions[index].value, 0),
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
	instructions[i] = instructions[i] with { act = instructions[i].act == nop ? jmp : nop };
}

record Instruction(Action act, int value);

enum Action
{
	nop,
	jmp,
	acc,
}