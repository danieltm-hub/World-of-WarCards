using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class And : BinaryExpression
    {
        public override Func<Expression, Expression, bool> IsValid => (left, right) => left.Type == ExpressionType.Bool && right.Type == ExpressionType.Bool;
        public override ExpressionType Type { get; set; }
        public override object Value { get; set; }
        public And(Expression left, Expression right, CodeLocation location) : base(left, right, location)
        {
            Type = ExpressionType.Bool;
            Value = 0;
        }
        public override void Evaluate()
        {
            Value = (bool)Right.Value && (bool)Left.Value;
        }

    }
}