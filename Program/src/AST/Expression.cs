using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public enum ExpressionType
    {
        Text,
        Number,
        Error,
        Bool,
    }

    public abstract class Expression : Node
    {
        public abstract void Evaluate();
        public virtual ExpressionType Type { get; set; }
        public abstract object Value { get; set; }

        public Expression(CodeLocation location) : base(location) { }
    }
}