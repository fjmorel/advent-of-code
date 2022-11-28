#if USE_EXAMPLE
const string folder = "examples";
#else
const string folder = "inputs";
#endif

var assembly = typeof(Program).Assembly;
await new Runner(assembly).Run(args, folder);
