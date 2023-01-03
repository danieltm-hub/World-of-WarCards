using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public abstract class Entity : AtomicExpression, IKeyword
    {
        public abstract string Keyword { get; }
        public List<Node> Parameters { get; private set; }
        public Entity(List<Node> parameters, CodeLocation location) : base(location)
        {
            Parameters = parameters;
            Type = NodeType.Entity;
        }

        public override bool CheckSemantic(List<Error> errors)
        {
            if(Parameters.Count != 0)
            {
                errors.Add(new Error(ErrorCode.Expected, Location, $"Method recieves {0} and {Parameters.Count} where given"));
                Type = NodeType.Error;
                return false;
            }

            return true;
        }
    }
}