
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

            MiniMax PC = new MiniMax(GameManager.CurrentGame.Players[1], Scores.LifeScore);

            while (true)
            {
                GameManager.CurrentGame.Print();
                Player? winner = GameManager.CurrentGame.Winner();

                if (winner != null)
                {
                    System.Console.WriteLine(Separator + "WAIT A MINUTE !!!!!");
                    System.Console.WriteLine($"THE WINNER IS {winner.Name}");
                    break;
                }

                Player currentPlayer = GameManager.CurrentGame.CurrentPlayer;
                System.Console.WriteLine(Separator + $"A TURN FOR {currentPlayer.Name} STARTED !!!!!");

                if (currentPlayer.Name == "You")
                {
                    currentPlayer.Print();
                    System.Console.WriteLine("Choose a card to play:");
                    currentPlayer.PlayCard(ReadCard());
                }

                else
                {
                    PC.Play();
                }

                
            }

            return;
        }

        private static Card ReadCard()
        {
            if (GameManager.CurrentGame.CurrentPlayer.Cards.Count == 0)
                throw new System.Exception("No cards to play");

            return GameManager.CurrentGame.CurrentPlayer.Cards[ReadCorrectInput()];
        }

        private static int ReadCorrectInput()
        {

            int CardIndex = -1;

            while (CardIndex == -1)
            {
                string input = System.Console.ReadLine() ?? "ERROR";
                try
                {
                    CardIndex = int.Parse(input);

                    if (CardIndex < 0 || CardIndex >= GameManager.CurrentGame.CurrentPlayer.Cards.Count)
                    {
                        System.Console.WriteLine("Please enter a valid number");
                        CardIndex = -1;
                    }
                }

                catch (System.Exception)
                {
                    System.Console.WriteLine("Please enter a number");
                }
            }
            return CardIndex;
        }

    }
}