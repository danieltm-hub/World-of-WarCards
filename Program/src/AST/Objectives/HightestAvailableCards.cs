using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class HighestCards : Objective
    {
        public override string Keyword => "highestcards";
        public override string Description => "player with the highest available cards";
        public override List<NodeType> ExpectedTypes => new List<NodeType>();
        public HighestCards(List<Node> parameters, CodeLocation location) : base(parameters, location) { }
        public override List<Player> Evaluate()
        {
            Player selected = GameManager.CurrentGame.Players[0];
            int cardsAmount = int.MinValue;

            bool twoPlayerwithSameAmount = false;

            foreach (Player player in GameManager.CurrentGame.Players)
            {
                int playerAvailableCards = AvailablesCard(player);

                if (cardsAmount < playerAvailableCards)
                {
                    cardsAmount = playerAvailableCards;
                    selected = player;
                    twoPlayerwithSameAmount = false;
                }

                else if (cardsAmount == playerAvailableCards)
                {
                    twoPlayerwithSameAmount = true;
                }
            }

            return (twoPlayerwithSameAmount) ? new List<Player>() : new List<Player> { selected };
        }
        private int AvailablesCard(Player player)
        {
            int availableCards = 0;
            
            for (int i = 0; i < player.Cards.Count; i++)
            {
                if (player.CanPlay(i)) availableCards++;
            }
            return availableCards;
        }
    }



}