using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class Number : AtomicExpression
    {
        public override NodeType Type { get => NodeType.Number; set { } }
        public override string Description { get => (Value == null) ? " " : ((double)Value).ToString(); }
        public override object Value { get; set; }
        public override void Evaluate(){}
        public Number(double value, CodeLocation location) : base(location)
        {
            Value = value;
        }
    }
}