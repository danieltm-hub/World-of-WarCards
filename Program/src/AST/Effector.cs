using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class Effector : Action
    {
        public List<Objective> Objectives { get; private set; }

        public List<Power> Powers { get; private set; }

        public override NodeType Type { get => NodeType.Action; set { } }

        public Effector(List<Objective> objectives, List<Power> powers, CodeLocation location) : base(location) { }
        

        public override bool CheckSemantic(List<Error> errors)
        {
            bool objectivesBoolean = Objective.CheckSemantic(errors);
            bool powersBoolean = Power.CheckSemantic(errors);

            if( Objective.Type != NodeType.Objective)
            {
                errors.Add(new Error(ErrorCode.Invalid, Location, $"Expected Objectives as first argument"));
                Type = NodeType.Error;
            }

            if (Power.Type != NodeType.Power)
            {
                errors.Add(new Error(ErrorCode.Invalid, Location, $"Expected Powers as second argument"));
                Type = NodeType.Error;
            }

            return objectivesBoolean && powersBoolean && Type != NodeType.Error; 
        }

        public override void Evaluate()
        {
            List<Player> players = new List<Player>();
            
            foreach (var objective in Objectives)
            {
                players.AddRange(objective.GetPlayers(game));
            }
        }
    }
}