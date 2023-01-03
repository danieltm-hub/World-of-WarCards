using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class Equal : BinaryExpression
    {
        public override Func<Expression, Expression, bool> IsValid => (left, right) => CheckValid(left, right);
        public override NodeType Type { get; set; }
        public override string OperationSymbol => "==";
        public override object Value { get; set; }
        public Equal(Expression left, Expression right, CodeLocation location) : base(left, right, location)
        {
            Type = NodeType.Bool;
            Value = false;
        }
        public override void Evaluate()
        {
            base.Evaluate();

            if(Right.Type == NodeType.Number)
            {
                Value = (double)Right.Value == (double)Left.Value;
            }
            else
            {
                Value = (string)Right.Value == (string)Left.Value;
            }
        }

        private bool CheckValid(Expression left, Expression right)
        {
            if (left.Type == NodeType.Number && right.Type == NodeType.Number)
            {
                return true;
            }
            
            if (left.Type == NodeType.Text && right.Type == NodeType.Text)
            {
                return true;
            }
            
            return false;
        }
    }
}