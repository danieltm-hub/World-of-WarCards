using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class ApplyInitState : Power
    {
        public override string Keyword => "initstate";
        public override string Description { get => GetDescription(); }
        public override List<NodeType> ExpectedTypes => new List<NodeType> {NodeType.Effect, NodeType.Number};
        
        public ApplyInitState(List<Node> parameters, CodeLocation location) : base(parameters, location) { }

        public override void Evaluate(IEnumerable<Player> players)
        {
            foreach(Player player in players)
            {
                Effect effect = (Effect)Parameters[0];
                
                Expression duration = (Expression)Parameters[1];
                duration.Evaluate();

                player.AddTurnInitState(new State(effect, (double)duration.Value));
            }
        }

        private string GetDescription()
        {
            Effect effect = (Effect)Parameters[0];
            Expression duration = (Expression)Parameters[1];
            duration.Evaluate();
            
            return $"At the start of turn {effect.Description} for {duration.Value} turns";
        }
    }
}