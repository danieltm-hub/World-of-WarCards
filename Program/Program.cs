using AST;

static class Program
{
    static void Main(string[] args)
    {
        Expression Sum = new Add(new Number(1, new CodeLocation()), new Number(2, new CodeLocation()), new CodeLocation());
        Sum.Evaluate();
        Console.WriteLine(Sum.Value);

        Expression Div = new Div(new Number(1, new CodeLocation()), new Number(2, new CodeLocation()), new CodeLocation());
        
        //System.Console.WriteLine(Div.CheckSemantic(new List<Error>()));
        
        Div.Evaluate();
        System.Console.WriteLine(Div.Value);
    }
}