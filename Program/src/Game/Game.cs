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
       
        }

        public Game Clone()
        {
            Game clone = new Game();

            foreach (Player player in Players)
            {
                clone.Players.Add(player.Clone());
            }
            clone.CurrentPlayerIndex = CurrentPlayerIndex;

            return clone;
        }
        
        public bool EqualGame(Game game)
        {
            if (CurrentPlayerIndex != game.CurrentPlayerIndex) return false;

            for (int i = 0; i < Players.Count; i++)
            {
                if (!Players[i].EqualPlayer(game.Players[i])) return false;
            }

            return true;
        }


    }
}