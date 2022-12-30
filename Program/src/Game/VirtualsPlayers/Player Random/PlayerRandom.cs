using System;
using AST;

namespace GameProgram
{
    public class RandomPlayer : IPlayer
    {

        Player myPlayer;
        public RandomPlayer(Player player)
        {
            myPlayer = player;
        }

        public void Play()
        {
            CheckTurn();

            Player player = GameManager.CurrentGame.CurrentPlayer; //get reference

            List<Card> RandomCards = RandomMove(player);

            foreach (Card card in RandomCards)
            {
                card.Play();
            }
        }

        private void CheckTurn()
        {
            string CurrentPlayerName = GameManager.CurrentGame.CurrentPlayer.Name;

            if (CurrentPlayerName != myPlayer.Name)
            {
                throw new Exception("RandomPlayer: Not my turn" + myPlayer.Name + "turn of " + CurrentPlayerName);
            }
        }

        private List<Card> RandomMove(Player player)
        {
            List<Card> Moves = AvailableCards(player).Item1;

            List<List<Card>> AllPosibilities = AvailableMoves(Moves, new bool[Moves.Count], new List<Card>(), player.Energy);

            Random number = new Random();
            return AllPosibilities[number.Next() % AllPosibilities.Count];
        }

        private (List<Card>, double) AvailableCards(Player player)
        {
            List<Card> availableCards = new List<Card>();

            double minEnergyCard = double.MaxValue;

            for (int i = 0; i < player.Cards.Count; i++)
            {
                if (player.ColdownCards[i] == 0)
                {
                    player.Cards[i].EnergyCost.Evaluate();

                    double cardCost = (double)player.Cards[i].EnergyCost.Value;

                    if (player.Energy >= cardCost)
                    {
                        minEnergyCard = Math.Min(minEnergyCard, cardCost);

                        availableCards.Add(player.Cards[i]);
                    }
                }
            }

            return (availableCards, minEnergyCard);
        }

        //en este el orden importa porque se lo puede permitir
        private List<List<Card>> AvailableMoves(List<Card> Available, bool[] mk, List<Card> Selected, double energy)
        {
            List<List<Card>> Moves = new List<List<Card>>();

            if (Selected.Count != 0)
            {
                Moves.Add(CloneCardList(Selected));
            }

            for (int i = 0; i < Available.Count; i++)
            {
                if (!mk[i])
                {
                    Available[i].EnergyCost.Evaluate();
                    double cardCost = (double)Available[i].EnergyCost.Value;

                    if (energy - cardCost >= 0)
                    {
                        mk[i] = true;
                        Selected.Add(Available[i]);

                        Moves.AddRange(AvailableMoves(Available, mk, Selected, energy - cardCost));

                        Selected.Remove(Available[i]);
                        mk[i] = false;
                    }
                }
            }

            return Moves;
        }
        private static List<Card> CloneCardList(List<Card> toClone)
        {
            List<Card> clone = new List<Card>();

            foreach (Card card in toClone)
            {
                clone.Add(card);
            }

            return clone;
        }

    }
}