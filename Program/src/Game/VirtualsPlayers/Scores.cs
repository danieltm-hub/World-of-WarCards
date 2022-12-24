
using System;

namespace GameProgram
{
    public delegate double GetScore<Game, Player>(Game game, Player player);
    public static class Scores
    {
        /*Scores € [0 , 1] */

        private static double Abs(double A) => (A < 0) ? A * -1 : A;
        private static double OnlyPositive(double A) => (A < 0) ? 0 : A;
        private static double GamePlayerFilter(Game game, Func<Player, double> FilterStats)
        {
            double total = 0;
            foreach (Player player in game.Players)
            {
                total += FilterStats(player);
            }
            return total;
        }

        public static double MyLifeScore(Game game, Player player)
        {
            return player.Health / GamePlayerFilter(game, player => player.Health);
        }

        public static double LifeScore(Game game, Player player)
        {
            return 1 - MyLifeScore(game, player);
        }

        public static double NextPlayerLifeScore(Game game, Player player)
        {
            return game.Players[(game.CurrentPlayerIndex + 1) % game.Players.Count].Health / GamePlayerFilter(game, player => player.Health);
        }

        public static double PreviousPlayerLifeScore(Game game, Player player)
        {
            return game.Players[(game.CurrentPlayerIndex - 1) % game.Players.Count].Health / GamePlayerFilter(game, player => player.Health);
        }

        public static double MyCardsScore(Game game, Player player)
        {
            return player.Cards.Count / GamePlayerFilter(game, player => player.Cards.Count);
        }

        public static double CardsScore(Game game, Player player)
        {
            return 1 - MyCardsScore(game, player);
        }
    }
}