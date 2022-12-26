using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class Mult : BinaryExpression
    {
        public override NodeType Type {get; set;}
        public override string OperationSymbol => "*";
        public override object Value {get; set;}
        public override Func<Expression, Expression, bool> IsValid => (left, right) => left.Type == NodeType.Number && right.Type == NodeType.Number;
        public Mult(Expression left, Expression right, CodeLocation location) : base(left, right, location)
        {
            Type = NodeType.Number;
            Value = 0;
        }
        public override void Evaluate()
        {
            base.Evaluate();
            Value = (double)Right.Value * (double)Left.Value;
        }
    }
}