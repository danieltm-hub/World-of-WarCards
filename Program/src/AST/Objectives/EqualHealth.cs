using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class EqualHealth : Objective
    {
        public override string Keyword => "equalhealth";
        public override string Description => "players with equal health to the current player";
        public override List<NodeType> ExpectedTypes => new List<NodeType>();
        public EqualHealth(List<Node> parameters, CodeLocation location) : base(parameters, location) { }
        public override List<Player> Evaluate()
        {
            Player selected = GameManager.CurrentGame.CurrentPlayer;
            double health = selected.Health;
            List<Player> objectives = new List<Player>();


            foreach (Player player in GameManager.CurrentGame.Players)
            {
                if (health == player.Health && player.Name != player.Name)
                {
                    objectives.Add(player);
                }
            }

            return objectives;
        }
    }

}