using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class Condition : Effect
    {
        public Expression BooleanCondition { get; private set; }
        public Effect Left { get; private set; }
        public Effect? Right { get; private set; }

        public Condition(Expression booleanCondition, Effect left, Effect? right, CodeLocation location) : base(location)
        {
            BooleanCondition = booleanCondition;
            Left = left;
            Right = right;
            Type = NodeType.Effect;
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

            if (BooleanCondition.Type != NodeType.Bool)
            {
                errors.Add(new Error(ErrorCode.Invalid, Location, $"Must recieve a boolean parameter"));
                Type = NodeType.Error;
            }

            if(Left.Type != NodeType.Effect && (Right == null || Right.Type != NodeType.Effect))
            {
                errors.Add(new Error(ErrorCode.Invalid, Location, $"Must recieve an action parameter"));
                Type = NodeType.Error;
            }

            return Type != NodeType.Error && left && right && condition;
        }
    }
}