using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using AST;

namespace GameProgram
{
    public class MCSTNode
    {
        public long GamesPlayed { get; private set; }
        public long GamesWon { get; private set; }
        public double Score { get; private set; }
        public List<int> Move { get; private set; }
        public List<MCSTNode> Posibilities = new List<MCSTNode>();

        public MCSTNode(List<int> move)
        {
            Move = move;
        }
        public void UpdateMCSTNode(MCSTNode node)
        {
            UpdateMCSTNode(node.GamesPlayed, node.GamesWon, node.Score);
        }
        public void UpdateMCSTNode(long gamesPlayed, long gamesWon, double score)
        {
            GamesWon += (gamesWon == gamesPlayed) ? gamesWon : gamesWon + 1;

            GamesPlayed += gamesPlayed + 1;

            Score += score / gamesPlayed;
        }
        public void AddChild(List<int> move)
        {
            Posibilities.Add(new MCSTNode(move));
        }
    }

    public class MCTS : Handler
    {
        public GetScore<Game, Player> Score { get; private set; }
        public Stopwatch Crono = new Stopwatch();
        public double exploreFactor { get => 20; }

        private int DepthLimit = 30;
        private double TimeLimit = 5000;

        public MCTS(GetScore<Game, Player> score, Player player) : base(player)
        {
            Score = score;
        }

        public override List<int> GetCards()
        {
            Crono.Reset();
            Crono.Start();

            MCSTNode root = new MCSTNode(new List<int>());

            while (!IsTimeOut())
            {
                Explore(root, 0);
            }

            Crono.Stop();

            string path = "Checkout";
            string content = "";

            if(root.Posibilities.Count == 0) return new List<int>();

            MCSTNode current = root.Posibilities[0];

            foreach (MCSTNode node in root.Posibilities)
            {
                content += $"Score: {node.Score}, Games Played: {node.GamesPlayed}, Games Won: {node.GamesWon}, {String.Join(' ', node.Move)}\n";


                if (node.GamesWon / node.GamesPlayed < current.GamesWon / node.GamesPlayed) continue;
                
                if (node.GamesWon / node.GamesPlayed > current.GamesWon / node.GamesPlayed)
                {
                    current = node;
                    continue;
                }

                if (node.Score > current.Score)
                {
                    current = node;
                    continue;
                }
            }

            File.WriteAllText(path, content);

            return current.Move;
        }

        public void Explore(MCSTNode node, int depth)
        {
            if (GameManager.CurrentGame.IsOver())
            {
                int winner = (GameManager.CurrentGame.Winner().Name == myPlayer.Name) ? 1 : 0;
                node.UpdateMCSTNode(1, winner, Score(GameManager.CurrentGame, myPlayer));
                return;
            }

            if (IsTimeOut() || depth > DepthLimit)
            {
                node.UpdateMCSTNode(1, 0, Score(GameManager.CurrentGame, myPlayer));
                return;
            }

            if (node.Posibilities.Count == 0)
            {
                List<List<int>> children = AllGeneratorPlays(new List<int>());

                //children.RemoveAt(0);

                children.ForEach(move => node.AddChild(move));
            }

            List<MCSTNode> toExplore = new List<MCSTNode>();
            double mostOddsOnPath = double.MinValue;

            foreach (MCSTNode child in node.Posibilities)
            {

                double oddsOnPath = 0;
                if (child.GamesPlayed != 0) oddsOnPath = child.Score + exploreFactor * Math.Sqrt(Math.Log(Crono.ElapsedMilliseconds) / child.GamesPlayed);
                else oddsOnPath = double.MaxValue;

                if (oddsOnPath > mostOddsOnPath)
                {
                    mostOddsOnPath = oddsOnPath;
                    toExplore.Clear();
                    toExplore.Add(child);
                    continue;
                }

                if (oddsOnPath == mostOddsOnPath) toExplore.Add(child);
            }

            //Out of index exception revisar
            if (toExplore.Count() == 0)
            {
                node.UpdateMCSTNode(1, 0, Score(GameManager.CurrentGame, myPlayer));
                return;
            }

            Random random = new Random();
            int index = random.Next(0, toExplore.Count());


            MCSTNode selected = toExplore[index];

            Game gameState = GameManager.CurrentGame;
            GameManager.CurrentGame = gameState.Clone();

            PlayCards(selected.Move);

            GameManager.CurrentGame.NextTurn(false);

            Explore(selected, depth + 1);

            node.UpdateMCSTNode(selected);

            GameManager.CurrentGame = gameState;
        }

        private bool IsTimeOut()
        {
            return Crono.ElapsedMilliseconds > TimeLimit;
        }
    }
}