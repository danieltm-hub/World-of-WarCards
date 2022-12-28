using System.Diagnostics;
using System;
using System.IO;
using AST;

namespace GameProgram
{

    public struct NodeMCTS
    {
        public int Games;
        public int Wins;
        public double Score;
        public Game GameState;
        public List<Card> Moves;
        public NodeMCTS(int games, int wins, double score, Game gamestate, List<Card> moves)
        {
            Games = games;
            Wins = wins;
            Score = score;
            GameState = gamestate;
            Moves = moves;
        }
    }



    public class MCTS
    {
        /* Todas las busquedas y extraccion de informacion se hace sobre GameManager,
        para evitar errores en los gameStates.*/
        Player myPlayer;
        IStrategy Strategy;
        double TimeLimit;

        public MCTS(Player pcplayer, IStrategy strategy, double timeLimit = 5000)
        {
            myPlayer = pcplayer;
            Strategy = strategy;
            TimeLimit = timeLimit;
        }

        public void Play()
        {
            CheckTurn();
            Game initialgame = GameManager.CurrentGame.Clone();

            List<Card> BestMoves = new List<Card>();
            BestMoves = BestMove();

            CheckGame(initialgame);
            PlayCards(BestMoves);
        }
        private void PlayCards(List<Card> toPlay)
        {
            foreach (Card card in toPlay)
            {
                myPlayer.Play(card);
            }
        }
        public void CheckGame(Game gameState)
        {
            if (!gameState.EqualGame(GameManager.CurrentGame))
            {
                throw new Exception(myPlayer.Name + "CheckGame MCTS: GameStates are not equal, SimulationError");
            }
        }
        public void CheckTurn()
        {
            if (GameManager.CurrentGame.CurrentPlayer.Name != myPlayer.Name)
            {
                throw new Exception(myPlayer.Name + "CheckTurn MCTS: Not my turn");
            }
        }
        private (List<Card>, double) AvailableCards(Player player)
        {
            List<Card> availableCards = new List<Card>();
            double minEnergyCard = double.MaxValue;

            for (int i = 0; i < player.Cards.Count; i++)
            {
                if (player.ColdownCards[i] == 0)
                {
                    player.Cards[i].EnergyCost.Evaluate();
                    double cardCost = (double)player.Cards[i].EnergyCost.Value;

                    if (player.Energy >= cardCost)
                    {
                        minEnergyCard = Math.Min(minEnergyCard, cardCost);
                        availableCards.Add(player.Cards[i]);
                    }
                }
            }

            return (availableCards, minEnergyCard);
        }
        private Player SearchPlayer(Player player)
        {
            foreach (Player gamer in GameManager.CurrentGame.Players)
            {
                if (gamer.Name == player.Name)
                {
                    return gamer;
                }
            }
            throw new Exception("SearchPlayer: Player not found");
        }
        private List<Card> RandomMove(List<List<Card>> availableMoves)
        {
            Random random = new Random();
            int index = random.Next(availableMoves.Count);
            return availableMoves[index];
        }
        private List<List<Card>> AvailableMoves(Player player, List<Card> selected, List<Card> available, int index, int minEnergyCard)
        {
            if (index >= available.Count || player.Energy < minEnergyCard)
            {
                return new List<List<Card>> { selected };
            }

            List<List<Card>> moves = new List<List<Card>>();

            for (int i = 0; i < available.Count; i++)
            {
                available[i].EnergyCost.Evaluate();
                double cardCost = (double)available[i].EnergyCost.Value;

                if (player.Energy >= cardCost)
                {
                    player.ChangeEnergy(-cardCost);

                    selected.Add(available[i]);

                    moves.AddRange(AvailableMoves(player, selected, available, i + 1, minEnergyCard));

                    selected.RemoveAt(selected.Count - 1);

                    player.ChangeEnergy(cardCost);
                }
            }
            return moves;
        }

        private string HashMove(List<Card> cards)
        {
            string hash = "";
            foreach (Card card in cards)
            {
                hash += card.Name + ' ';
            }
            return hash;
        }

        private List<Card> BestMove()
        {
            Stopwatch Crono = new Stopwatch();
            Crono.Start();
            return MonteCarlosTreeSearch(new NodeMCTS(0, 0, 0, new Game(), new List<Card>()), Crono).Moves;
        }


        private NodeMCTS MonteCarlosTreeSearch(NodeMCTS node, Stopwatch time)
        {
            
        }
    }





}