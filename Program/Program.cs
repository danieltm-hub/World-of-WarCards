using System.Collections.Generic;
using AST;
using GameProgram;
using Compiler;

public static class Program
{
    static void Main(string[] args)
    {
        Registerdll();
        (List<Error> Errors, List<Token> tokens) = ReadUserInput(Path.Join("code.txt"));

        if (WarCardProgramTest(Errors, tokens)) SimulationTester.StartGameSimulation();

    }
    private static (List<Error>, List<Token>) ReadUserInput(string path)
    {
        string randomInput = File.ReadAllText(path);

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
        return (Errors, tokens);
    }
    private static bool WarCardProgramTest(List<Error> Errors, List<Token> tokens)
    {
        Parser parser = new Parser(new TokenStream(tokens));
        WarCardProgram? program = parser.ParseProgram();

        Errors = program.Errors;

        if (Errors.Count != 0)
        {
            System.Console.WriteLine("Syntax errors");
            System.Console.WriteLine(string.Join('\n', Errors));
            return false;
        }

        if (!program.CheckSemantic(Errors))
        {
            System.Console.WriteLine("Semantic errors");
            System.Console.WriteLine(string.Join('\n', Errors));
            return false;
        }

        System.Console.WriteLine(program);
        return true;
    }


    private static void Registerdll()
    {
        string dllPath = Path.Join(".", "dlls");
        string selfPath = Path.Join(".", "bin", "Debug", "net6.0", "Program.dll");

        List<string> paths = new List<string>() { selfPath };
        paths.AddRange(ReadDllFiles(dllPath));

        foreach (string path in paths)
        {
            Reflection.RegisterDll(path);
        }

    }

    private static List<string> ReadDllFiles(string path)
    {
        List<string> paths = new List<string>() { };

        foreach (string file in Directory.GetFiles(path, "*.dll"))
        {
            paths.Add(file);
        }

        return paths;
    }

}