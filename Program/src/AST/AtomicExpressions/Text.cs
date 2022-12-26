using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class Text : AtomicExpression
    {
        public override NodeType Type { get => NodeType.Text; set { } }
        public override string Description => (Value == null) ? " " : ((string)Value);
        public override object Value { get; set; }
        public override void Evaluate(){}
        public Text(string value, CodeLocation location) : base(location)
        {
            Value = value;
        }
    }
}