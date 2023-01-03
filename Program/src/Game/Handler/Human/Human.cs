using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AST;

namespace GameProgram
{
    public class Human : Handler
    {
        public Human(Player player) : base(player) { }
        public override HashSet<string> GetCards()
        {
            Game initialGame = GameManager.CurrentGame;
           
            GameManager.CurrentGame = initialGame.Clone();

            HashSet<string> toPlay = HumanController();

            GameManager.CurrentGame = initialGame;

            return toPlay;

        }

        public HashSet<string> HumanController()
        {
            HashSet<string> cardtoPlay = new HashSet<string>();  
            return cardtoPlay;
        }

        
    }

}