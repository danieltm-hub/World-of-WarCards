using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class Div : BinaryExpression
    {
        public override Func<Expression, Expression, bool> IsValid => (left, right) => left.Type == NodeType.Number && right.Type == NodeType.Number && (double)right.Value != 0;
        public override NodeType Type { get; set; }
        public override object Value { get; set; }
        public Div(Expression left, Expression right, CodeLocation location) : base(left, right, location)
        {
            Type = NodeType.Number;
            Value = 0;
        }

        public override void Evaluate()
        {
            base.Evaluate();
            Value = (double)Left.Value / (double)Right.Value;
        }
    }
}