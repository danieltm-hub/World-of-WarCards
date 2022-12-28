using System;
using System.IO;
using AST;

namespace GameProgram
{

    public struct NodeMCTS
    {
        public int Games;
        public int Wins;
        public double Score;
        public Game GameState;
        public NodeMCTS(int games, int wins, double score, Game gamestate)
        {
            Games = games;
            Wins = wins;
            Score = score;
            GameState = gamestate;
        }
    }



    public class MCTS
    {
        /* Todas las busquedas y extraccion de informacion se hace sobre GameManager,
        para evitar errores en los gameStates.*/
        Player myPlayer;
        IStrategy Strategy;

        public MCTS(Player pcplayer, IStrategy strategy)
        {
            myPlayer = pcplayer;
            Strategy = strategy;
        }

        public void Play()
        {

        }

        public void CheckTurn()
        {
            if (GameManager.CurrentGame.CurrentPlayer.Name != myPlayer.Name)
            {
                throw new Exception(myPlayer.Name + "CheckTurn MCTS: Not my turn");
            }
        }

        
        private List<Card> AvailableCards(Player player)
        {
            List<Card> availableCards = new List<Card>();

            foreach (Card card in player.Cards)
            {
                card.EnergyCost.Evaluate();
                double cardCost = (double)card.EnergyCost.Value;

                if (cardCost <= player.Energy)
                {
                    availableCards.Add(card);
                }
            }

            return availableCards;
        }

        private Player SearchPlayer(Player player)
        {
            foreach (Player gamer in GameManager.CurrentGame.Players)
            {
                if (gamer.Name == player.Name)
                {
                    return gamer;
                }
            }
            throw new Exception("SearchPlayer: Player not found");
        }

        private Card RandomCard(List<Card> cards)
        {
            Random random = new Random();
            int index = random.Next(cards.Count);
            return cards[index];
        }



    }





}