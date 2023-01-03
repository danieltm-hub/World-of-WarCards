using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public abstract class Effect : Node
    {   
        public Effect(CodeLocation location) : base(location) { }

        public abstract void Evaluate();
    }
}