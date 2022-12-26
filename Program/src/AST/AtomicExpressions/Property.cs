using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public abstract class Property : AtomicExpression
    {
        public Property(Player player, CodeLocation location) : base(location) 
        { 
            Type = NodeType.Error;
        }
    }
}