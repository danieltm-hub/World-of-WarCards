using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{

    public abstract class Expression : Node
    {
        public abstract void Evaluate();
        public virtual NodeType Type { get; set; }
        public abstract object Value { get; set; }

        public Expression(CodeLocation location) : base(location) { }
    }
}