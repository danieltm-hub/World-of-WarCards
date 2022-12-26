using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class SmallerThan : BinaryExpression
    {
        public override Func<Expression, Expression, bool> IsValid => (left, right) => left.Type == NodeType.Number && right.Type == NodeType.Number;
        public override NodeType Type { get; set; }
        public override string OperationSymbol => "<";
        public override object Value { get; set; }
        public SmallerThan(Expression left, Expression right, CodeLocation location) : base(left, right, location)
        {
            Type = NodeType.Bool;
            Value = false;
        }
        public override void Evaluate()
        {
            base.Evaluate();
            Value = (double)Left.Value < (double)Right.Value;
        }

    }
}