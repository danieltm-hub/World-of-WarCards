using AST;
using GameProgram;
using Compiler;

public static class Program
{
    static void Main(string[] args)
    {
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
        Parser parser = new Parser(new TokenStream (tokens));
        WarCardProgram? program = parser.ParseProgram();

        if(program == null) return;

        if(!program.CheckSemantic(Errors)) 
        {
            foreach(Error error in Errors)
            {
                System.Console.WriteLine(error);
            }
            return;
        }

        System.Console.WriteLine(program);
    }

}