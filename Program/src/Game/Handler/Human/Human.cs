using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AST;

namespace GameProgram
{
    public class Human : Handler
    {
        public Human(Player player) : base(player) { }
        public override HashSet<string> GetCards()
        {
            Game initialGame = GameManager.CurrentGame;
            GameManager.CurrentGame = initialGame.Clone();

            HashSet<string> toPlay = HumanController();

            GameManager.CurrentGame = initialGame;

            return new HashSet<string>();

        }

        public HashSet<string> HumanController()
        {
            HashSet<string> cardtoPlay = new HashSet<string>();

            while (true)
            {
                Player currentPlayer = GameManager.CurrentGame.CurrentPlayer;

                System.Console.WriteLine("Do you want to see the description of a card? [press 1]");
                System.Console.WriteLine("Do you want play a card? [press 2]");
                System.Console.WriteLine("Do you wnat pass? [press 3]");

                int answer = ReadCorrectInput(1, 3);

                if (answer == 1)
                {
                    System.Console.WriteLine("Which card?");
                    SimulationTester.PrintCardDecription(currentPlayer.Cards
                                    [ReadCorrectInput(1, 3)]);
                    continue;
                }

                if (answer == 2)
                {
                    System.Console.WriteLine("Choose a card to play:");
                    Card? card = ReadCard();

                    if (card != null)
                    {
                        GameManager.CurrentGame.PlayCard(card);
                        cardtoPlay.Add(card.Name);
                    }
                }

                if (answer == 3)
                {
                    break;
                }

                SimulationTester.PrintCurrentGame();
            }

            return cardtoPlay;
        }


        public Card? ReadCard()
        {
            if (AvailableCards().Count == 0)
            {
                System.Console.WriteLine("No cards to play");
                return null;
            }

            int answer = -1;
            Player currentPlayer = GameManager.CurrentGame.CurrentPlayer;

            while (answer == -1)
            {
                answer = ReadCorrectInput(0, currentPlayer.Cards.Count - 1);
                if (!currentPlayer.CanPlay(currentPlayer.Cards[answer]))
                {
                    answer = -1;
                    System.Console.WriteLine("You can't play that card");
                }
            }
            return currentPlayer.Cards[answer];
        }

        private int ReadCorrectInput(int min, int max)
        {
            int number = -1;

            while (number == -1)
            {
                string input = System.Console.ReadLine() ?? "ERROR";
                try
                {
                    number = int.Parse(input);

                    if (number < min || number > max)
                    {
                        System.Console.WriteLine("Please enter a valid number");
                        number = -1;
                    }
                }

                catch (System.Exception)
                {
                    System.Console.WriteLine("Please enter a number");
                }
            }
            return number;
        }
    }

}