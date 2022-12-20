using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public abstract class Objective : Node
    {
        public abstract string Keyword();
        public List<Expression> Parameters {get; private set;}
        public abstract List<NodeType> ExpectedTypes { get; }
        public Objective(List<Expression> parameters, CodeLocation location) : base(location) 
        { 
            Parameters = parameters;
            Type = NodeType.Objective;
        }
        
        public abstract List<Player> Evaluate();

        public override bool CheckSemantic(List<Error> errors)
        {
            if (Parameters.Count != ExpectedTypes.Count)
            {
                errors.Add(new Error(ErrorCode.Expected, Location, $"Method recieves {ExpectedTypes.Count} and {Parameters.Count} where given"));
                Type = NodeType.Error;
                return false;
            }

            bool flag = true;
            for(int i=0; i<Parameters.Count; i++)
            {
                Parameters[i].CheckSemantic(errors);
                
                if(Parameters[i].Type != ExpectedTypes[i])
                {
                    Type = NodeType.Error;
                    flag = false;
                    errors.Add(new Error(ErrorCode.Expected, Location, $"Method recieves {ExpectedTypes[i]} and {Parameters[i].Type} was given as {i} argument"));
                }
            }

            return flag;
        }
    }

}