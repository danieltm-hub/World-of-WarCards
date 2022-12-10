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
            Players.Add(new Player("Player 1"));
            Players.Add(new Player("Player 2"));
            Players.Add(new Player("Player 3"));
            Players.Add(new Player("Player 4"));
        }


    }
}