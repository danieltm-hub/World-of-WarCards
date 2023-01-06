using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class Next : Entity
    {
        public override object Value { get; set; }
        public override string Keyword => "other";
        public Next(List<Node> parameters, CodeLocation location) : base(parameters, location)
        {
            Value = 0;
        }

        public override void Evaluate()
        {
            Game game = GameManager.CurrentGame;
            Value = game.Players[(game.CurrentPlayerIndex + 1) % game.Players.Count];
        }

        public override bool CheckSemantic(List<Error> errors)
        {
            return true;
        }

        public override string Description => "Other";
    }
}