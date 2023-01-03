using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public abstract class UnaryExpression : Expression
    {
        public Expression Argument { get; private set; }
        public abstract Func<Expression, bool> IsValid { get; }
        public abstract string OperationSymbol { get; }
        public override string Description => $"{OperationSymbol} ( {Argument.Description} )";

        public UnaryExpression(Expression argumment, CodeLocation location) : base(location)
        {
            Argument = argumment;
        }

        public override bool CheckSemantic(List<Error> errors)
        {
            bool argument = Argument.CheckSemantic(errors);
            if (!IsValid(Argument) || !argument)
            {
                errors.Add(new Error(ErrorCode.Invalid, Location, $"Expected {Type} in expression"));
                Type = NodeType.Error;
                return false;
            }
            return argument;
        }

    }
}