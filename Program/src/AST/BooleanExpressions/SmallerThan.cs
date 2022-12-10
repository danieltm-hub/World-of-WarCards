using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class SmallerThan : BinaryExpression
    {
        public override Func<Expression, Expression, bool> IsValid => (left, right) => left.Type == ExpressionType.Number && right.Type == ExpressionType.Number;
        public override ExpressionType Type { get; set; }
        public override object Value { get; set; }
        public SmallerThan(Expression left, Expression right, CodeLocation location) : base(left, right, location)
        {
            Type = ExpressionType.Bool;
            Value = false;
        }
        public override void Evaluate()
        {
            base.Evaluate();
            Value = (double)Right.Value < (double)Left.Value;
        }

    }
}