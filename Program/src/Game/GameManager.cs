using System.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST;
namespace GameProgram
{
    public static class GameManager
    {
        public static Game CurrentGame { get; private set; }
        static GameManager()
        {
            List<Player> players = new List<Player>();

            //1vs1 Same cards
            players.Add(new Player("You", 100, CardsStore.BasicDeck));
            players.Add(new Player("TheCrazyPC", 100, CardsStore.BasicDeck));

            CurrentGame = new Game(players);
        }

        public static void ResetGame(Game game)
        {
            CurrentGame = game.Clone();
        }
    }
}