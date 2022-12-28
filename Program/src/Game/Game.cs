using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProgram
{
    public class Game : IClonable<Game>
    {
        public int CurrentPlayerIndex { get; set; }
        public List<Player> Players = new List<Player>();
        public Player CurrentPlayer => Players[CurrentPlayerIndex];
        private IWinCondition WinCondition;

        public Game(List<Player> players, IWinCondition winCondition)
        {
            Players = players;
            WinCondition = winCondition;
        }

        public void NextTurn()
        {
            CurrentPlayer.OnTurnEnd();
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
            CurrentPlayer.OnTurnInit();
        }
        public bool IsOver()
        {
            return WinCondition.CheckWinCondition(this);
        }

        public Player Winner()
        {
            if (!IsOver()) throw new Exception("Game.Winner() : Game is not over yet");

            return WinCondition.Winner!;
        }

        public Game Clone()
        {
            List<Player> playersClone = new List<Player>();

            foreach (Player player in Players)
            {
                playersClone.Add(player.Clone());
            }

            Game gameClone = new Game(playersClone, WinCondition);

            gameClone.CurrentPlayerIndex = CurrentPlayerIndex;

            return gameClone;
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