using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProgram
{
    public class Game
    {
        public int CurrentPlayerIndex { get; private set; }
        public List<Player> Players { get; private set; } = new List<Player>();
        public Player CurrentPlayer => Players[CurrentPlayerIndex];

        public Game(List<Player> players, int currentPlayerIndex = 0)
        {
            Players = players;
            CurrentPlayerIndex = currentPlayerIndex;
        }

        public void NextPlayer()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
            //UpdateGame();
        }

        private void UpdateGame() //Optional
        {
            Players = Players.FindAll(player => player.Health > 0);
        }
        public Player? Winner()
        {
            List<Player> Survivers = Players.FindAll(player => player.Health > 0);
            return (Survivers.Count == 1) ? Survivers[0] : null;
        }

        public Game Clone()
        {
            List<Player> playersClone = new List<Player>();

            foreach (Player player in Players)
            {
                playersClone.Add(player.Clone());
            }

            return new Game(playersClone, CurrentPlayerIndex);
        }

        public void Print()
        {
            Console.WriteLine(ToString());
        }
        public void PrintAll()
        {
            foreach (Player player in Players)
            {
                Console.WriteLine(player.ToString());
            }
        }

        public override string ToString()
        {
            string str = "Current Player: " + CurrentPlayer.Name + "\n";

            foreach (Player player in Players)
            {
                str += player.Name + " => Health: " + player.Health + "\n";
            }
            return str;
        }

        public bool EqualGame(Game other)
        {
            if (this.Players.Count != other.Players.Count) return false;
            if (this.CurrentPlayerIndex != other.CurrentPlayerIndex) return false;
            if (this.CurrentPlayer.Name != other.CurrentPlayer.Name) return false;

            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].Name != other.Players[i].Name) return false;
                if (Players[i].Health != other.Players[i].Health) return false;
            }

            return true;
        }
    }
}