using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AST
{
    public class Not : UnaryExpression
    {
        public Not(Expression argument, CodeLocation location) : base(argument, location) { Value = false; }
        
        public override ExpressionType Type { get => ExpressionType.Bool; set { } }

        public override object Value { get; set; }

        public override Func<Expression, bool> IsValid => (e) => e.Type == ExpressionType.Bool;

        public override void Evaluate()
        {
            Argument.Evaluate();
            Value = !(bool)Argument.Value;
        }

    }
}