using AST;

namespace GameProgram
{
    /*
    Este es el player de la A43, que de todos los movimientos escogia el mejor por turno,
    bajo el mismo concepto la idea es desarrollar una interfaz que dado dos movimientos escoja el mejor.
    */
    public class PlayerA43 : IPlayer
    {
        Player myPlayer;
        IStrategy43 Strategy;
        public PlayerA43(Player player, IStrategy43 strategy)
        {
            myPlayer = player;
            Strategy = strategy;
        }

        public void Play()
        {
            CheckTurn();
            Player player = GameManager.CurrentGame.CurrentPlayer;
            List<Card> available = AvailableCards(player).Item1;
            List<Card> BestMove = KnapsackCards(available, new bool[available.Count], new List<Card>(), int.MinValue, new List<Card>(), player.Energy);
            
        
        }
        public void PlayMove(List<Card>cards)
        {
            foreach (Card card in cards)
            {
                GameManager.CurrentGame.CurrentPlayer.Play(card);
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

        private List<Card> KnapsackCards(List<Card> Available, bool[] mask, List<Card> Selected, double bestScore, List<Card> BestMove, double playerEnergy)
        {
            if (Selected.Count != 0)
            {
                double scored = GetMoveScore(Selected);
                if (scored > bestScore)
                {
                    bestScore = scored;
                    BestMove = CloneCardList(Selected);
                }
            }

            for (int i = 0; i < Available.Count; i++)
            {
                if (!mask[i])
                {
                    Available[i].EnergyCost.Evaluate();
                    double cardCost = (double)Available[i].EnergyCost.Value;

                    if (playerEnergy - cardCost >= 0)
                    {
                        mask[i] = true;

                        Selected.Add(Available[i]);

                        List<Card> partial = KnapsackCards(Available, mask, Selected, bestScore, BestMove, playerEnergy - cardCost);

                        Selected.Remove(Available[i]);

                        mask[i] = false;

                        double scored = GetMoveScore(partial);

                        if (scored > bestScore)
                        {
                            bestScore = scored;
                            BestMove = CloneCardList(partial);
                        }
                    }
                }
            }

            return BestMove;

        }

        private double GetMoveScore(List<Card> cards)
        {
            Game BeforePlay = GameManager.CurrentGame.Clone();

            Player player = GameManager.CurrentGame.CurrentPlayer;

            foreach (Card card in cards)
            {
                player.Play(card);
            }

            double score = Strategy.GameStateScore(GameManager.CurrentGame);

            GameManager.CurrentGame = BeforePlay;

            return score;
        }
        private List<Card> CloneCardList(List<Card> toClone)
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