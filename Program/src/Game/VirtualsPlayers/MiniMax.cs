using System.Runtime.InteropServices;
namespace GameProgram
{
    public class MiniMax : IPlayer
    {
        Player MyPlayer;
        GetScore<Player> Score;

        int MaxDepth;
        public MiniMax(Player player, GetScore<Player> score, int maxDepth = 21000)
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
            Thread.Sleep(2000);
            System.Console.WriteLine("Play Card: " + card.ToString() + "\n");
            MyPlayer.PlayCard(card);
        }
        private void CheckGame(Game toReset)
        {
            if (!GameManager.CurrentGame.EqualGame(toReset))
            {
                throw new Exception("Error in Play Virtual . Game has changed, out of Simulation");
            }
        }
        private void CheckTurn()
        {
            if (GameManager.CurrentGame.CurrentPlayer.Name != MyPlayer.Name)
            {
                throw new Exception("Error in Play Virtual . Not my turn");
            }
        }


        private double FinalMove()
        {
            Player? Winner = GameManager.CurrentGame.Winner();

            if (Winner == null) throw new Exception("Error in FinalMove Minimax . No winner");

            return (Winner.Name == MyPlayer.Name) ? 1 : Score(MyPlayer);
        }

        private (double, Card) MiniMaxCards(int depth)
        {
            Player thisPlayer = GameManager.CurrentGame.CurrentPlayer;

            if (depth == MaxDepth) return (Score(MyPlayer), thisPlayer.Cards[0]); //depth limit => Aproximity

            bool isMyTurn = (thisPlayer.Name == MyPlayer.Name);
            double bestScore = isMyTurn ? int.MinValue : int.MaxValue;

            Card bestCard = thisPlayer.Cards[0];
            Game gametoReset = GameManager.CurrentGame.Clone(); //Save State

            foreach (Card card in thisPlayer.Cards)
            {
                thisPlayer.PlayCard(card); //play card

                double scoreMove = 0;

                if (GameManager.CurrentGame.Winner() != null) scoreMove = FinalMove();

                else
                {
                    GameManager.CurrentGame.NextPlayer(); //next player
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