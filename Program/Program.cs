using AST;
using GameProgram;
using Compiler;

public static class Program
{
    static void Main(string[] args)
    {
        List<string> paths = new List<string>(){"./bin/Debug/net6.0/Program.dll"};

        foreach(string path in paths)
        {
            Reflection.RegisterDll(path);
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