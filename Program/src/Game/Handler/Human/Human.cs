using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AST;

namespace GameProgram
{
    public class Human : Handler
    {
        public Human(Player player) : base(player) { }
        
        public override List<int> GetCards()
        {
            Game initialGame = GameManager.CurrentGame;

            GameManager.CurrentGame = initialGame.Clone();

            List<int> toPlay = HumanController();

            GameManager.CurrentGame = initialGame;

            return toPlay;

        }

        public List<int> HumanController()
        {
            List<int> cardtoPlay = new List<int>();
            return cardtoPlay;
        }
    }
}