using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Visual;

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

            List<Card> toPlay = GetCards();

            IsPreviousGame(initialGame);

            PlayCards(toPlay);

            for (int i = 0; i < toPlay.Count; i++)
            {
                Draw.WriteAt($"{myPlayer.Name} jugo " + toPlay[i].Name, Console.BufferWidth / 2 - Console.BufferWidth / 5 + 1, i + 2);
            }

            GameManager.CurrentGame.NextTurn();
        }
        public abstract List<Card> GetCards();

        public List<Card> PlayGenerator()
        {
            Game initialGame = GameManager.CurrentGame.Clone();

            List<Card> toReturn = new List<Card>();

            Random randomGenerator = new Random();

            while (true)
            {
                List<Card> availableCards = AvailableCards();

                int index = randomGenerator.Next(availableCards.Count + 1);

                if (index == availableCards.Count || availableCards.Count == 0) break;

                toReturn.Add(availableCards[index]);

                GameManager.CurrentGame.PlayCard(availableCards[index]);
            }

            GameManager.CurrentGame = initialGame;
            
            return toReturn;
        }

        public List<Card> AvailableCards()
        {
            List<Card> toReturn = new List<Card>();
            Player player = GameManager.CurrentGame.CurrentPlayer;

            foreach (Card card in GameManager.CurrentGame.CurrentPlayer.Cards)
            {
                if (player.CanPlay(card)) toReturn.Add(card);
            }

            return toReturn;
        }

        protected void PlayCards(List<Card> cards) => cards.ForEach(card => GameManager.CurrentGame.PlayCard(card));
        public void IsCurrent()
        {
            if (myPlayer.Name != GameManager.CurrentGame.CurrentPlayer.Name) throw new Exception($"It is not {myPlayer.Name}'s turn, is {GameManager.CurrentGame.CurrentPlayer.Name}'s turn");
        }
        public void IsPreviousGame(Game expected)
        {
            if (!GameManager.CurrentGame.IsSameGame(expected)) throw new Exception("The game was changed in Simulations");
        }

    }
}