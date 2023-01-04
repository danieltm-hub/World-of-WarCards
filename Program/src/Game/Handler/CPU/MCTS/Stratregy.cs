using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST;

namespace GameProgram
{
    public delegate double GetScore<Game, Player>(Game gameState, Player player);

    public static class BasicStratergy
    {
        public static double BasicLifeLScore(Game gameState, Player player)
        {
            double score = gameState.Players[0].Health - gameState.Players[1].Health;
            return (gameState.Players[0].Name == player.Name) ? score : -score;
        }
    }
}