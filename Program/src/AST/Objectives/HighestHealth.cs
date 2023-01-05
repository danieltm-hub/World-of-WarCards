using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class HighestHealth : Objective
    {
        public override string Keyword => "highesthealth";
        public override string Description => "player with the highest health";
        public override List<NodeType> ExpectedTypes => new List<NodeType>();
        public HighestHealth(List<Node> parameters, CodeLocation location) : base(parameters, location) { }
        public override List<Player> Evaluate()
        {
            Player selected = GameManager.CurrentGame.Players[0];
            double health = double.MinValue;

            bool twoPlayerwithSameLife = false;

            foreach (Player player in GameManager.CurrentGame.Players)
            {
                if (health < player.Health)
                {
                    health = player.Health;
                    selected = player;
                    twoPlayerwithSameLife = false;
                }

                else if (health == player.Health)
                {
                    twoPlayerwithSameLife = true;
                }
            }

            return (twoPlayerwithSameLife) ? new List<Player>() : new List<Player> { selected };
        }
    }

}