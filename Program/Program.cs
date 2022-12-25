using AST;
using GameProgram;
using Compiler;
using System.Reflection;

public static class Program
{
    static void Main(string[] args)
    {
        // Create self path

        Assembly assembly = Assembly.LoadFrom("./bin/Debug/net6.0/Program.dll");
        Type[] types = assembly.GetExportedTypes();

        // Create external paths

        List<string> paths = new List<string>(){"Program.dll"};

        foreach(string path in paths)
        {
            assembly = Assembly.LoadFile(path);
            types = assembly.GetExportedTypes();

            Reflection.RegisterDll(types);
        }

        string randomInput = File.ReadAllText("./code.txt");

        List<Error> Errors = new List<Error>();
        List<Token> tokens = Analyzer.GetTokens("code", randomInput, Errors);

        foreach (var token in tokens)
        {
            Console.WriteLine(token.ToString());
        }
        foreach (var error in Errors)
        {
            Console.WriteLine(error.ToString());
        }

        WarCardProgramTest(Errors, tokens);
    }

    static void WarCardProgramTest(List<Error> Errors, List<Token> tokens)
    {
        Parser parser = new Parser(new TokenStream(tokens));
        WarCardProgram? program = parser.ParseProgram();

        Errors = program.Errors;

        if (Errors.Count != 0)
        {
            System.Console.WriteLine("Syntax errors");
            System.Console.WriteLine(string.Join('\n', Errors));
            return;
        }

        if (!program.CheckSemantic(Errors))
        {
            System.Console.WriteLine("Semantic errors");
            System.Console.WriteLine(string.Join('\n', Errors));
            return;
        }

        System.Console.WriteLine(program);
    }

}