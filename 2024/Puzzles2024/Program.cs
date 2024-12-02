var folder = "inputs";
// folder = "examples";
var assembly = typeof(Program).Assembly;
await new Runner(assembly).Run(args, folder);
