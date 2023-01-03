using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class Effector : Effect
    {
        public List<Objective> Objectives { get; private set; }

        public List<Power> Powers { get; private set; }

        public Effector(List<Objective> objectives, List<Power> powers, CodeLocation location) : base(location)
        {
            Objectives = objectives;
            Powers = powers;
            Type = NodeType.Effect;
        }

        public override string Description => GetDescription();

        public override bool CheckSemantic(List<Error> errors)
        {
            bool objectivesBoolean = true;
            bool powersBoolean = true;
            
            foreach (var objective in Objectives)
            {
                objectivesBoolean &= objective.CheckSemantic(errors);
                if (objective.Type != NodeType.Objective)
                {
                    errors.Add(new Error(ErrorCode.Invalid, Location, $"Expected Objectives as first argument"));
                    Type = NodeType.Error;
                }
            }

            foreach (var power in Powers)
            {
                powersBoolean &= power.CheckSemantic(errors);
                if (power.Type != NodeType.Power)
                {
                    errors.Add(new Error(ErrorCode.Invalid, Location, $"Expected Powers as second argument"));
                    Type = NodeType.Error;
                }
            }

            return objectivesBoolean && powersBoolean && Type != NodeType.Error;
        }

        public override void Evaluate()
        {
            List<Player> players = new List<Player>();

            foreach (var objective in Objectives)
            {
                players.AddRange(objective.Evaluate());
            }

            foreach (var power in Powers)
            {
                power.Evaluate(players);
            }
        }

        private string GetDescription()
        {
            string description = "";

            List<string> powers = new List<string>();

            foreach (var power in Powers)
            {
                powers.Add(power.Description);
            }

            description += String.Join(", ", powers);

            description += " of ";

            List<string> objectives = new List<string>();

            foreach (var objective in Objectives)
            {
                objectives.Add(objective.Description);
            }

            description += String.Join(", ", objectives);

            return description;
        }
    }
}