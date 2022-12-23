using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{

    public class NextPlayer : Objective
    {
        public override string Keyword() => "nextplayer";
        public override List<NodeType> ExpectedTypes => new List<NodeType>();
        public NextPlayer(List<Expression> parameters, CodeLocation location) : base(parameters, location) { }
        public override List<Player> Evaluate()
        {
            Game game = GameManager.CurrentGame;
            return new List<Player>() { game.Players[(game.CurrentPlayerIndex + 1) % game.Players.Count] };
        }
    }

}