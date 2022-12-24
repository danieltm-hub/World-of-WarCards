
using System;

namespace GameProgram
{
    public delegate double GetScore<Player>(Player player);
    public static class Scores
    {
        /*Scores € [0 , 1] */

        private static double Abs(double A) => (A < 0) ? A * -1 : A;
        private static double OnlyPositive(double A) => (A < 0) ? 0 : A;
        private static double GamePlayerFilter(Func<Player, double> FilterStats)
        {
            double total = 0;
            foreach (Player player in GameManager.CurrentGame.Players)
            {
                total += FilterStats(player);
            }
            return total;
        }

        public static double GetPlayer(Player player, Func<Player, double> FilterStats)
        {
            foreach (Player players in GameManager.CurrentGame.Players)
            {
                if (player.Name == players.Name) return FilterStats(players);
            }
            throw new Exception();
        }
        public static double MyLifeScore(Player player)
        {
            double TotalMaxLife = GamePlayerFilter((players) => ((players.Name != player.Name) ? players.MaxHealth : 0));
            double TotalLifeGame = GamePlayerFilter((players) => ((players.Name != player.Name) ? players.Health : 0));

            return 1 - (TotalLifeGame / TotalMaxLife);
        }

    }
}