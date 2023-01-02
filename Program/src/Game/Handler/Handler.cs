using System.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProgram
{
    public abstract class Handler
    {
        private Player myPlayer;
        public Handler(Player player)
        {
            myPlayer = player;
        }

        public void Play()
        {
            IsCurrent();

            Game initialGame = GameManager.CurrentGame.Clone();

            HashSet<string> toPlay = GetCards();

            IsPreviousGame(initialGame);

            PlayCards(toPlay);

            System.Console.WriteLine($"{myPlayer.Name} played {string.Join(' ', toPlay)} cards");

            GameManager.CurrentGame.NextTurn();
        }
        public abstract HashSet<string> GetCards();
      
        public void IsCurrent()
        {
            if (myPlayer.Name != GameManager.CurrentGame.CurrentPlayer.Name) throw new Exception($"It is not {myPlayer.Name}'s turn, is {GameManager.CurrentGame.CurrentPlayer.Name}'s turn");
        }

        public HashSet<string> PlayGenerator()
        {
            Game initialGame = GameManager.CurrentGame;

            GameManager.CurrentGame = initialGame.Clone();

            HashSet<string> toReturn = new HashSet<string>();

            Random randomGenerator = new Random();

            while (true)
            {
                List<Card> availableCards = AvailableCards();

                int index = randomGenerator.Next(availableCards.Count + 1);

               
                if (index == availableCards.Count) break;


                toReturn.Add(availableCards[index].Name);

                GameManager.CurrentGame.PlayCard(availableCards[index]);
            }

            GameManager.CurrentGame = initialGame;

            return toReturn;
        }

        public List<Card> AvailableCards()
        {
            List<Card> toReturn = new List<Card>();

            foreach (Card card in GameManager.CurrentGame.CurrentPlayer.Cards)
            {
                if (GameManager.CurrentGame.CurrentPlayer.CanPlay(card)) toReturn.Add(card);
            }

            return toReturn;
        }

        private void PlayCards(HashSet<string> cardsToPlay)
        {
            Player getPlayer = GameManager.CurrentGame.CurrentPlayer;
            int cont = 0;

            foreach (Card card in getPlayer.Cards)
            {
                if (cardsToPlay.Contains(card.Name))
                {
                    GameManager.CurrentGame.PlayCard(card);
                    cont++;
                }
            }

            if (cont != cardsToPlay.Count) System.Console.WriteLine("No se juagron todas las cartas recibidas");

        }

        public void IsPreviousGame(Game expected)
        {
            if (!GameManager.CurrentGame.IsSameGame(expected)) throw new Exception("The game was changed in Simulations");
        }

    }
}