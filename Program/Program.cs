using AST;

static class Program
{
    static void Main(string[] args)
    {

        Expression Sum = new Add(new Number(1, new CodeLocation()), new Number(2, new CodeLocation()), new CodeLocation());
        Expression Div = new Div(new Number(1, new CodeLocation()), new Number(2, new CodeLocation()), new CodeLocation());
        //System.Console.WriteLine(Div.CheckSemantic(new List<Error>()));
        Expression Greater = new GreaterThan(Sum, Div, new CodeLocation());
        Expression Smaller = new SmallerThan(Sum, Div, new CodeLocation());

        Expression Addcorner = new Add(Sum, Sum, new CodeLocation());

        Greater.Evaluate();
        Smaller.Evaluate();

        Expression AndExpression = new And(Greater, Smaller, new CodeLocation());
        Expression OrExpression = new Or(Greater, Smaller, new CodeLocation());

        AndExpression.Evaluate();
        OrExpression.Evaluate();

        Console.WriteLine($"{Greater.Value} {Smaller.Value} {AndExpression.Value} {OrExpression.Value}");
    }
}
