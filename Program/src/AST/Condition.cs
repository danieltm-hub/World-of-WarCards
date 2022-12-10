using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class Condition : Action
    {
        public Expression BooleanCondition { get; private set; }
        public Action Left { get; private set; }
        public Action? Right { get; private set; }


        public Condition(Expression booleanCondition, Action left, Action? right, CodeLocation location) : base(location)
        {
            BooleanCondition = booleanCondition;
            Left = left;
            Right = right;
        }

        public override void Evaluate()
        {
            BooleanCondition.Evaluate();

            if ((bool)BooleanCondition.Value) Left.Evaluate();

            else if (Right != null) Right.Evaluate();
        }

        public override bool CheckSemantic(List<Error> errors)
        {
            bool left = Left.CheckSemantic(errors);
            bool right = (Right == null) ? true : Right.CheckSemantic(errors);
            bool condition = BooleanCondition.CheckSemantic(errors);

            if (BooleanCondition.Type != ExpressionType.Bool)
            {
                errors.Add(new Error(ErrorCode.Invalid, Location, $"Must recieve a boolean parameter"));

                return false;
            }

            return left && right && condition;
        }
    }
}