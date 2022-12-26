namespace AST
{
    public class Pow : BinaryExpression
    {
        public override Func<Expression, Expression, bool> IsValid => (left, right) => left.Type == NodeType.Number && right.Type == NodeType.Number && ((double)right.Value != 0 || (double)left.Value != 0);
        public override NodeType Type { get; set; }
        public override string OperationSymbol => "^";
        public override object Value { get; set; }
        public Pow(Expression left, Expression right, CodeLocation location) : base(left, right, location)
        {
            Type = NodeType.Number;
            Value = 0;
        }
        public override void Evaluate()
        {
            base.Evaluate();
            Value = System.Math.Pow((double)Left.Value, (double)Right.Value);
        }
    }
}