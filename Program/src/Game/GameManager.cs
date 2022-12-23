using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProgram
{
    public static class GameManager
    {
        public static Game CurrentGame { get; private set; }

        static GameManager()
        {
            List<Player> players = new List<Player>();
            players.Add(new Player("Player 1", 100, new List<Card>()));
            players.Add(new Player("Player 2", 100, new List<Card>()));
            CurrentGame = new Game(players);
        }

        public static void ResetGame(Game game)
        {
            CurrentGame = game;
        }
    }
}