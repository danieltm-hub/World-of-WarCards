using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class Sin : UnaryExpression
    {
        public Sin(Expression argument, CodeLocation location) : base(argument, location) { Value = 0;}
        
        public override ExpressionType Type { get => ExpressionType.Number; set { } }

        public override object Value { get; set; }

        public override Func<Expression, bool> IsValid => (e) => e.Type == ExpressionType.Number;

        public override void Evaluate()
        {
            Argument.Evaluate();
            Value = Math.Sin((double)Argument.Value);
        }

    }
}