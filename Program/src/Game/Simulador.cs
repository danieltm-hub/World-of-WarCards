using System.Threading;
using AST;
namespace GameProgram
{
    public static class SimulationTester
    {
        public static void StartGameSimulation()
        {
            string Separator = "\n" + new string('#', 100) + "\n";
            string turnSeparator = "\n" + new string('-', 100) + "\n";

            System.Console.WriteLine(Separator);
            System.Console.WriteLine("WARNING A GAME STARTED !!!!!");

            // MiniMax PC = new MiniMax(GameManager.CurrentGame.Players[1], Scores.MyLifeScore);

            while (true)
            {
                PrintCurrentGame();

                System.Console.WriteLine();
                
                Thread.SpinWait(1000);

                if (GameManager.CurrentGame.IsOver())
                {
                    System.Console.WriteLine(Separator + "WAIT A MINUTE !!!!!");
                    System.Console.WriteLine($"THE WINNER IS {GameManager.CurrentGame.Winner().Name}");
                    break;
                }

                Player currentPlayer = GameManager.CurrentGame.CurrentPlayer;

                System.Console.WriteLine($"A TURN FOR {currentPlayer.Name} STARTED !!!!!\n");

                currentPlayer.Controller.Play();

                System.Console.WriteLine(turnSeparator);

                Thread.SpinWait(1000);
            }

            return;
        }

        public static void PrintCurrentGame()
        {
            foreach (Player player in GameManager.CurrentGame.Players)
            {
                PrintPlayer(player);
                System.Console.WriteLine();
            }

        }

        public static void PrintPlayer(Player toprint)
        {
            System.Console.WriteLine($"Player {toprint.Name} has {toprint.Health} life and {toprint.Energy} energy");
            System.Console.WriteLine("Cards:");

            for (int i = 0; i < toprint.Cards.Count; i++)
            {
                System.Console.Write($"{i} -> ");
                PrintCard(toprint.Cards[i]);
                System.Console.WriteLine();
            }

        }

        public static void PrintCard(Card toprint)
        {
            System.Console.Write($"{toprint.Name} [{toprint.EnergyCostValue} EnergyCost] [{toprint.ColdownValue} Coldown]");
        }

        public static void PrintCardDecription(Card toprint)
        {
            System.Console.WriteLine($"{toprint.Name} [{toprint.EnergyCostValue} EnergyCost] [{toprint.ColdownValue} Coldown]");
            System.Console.WriteLine($"Description: {toprint.Description}");
        }
    }
}