using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Visual;

namespace GameProgram
{
    public class Game : IClonable<Game>
    {
        public List<Player> Players = new List<Player>();
        public Player CurrentPlayer => Players[CurrentPlayerIndex];
        public int CurrentPlayerIndex { get; private set; }
        public IWinCondition WinCondition;

        public Game(List<Player> players, int currentPlayerIndex, IWinCondition winCondition)
        {
            Players = players;
            CurrentPlayerIndex = currentPlayerIndex;
            WinCondition = winCondition;
        }

        public Game Clone()
        {
            List<Player> players = new List<Player>();

            foreach (Player player in Players)
            {
                players.Add(player.Clone());
            }

            return new Game(players, CurrentPlayerIndex, WinCondition);
        }

        public void NextTurn(bool print = true)
        {
            CurrentPlayer.OnTurnEndStates.ForEach(state => state.Evaluate());
            ReduceCooldown();
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
            CurrentPlayer.OnTurnInitStates.ForEach(state => state.Evaluate());
            CurrentPlayer.ResetEnergy();
            CurrentPlayer.FillWill();
        }

        public void PlayCard(int cardIndex, bool print = true)
        {
            if (!CurrentPlayer.PlayCard(cardIndex, print)) return;
            ReduceCooldown();
        }

        public bool IsOver()
        {
            return WinCondition.CheckWinCondition(this);
        }

        public Player Winner()
        {
            if (WinCondition.Winner == null) throw new Exception("There is no current winner");

            return WinCondition.Winner;
        }

        public void ReduceCooldown()
        {
            CurrentPlayer.ReduceCooldown();
        }

        public bool IsSameGame(Game game)
        {
            if (Players.Count != game.Players.Count) return false;

            if (CurrentPlayerIndex != game.CurrentPlayerIndex) return false;

            for (int i = 0; i < Players.Count; i++)
            {
                if (!Players[i].IsSamePlayer(game.Players[i])) return false;
            }

            return true;
        }
    }
}