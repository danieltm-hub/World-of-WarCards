using System.Reflection.Metadata;
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

            List<int> toPlay = GetCards();

            IsPreviousGame(initialGame);

            PlayCards(toPlay);
            
            string toPrint = String.Join("\n", toPlay.Select(card => GameManager.CurrentGame.CurrentPlayer.Cards[card].Name));
            Draw.WriteText(toPrint, Console.BufferWidth/2 - Console.BufferWidth/5 +1, 2, Console.BufferWidth/2 + Console.BufferWidth/5, Console.BufferHeight / 4 - 1, "#8900FF");
            Draw.WriteText($"{myPlayer.Name} decide pasar turno", Console.BufferWidth/2 - Console.BufferWidth/5 +1, 2, Console.BufferWidth/2 + Console.BufferWidth/5, Console.BufferHeight / 4 - 1, "#8900FF");
            GameManager.CurrentGame.NextTurn();
        }

        public abstract List<int> GetCards();

        public void IsCurrent()
        {
            if (myPlayer.Name != GameManager.CurrentGame.CurrentPlayer.Name) throw new Exception($"It is not {myPlayer.Name}'s turn, is {GameManager.CurrentGame.CurrentPlayer.Name}'s turn");
        }

        public List<int> PlayGenerator()
        {
            Game initialGame = GameManager.CurrentGame;

            GameManager.CurrentGame = initialGame.Clone();

            foreach (Player play in GameManager.CurrentGame.Players)
            {
                if (play.Cards.Count == 0) throw new Exception("AAAA");
            }

            List<int> toReturn = new List<int>();

            Random randomGenerator = new Random();

            while (true)
            {
                List<int> availableCards = AvailableCards();

                int index = randomGenerator.Next(availableCards.Count + 1);

                if (index == availableCards.Count) break;

                toReturn.Add(availableCards[index]);

                GameManager.CurrentGame.PlayCard(availableCards[index]);
            }

            GameManager.CurrentGame = initialGame;

            return toReturn;
        }

        public List<int> AvailableCards()
        {
            List<int> toReturn = new List<int>();

            for (int i = 0; i < GameManager.CurrentGame.CurrentPlayer.Cards.Count; i++)
            {

                if (GameManager.CurrentGame.CurrentPlayer.CanPlay(i)) toReturn.Add(i);
            }



            return toReturn;
        }

        private void PlayCards(List<int> cardsToPlay)
        {
            cardsToPlay.ForEach(card => GameManager.CurrentGame.PlayCard(card));
        }

        public void IsPreviousGame(Game expected)
        {
            if (!GameManager.CurrentGame.IsSameGame(expected)) throw new Exception("The game was changed in Simulations");
        }

    }
}