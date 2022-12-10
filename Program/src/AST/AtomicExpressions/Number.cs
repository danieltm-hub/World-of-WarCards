using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class Number : AtomicExpression
    {
        public override ExpressionType Type {get => ExpressionType.Number; set{}}
        public override object Value {get; set;}
        public override void Evaluate(){}

        public Number(double value, CodeLocation location) : base(location)
        {
            Value = value;
        }
    }
}