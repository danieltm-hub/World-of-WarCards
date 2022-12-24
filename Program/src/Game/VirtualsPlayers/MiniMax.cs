using System.Runtime.InteropServices;
namespace GameProgram
{
    public class MiniMax : IPlayer
    {
        Player MyPlayer;
        GetScore<Game, Player> Score;

        int MaxDepth;
        public MiniMax(Player player, GetScore<Game, Player> score, int maxDepth = 21000)
        {
            MyPlayer = player; //referece to the player
            Score = score;
            MaxDepth = maxDepth;
        }

        public void Play()
        {
            CheckTurn();
            System.Console.WriteLine(MyPlayer.Name + " is thinking... \n");

            Game toReset = GameManager.CurrentGame.Clone(); //copy initial state

            (double recivedScore, Card card) = MiniMaxCards(0);

            CheckGame(toReset);
            CheckTurn();

            System.Console.WriteLine("Score: " + recivedScore);
            Thread.Sleep(5000);
            System.Console.WriteLine("Play Card: " + card.ToString() + "\n");

            MyPlayer.PlayCard(card);
        }
        private void CheckGame(Game toReset)
        {
            if (!GameManager.CurrentGame.EqualGame(toReset))
                throw new Exception("Error in Play Virtual . Game has changed");
        }
        private void CheckTurn()
        {
            if (GameManager.CurrentGame.CurrentPlayer.Name != MyPlayer.Name)
            {
                throw new Exception("Error in Play Virtual . Not my turn");
            }
        }


        private double FinalMove(Game toReset)
        {
            Player? Winner = toReset.Winner();

            if (Winner == null) throw new Exception("Error in FinalMove Minimax . No winner");

            return (Winner == MyPlayer) ? 1 : Score(toReset, MyPlayer);
        }

        private (double, Card) MiniMaxCards(int depth)
        {
            Game Game = GameManager.CurrentGame; //using reference
            Player thisPlayer = Game.CurrentPlayer;

            if (depth == MaxDepth) return (Score(Game, MyPlayer), thisPlayer.Cards[0]); //depth limit => Aproximity

            bool isMyTurn = (thisPlayer.Name == MyPlayer.Name);
            double bestScore = isMyTurn ? int.MinValue : int.MaxValue; //Set max or min value
            //Card bestCard = thisPlayer.Cards[0]; //throw new Exception if player havent cards
            Card? bestCard = null;
            Game gametoReset = Game.Clone(); //Save State

            foreach (Card card in thisPlayer.Cards)
            {
                thisPlayer.PlayCard(card); //play card

                double scoreMove = 0;

                if (Game.Winner() != null) scoreMove = FinalMove(Game);

                else
                {
                    Game.NextPlayer(); //next player
                    scoreMove = MiniMaxCards(depth + 1).Item1;//backtrack
                }

                if (isMyTurn && scoreMove > bestScore) //update best score, in MaxTree
                {
                    bestScore = scoreMove;
                    bestCard = card;
                }

                if (!isMyTurn && scoreMove < bestScore) //update best score, in MinTree
                {
                    bestScore = scoreMove;
                    bestCard = card;
                }

                GameManager.ResetGame(gametoReset); //reset state, undo play card, previous player
            }

            return (bestScore, bestCard);
        }
    }
}