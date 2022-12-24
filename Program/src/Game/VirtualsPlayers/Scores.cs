
using System;

namespace GameProgram
{
    public delegate double GetScore<Game, Player>(Game game, Player player);
    public static class Scores
    {
        /*Scores € [0 , 1] */
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
            return game.CurrentPlayer.Health / GamePlayerFilter(game, player => player.Health);
        }

        public static double LifeScore(Game game, Player player)
        {
            return 1 - MyLifeScore(game, player);
        }

        public static double NextPlayerLifeScore(Game game)
        {
            return game.Players[(game.CurrentPlayerIndex + 1) % game.Players.Count].Health / GamePlayerFilter(game, player => player.Health);
        }

        public static double PreviousPlayerLifeScore(Game game)
        {
            return game.Players[(game.CurrentPlayerIndex - 1) % game.Players.Count].Health / GamePlayerFilter(game, player => player.Health);
        }

        public static double MyCardsScore(Game game)
        {
            return game.CurrentPlayer.Cards.Count / GamePlayerFilter(game, player => player.Cards.Count);
        }

        public static double CardsScore(Game game)
        {
            return 1 - MyCardsScore(game);
        }
    }
}