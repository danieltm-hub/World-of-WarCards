using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProgram
{
    public static class GameManager
    {
        public static Game CurrentGame = new Game(new List<Player>(), 0, new EnemyDefeated());

        public static void StartGame(List<Player> players)
        {
            CurrentGame = new Game(players, 0, new EnemyDefeated());
        }
        public static void TerminateGame()
        {
            CurrentGame = new Game(new List<Player>(), 0, new EnemyDefeated());
        }
    }
}