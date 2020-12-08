using System;
using System.Collections.Generic;
using System.Linq;

var instructions = System.IO.File.ReadAllLines("input.txt")
	.Select(x => x.Split(' '))
	.Select(x => new Instruction(x[0], int.Parse(x[1])))
	.ToList();

var toTrySwitch = new Queue<int>(instructions.Select((rule, i) => (rule, i)).Where(x => x.rule.act == "nop" || x.rule.act == "jmp").Select(x => x.i));

var ran = new HashSet<int>();
var accumulator = 0;
var index = 0;

Run(true);
Console.WriteLine(accumulator);

while (toTrySwitch.Any())
{
	var toSwitch = toTrySwitch.Dequeue();
	accumulator = 0;
	index = 0;
	ran.Clear();
	SwitchInstruction(toSwitch);
	var result = Run(false);
	if (result)
	{
		Console.WriteLine(accumulator);
		return;
	}
	else
	{
		Console.WriteLine($"Resetting {toSwitch}");
		SwitchInstruction(toSwitch);
	}
}

bool Run(bool breakOnRepeat)
{
	while (index < instructions.Count)
	{
		if (ran.Contains(index))
			return false;

		var value = instructions[index].value;
		Func<int> action = instructions[index].act switch
		{
			"acc" => () =>
			{
				accumulator += value;
				return 1;
			}
			,
			"jmp" => () => value,
			"nop" => () => 1,
			_ => throw new NotImplementedException(),
		};
		ran.Add(index);
		index += action();
	}
	return true;
}

void SwitchInstruction(int i)
{
	instructions[i] = instructions[i] with { act = instructions[i].act == "nop" ? "jmp" : "nop" };
}

record Instruction(string act, int value);
