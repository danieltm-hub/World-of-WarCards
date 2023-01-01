using AST;
using Compiler;
using GameProgram;
using System;
using System.IO;
using System.Reflection;

public static class Program
{
    static void Main(string[] args)
    {
        // Create self path

        Assembly assembly = Assembly.LoadFrom("./bin/Debug/net6.0/Program.dll");

        Reflection.RegisterDll(assembly);

        // Create external paths

        DirectoryInfo directory = new DirectoryInfo("./Dlls");
        FileInfo[] files = directory.GetFiles("*.dll");

        foreach (FileInfo file in files)
        {
            string path = file.FullName;

            System.Console.WriteLine(path);

            assembly = Assembly.LoadFrom(path);

            Reflection.RegisterDll(assembly);
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

        System.Console.WriteLine(program.Description);
        List<Card> cards = program.Cards.Values.ToList();

        Player one = new Player("PC", 20, 3, 3, cards);
        Player two = new Player("You", 20, 3, 3, cards);

        one.SetCPU(new RandomPlayer(one));

        GameManager.StartGame(new List<Player>{one, two});

        SimulationTester.StartGameSimulation();
    }



}