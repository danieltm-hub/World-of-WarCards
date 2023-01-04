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
        public int GamesPlayed { get; private set; }
        public int GamesWon { get; private set; }
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
        public void UpdateMCSTNode(int gamesPlayed, int gamesWon, double score)
        {
            // Revisar que se agreguen solo una vez

            GamesPlayed += gamesPlayed;
            GamesWon += gamesWon / gamesPlayed;
            Score += score / gamesPlayed;
        }
        public void AddChild(List<int> move)
        {
            Posibilities.Add(new MCSTNode(move));
        }
    }

    public class MCTS : Handler
    {
        public GetScore<Game, Player> Score { get; private set;}
        public Stopwatch Crono = new Stopwatch();
        public double exploreFactor { get => 0.5; }

        private int DepthLimit = 100;
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
            Explore(root, 0);

            root.UpdateMCSTNode(1, 0, double.MinValue);

            MCSTNode current = root;

            string path = "Checkout";
            string content = "";


            foreach(MCSTNode node in root.Posibilities)
            {
                content += $"Score: {node.Score}, Games Played: {node.GamesPlayed}, Games Won: {node.GamesWon}, {String.Join(' ', node.Move)}\n";

                if(node.GamesWon < current.GamesWon) continue;

                if(node.GamesWon > current.GamesWon)
                {
                    current = node;
                    continue;
                }

                if(node.Score > current.Score)
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
            if(GameManager.CurrentGame.IsOver())
            {
                int winner = (GameManager.CurrentGame.Winner().Name == myPlayer.Name) ? 1 : 0;
                node.UpdateMCSTNode(1, winner, Score(GameManager.CurrentGame, myPlayer));
                return;
            }

            if(IsTimeOut() || depth > DepthLimit)
            {
                node.UpdateMCSTNode(1, 0, Score(GameManager.CurrentGame, myPlayer));
                return;
            }

            if (node.Posibilities.Count == 0)
            {
                List<List<int>> children = AllGeneratorPlays(new List<int>());
                children.ForEach(move => node.AddChild(move));
            }

            List<MCSTNode> toExplore = new List<MCSTNode>();
            double mostOddsOnPath = double.MinValue;

            foreach (MCSTNode child in node.Posibilities)
            {

                double oddsOnPath = 0;
                if(child.GamesPlayed != 0) oddsOnPath = child.Score + exploreFactor * Math.Sqrt(Math.Log(Crono.ElapsedMilliseconds / 1000) / child.GamesPlayed);
                else oddsOnPath = double.MaxValue;

                if(oddsOnPath > mostOddsOnPath)
                {
                    mostOddsOnPath = oddsOnPath;
                    toExplore.Clear();
                    toExplore.Add(child);
                    continue;
                }

                if(oddsOnPath == mostOddsOnPath) toExplore.Add(child);
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