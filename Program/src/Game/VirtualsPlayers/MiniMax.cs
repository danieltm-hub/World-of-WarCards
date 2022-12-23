namespace GameProgram
{
    public class MiniMax : IPlayer
    {
        Player MyPlayer;
        GetScore<Game> Score;

        int MaxDepth;
        public MiniMax(Player player, GetScore<Game> score, int maxDepth = 23000)
        {
            MyPlayer = player; //referece to the player
            Score = score;
            MaxDepth = maxDepth;
        }

        public void Play()
        {
            if (GameManager.CurrentGame.CurrentPlayer.Name != MyPlayer.Name)
                throw new Exception("Error in Play Virtual . Not my turn");

            (double recivedScore, Card card) = MiniMaxCards(GameManager.CurrentGame, 0);
            System.Console.WriteLine("Score: " + recivedScore);

            if (GameManager.CurrentGame.CurrentPlayer.Name != MyPlayer.Name)
                throw new Exception("Error in Play Minimax Simluation . Not my turn");

            MyPlayer.PlayCard(card);
        }

        private double FinalMove(Game toReset)
        {
            Player? Winner = toReset.Winner();
            if (Winner == null) throw new Exception("Error in FinalMove Minimax . No winner");

            return (Winner == MyPlayer) ? 1 : 0;
        }

        private (double, Card) MiniMaxCards(Game toReset, int depth)
        {
            if (toReset.CurrentPlayer.Health == 0)
            {
                toReset.NextPlayer();
                throw new Exception("Exist a player with 0 health");
            }

            Player thisPlayer = toReset.CurrentPlayer;
            bool isMyTurn = thisPlayer.Name == MyPlayer.Name;

            if (depth == MaxDepth)
            {
                return (Score(toReset), toReset.CurrentPlayer.Cards[0]); //backtrack limit, use Score Euristic
            }

            double bestScore = isMyTurn ? int.MinValue : int.MaxValue; //Set max or min value

            Card bestCard = thisPlayer.Cards[0];

            Game game = toReset.Clone(); //copy state

            foreach (Card card in thisPlayer.Cards)
            {
                thisPlayer.PlayCard(card); //play card

                double scoreMove = 0;

                if (game.Winner() != null) scoreMove = FinalMove(game);

                else
                {
                    game.NextPlayer(); //next player
                    scoreMove = MiniMaxCards(game, depth + 1).Item1;//backtrack
                    game.PrevoiusPlayer(); //Come back
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

                GameManager.ResetGame(toReset); //reset game, undo play cart
            }

            return (bestScore, bestCard);
        }
    }
}