using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public abstract class Entity : AtomicExpression
    {
        public Entity(CodeLocation location) : base(location) 
        { 
            Type = NodeType.Entity;
        }
    }
}