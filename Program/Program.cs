using AST;
using GameProgram;
using Compiler;

public static class Program
{
    static void Main(string[] args)
    {
        string randomInput = File.ReadAllText("./code");

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

        TestEffector(Errors, tokens);
    }

    static void TestEffector(List<Error> Errors, List<Token> tokens)
    {
        Parser parser = new Parser(new TokenStream(tokens));
        Effector? effector = parser.ParseEffectorCall(out Errors);

        if (effector == null) return;

        else
        {
            effector.CheckSemantic(Errors);
        }
        foreach (Error error in Errors)
        {
            System.Console.WriteLine(error);
        }
    }

    static void TestPower(List<Error> Errors, List<Token> tokens)
    {
        Parser parsePower = new Parser(new TokenStream(tokens));
        Power? power = parsePower.ParsePowerCall(out Errors);

        foreach (Error error in Errors)
        {
            System.Console.WriteLine(error);
        }

        if (power == null)
        {
            System.Console.WriteLine("Returning null");
        }
        else if (!power.CheckSemantic(Errors))
        {
            System.Console.WriteLine("Check semantic invalid");
        }
        else
        {
            System.Console.WriteLine("Power is valid");
            power.Evaluate(new List<Player>());
        }
    }

}