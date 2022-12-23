
using System;

namespace GameProgram
{
    public delegate double GetScore<Game>(Game game);
    public static class Scores
    {
        /* score € [0 , 1]   */
        private static double TotalLife(Game game)
        {
            double total = 0;
            foreach (var player in game.Players)
            {
                total += player.Health;
            }
            //if(total == 0)throw new Exception("total life is 0");

            return total;
        }

        private static double TotalCards(Game game)
        {
            double total = 0;
            foreach (var player in game.Players)
            {
                total += player.Cards.Count;
            }
            //if(total == 0)throw new Exception("total cards is 0");

            return total;
        }
        public static double MyLifeScore(Game game)
        {
            return game.CurrentPlayer.Health / TotalLife(game);
        }

        public static double LifeScore(Game game)
        {
            return 1 - MyLifeScore(game);
        }

        public static double NextPlayerLifeScore(Game game)
        {
            return game.Players[(game.CurrentPlayerIndex + 1) % game.Players.Count].Health / TotalLife(game);
        }

        public static double PreviousPlayerLifeScore(Game game)
        {
            return game.Players[(game.CurrentPlayerIndex - 1) % game.Players.Count].Health / TotalLife(game);
        }

        public static double MyCardsScore(Game game)
        {
            return game.CurrentPlayer.Cards.Count / TotalCards(game);
        }

    }
}