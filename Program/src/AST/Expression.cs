using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{

    public abstract class Expression : Node
    {
        public abstract void Evaluate();
        public abstract object Value { get; set; }

        public Expression(CodeLocation location) : base(location) { }

        public override string ToString()
        {
            this.Evaluate();
            return $"{Location} => {this.Value}";
        }
    }
}