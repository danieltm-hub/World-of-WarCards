using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public abstract class BinaryExpression : Expression
    {
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }

        public abstract Func<Expression, Expression, bool> IsValid { get; }


        public BinaryExpression(Expression left, Expression right, CodeLocation location) : base(location)
        {
            Left = left;
            Right = right;
        }
        public override void Evaluate()
        {
            Right.Evaluate();
            Left.Evaluate();
        }

        public override bool CheckSemantic(List<Error> errors)
        {
            bool right = Right.CheckSemantic(errors);
            bool left = Left.CheckSemantic(errors);

            if (!IsValid(Left, Right))
            {
                errors.Add(new Error(ErrorCode.Invalid, Location, $"Expected {Type} in expression"));
                Type = ExpressionType.Error;
                return false;
            }

            return right && left;
        }
    }
}