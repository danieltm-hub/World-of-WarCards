using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST;
using System.Diagnostics;


namespace GameProgram
{
    public struct NodeMCTS
    {
        public int Games;
        public int Wins;
        public double Score;
        public List<int> Moves;
        public NodeMCTS()
        {
            Games = 0;
            Wins = 0;
            Score = 0;
            Moves = new List<int>();
        }
        public NodeMCTS(int games, int wins, double score, List<int> moves)
        {
            Games = games;
            Wins = wins;
            Score = score;
            Moves = moves;
        }

    }

    public class MCTS : Handler
    {
        public Stopwatch Crono = new Stopwatch();
        public int DepthLimit { get; private set; } = 10000;
        public GetScore<Game, Player> Score { get; private set; }
        public MCTS(GetScore<Game, Player> score, Player player) : base(player)
        {
            Score = score;
        }

        private double TimeLimit = 5000; // time 

        public override List<int> GetCards()
        {
            Crono.Reset();
            return BestMove();
        }

        private List<int> BestMove()
        {
            Crono.Start();

            if (GameManager.CurrentGame.IsOver()) throw new Exception("NO OVER");

            List<NodeMCTS> Options = MonteCarlosTreeSearch(GameManager.CurrentGame, 0);
            Crono.Stop();

            List<int> bestNode = new List<int>();
            double bestScore = int.MinValue;
            int winPlays = -1;
            int playedPlays = -1;

            foreach (NodeMCTS node in Options)
            {
                if (node.Wins > winPlays)
                {
                    bestNode = node.Moves;
                    bestScore = node.Score;
                    winPlays = node.Wins;
                    playedPlays = node.Games;
                }

                else if (node.Wins == winPlays)
                {
                    if (node.Games > playedPlays)
                    {
                        bestNode = node.Moves;
                        bestScore = node.Score;
                        playedPlays = node.Games;
                    }

                    else if (node.Games == playedPlays)
                    {
                        if (node.Score > bestScore)
                        {
                            bestNode = node.Moves;
                            bestScore = node.Score;
                        }
                    }
                }
            }

            return bestNode;
        }

        private int RandomMove(List<List<int>> availableMoves)
        {
            Random random = new Random();
            return random.Next(0, availableMoves.Count);
        }

        private bool IsTimeOut()
        {
            return Crono.ElapsedMilliseconds > TimeLimit;
        }


        private List<NodeMCTS> MonteCarlosTreeSearch(Game gameState, int depth)
        {
            if (gameState.IsOver())
            {
                int winner = gameState.Winner().Name == myPlayer.Name ? 1 : 0; // return score de ganar
                return new List<NodeMCTS> { new NodeMCTS(1, winner, Score(gameState, myPlayer), new List<int>()) };
            }


            if (IsTimeOut() || depth > DepthLimit)
            {

                return new List<NodeMCTS> { new NodeMCTS(1, 0, Score(gameState, myPlayer), new List<int>()) };
            }


            List<NodeMCTS> childs = new List<NodeMCTS>();

            List<List<int>> allMoves = AllGeneratorPlays(new List<int>(), AvailableCards(), GameManager.CurrentGame);


            GameManager.CurrentGame = gameState.Clone();

            while (allMoves.Count > 0)
            {
                int index = RandomMove(allMoves);

                List<int> move = allMoves[index];

                allMoves.RemoveAt(index);

                PlayCards(move);

                GameManager.CurrentGame.NextTurn(false);

                childs.Add(MakeNode(MonteCarlosTreeSearch(GameManager.CurrentGame, depth + 1), move));

                GameManager.CurrentGame = gameState.Clone();
            }


            GameManager.CurrentGame = gameState;

            return childs;
        }

        private NodeMCTS MakeNode(List<NodeMCTS> childs, List<int> Moves)
        {
            NodeMCTS node = new NodeMCTS(0, 0, 0, new List<int>());

            foreach (NodeMCTS child in childs)
            {
                node.Games += child.Games;
                node.Wins += child.Wins;
                node.Score += child.Score;
            }

            if (node.Wins == node.Games) node.Wins++; // if all ways to Roma are win, then this is a win

           

            node.Games++; // add visit
            Moves.ForEach(x => node.Moves.Add(x)); // add moves

            return node;
        }
    }
}