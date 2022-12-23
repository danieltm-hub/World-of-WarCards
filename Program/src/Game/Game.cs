using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProgram
{
    public class Game
    {
        public int CurrentPlayerIndex { get; set; }
        public List<Player> Players = new List<Player>();
        public Player CurrentPlayer => Players[CurrentPlayerIndex];

        public Game()
        {
            Players.Add(new Player("Player 1", 20, 20));
            Players.Add(new Player("Player 2", 20, 20));
        }
    }
}