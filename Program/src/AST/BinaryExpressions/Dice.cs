using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class Dice : BinaryExpression
    {
        public override Func<Expression, Expression, bool> IsValid => (left, right) => left.Type == NodeType.Number && right.Type == NodeType.Number;
        public override NodeType Type { get; set; }
        public override string OperationSymbol => "d";
        public override object Value { get; set; }
        public Dice(Expression left, Expression right, CodeLocation location) : base(left, right, location)
        {
            Type = NodeType.Number;
            Value = 0;
        }
        public override void Evaluate()
        {
            base.Evaluate();
            double right = (double)Right.Value;
            double left = (double)Left.Value;
            Random random = new Random();

            Value = random.NextDouble() % (right - left) + left;
        }
    }
}