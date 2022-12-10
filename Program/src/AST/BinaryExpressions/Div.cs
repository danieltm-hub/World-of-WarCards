using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class Div : BinaryExpression
    {
        public override Func<Expression, Expression, bool> IsValid => (left, right) => left.Type == ExpressionType.Number && right.Type == ExpressionType.Number && (double)right.Value != 0;
        public override ExpressionType Type { get; set; }
        public override object Value { get; set; }
        public Div(Expression left, Expression right, CodeLocation location) : base(left, right, location)
        {
            Type = ExpressionType.Number;
            Value = 0;
        }

        public override void Evaluate()
        {
            Value = (double)Left.Value / (double)Right.Value;
        }
    }
}