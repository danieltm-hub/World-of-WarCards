using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProgram
{
    public class EnemyDefeated : IWinCondition
    {
        public Player? GameWinner { get; private set; }
        public Player? Winner => GameWinner;
        public bool CheckWinCondition(Game game)
        {
            if (game.Players[0].Health <= 0)
            {
                GameWinner = game.Players[1];
                return true;
            }
            
            else if (game.Players[1].Health <= 0)
            {
                GameWinner = game.Players[0];
                return true;
            }
            
            return false;
        }
    }
}