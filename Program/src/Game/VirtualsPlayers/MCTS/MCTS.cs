using System.Diagnostics;
using System;
using System.IO;
using AST;

namespace GameProgram
{

    public struct NodeMCTS
    {
        /* quisiera un mejor nombre para esto pero no se me ocurre, 
        la idea de esto es tener todos los datos de una partida */
        public int Games;
        public int Wins;
        public double Score;
        public Game GameState;
        public List<Card> Moves;
        public NodeMCTS()
        {
            Games = 0;
            Wins = 0;
            Score = 0;
            GameState = new Game(new List<Player>(), new EnemyDefeated());
            Moves = new List<Card>();
        }
        public NodeMCTS(int games, int wins, double score, Game gamestate, List<Card> moves)
        {
            Games = games;
            Wins = wins;
            Score = score;
            GameState = gamestate;
            Moves = moves;
        }
    }

    public class MCTS : IPlayer
    {
        /* Todas las busquedas y extraccion de informacion se hace sobre GameManager,
        para evitar errores en los gameStates.*/
        Player myPlayer;
        IStrategy Strategy;
        IStadistic Select;
        double TimeLimit;

        public MCTS(Player pcplayer, IStrategy strategy, IStadistic select, double timeLimit = 5000)
        {
            myPlayer = pcplayer;
            Strategy = strategy;
            Select = select;
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

        private List<Card> RandomMove(List<List<Card>> availableMoves)
        {
            Random random = new Random();

            return availableMoves[random.Next() % availableMoves.Count];
        }

        private List<List<Card>> AvailableMoves(List<Card> Available, bool[] mk, List<Card> Selected, double energy)
        {
            List<List<Card>> Moves = new List<List<Card>>();

            if (Selected.Count != 0)
            {
                Moves.Add(CloneCardList(Selected));
            }

            for (int i = 0; i < Available.Count; i++)
            {
                if (!mk[i])
                {
                    Available[i].EnergyCost.Evaluate();
                    double cardCost = (double)Available[i].EnergyCost.Value;

                    if (energy - cardCost >= 0)
                    {
                        mk[i] = true;
                        Selected.Add(Available[i]);

                        Moves.AddRange(AvailableMoves(Available, mk, Selected, energy - cardCost));

                        Selected.Remove(Available[i]);
                        mk[i] = false;
                    }
                }
            }

            return Moves;
        }

        private static List<Card> CloneCardList(List<Card> toClone)
        {
            List<Card> clone = new List<Card>();

            foreach (Card card in toClone)
            {
                clone.Add(card);
            }

            return clone;
        }

        private List<Card> BestMove()
        {
            Stopwatch Crono = new Stopwatch();

            Crono.Start();

            List<NodeMCTS> Options = MonteCarlosTreeSearch(Crono);

            return Select.SelectNode(Options).Moves;
        }

        private bool IsTimeOut(Stopwatch time)
        {
            return time.ElapsedMilliseconds > TimeLimit;
        }

        private List<NodeMCTS> MonteCarlosTreeSearch(Stopwatch time)
        {
            Game game = GameManager.CurrentGame; //solo para no tener que escribirlo todo

            if (game.IsOver())
            {
                int winner = game.Winner().Name == myPlayer.Name ? 1 : 0;

                return new List<NodeMCTS> { new NodeMCTS(1, winner, Strategy.FinalState(game, myPlayer), game, new List<Card>()) };
            }

            if (IsTimeOut(time))
            {
                time.Stop();
                return new List<NodeMCTS> { new NodeMCTS(1, 0, Strategy.FinalState(game, myPlayer), game, new List<Card>()) };
            }


            List<Card> availableCards = AvailableCards(game.CurrentPlayer).Item1;

            List<List<Card>> AllMoves = AvailableMoves(availableCards, new bool[availableCards.Count], new List<Card>(), game.CurrentPlayer.Energy);

            List<NodeMCTS> childs = new List<NodeMCTS>();

            Game toReset = GameManager.CurrentGame.Clone();

            while (AllMoves.Count > 0)
            {
                List<Card> move = RandomMove(AllMoves);

                AllMoves.Remove(move);

                PlayMove(move); //GameManager.CurrentGame.CurrentPlayer.Play(every card of move);

                GameManager.CurrentGame.NextTurn(); // pass

                childs.Add(MakeNode(MonteCarlosTreeSearch(time), move)); // backtracking get stadistics

                GameManager.CurrentGame = toReset.Clone(); //Reset the game state
            }

            return childs;
        }

        private NodeMCTS MakeNode(List<NodeMCTS> childs, List<Card> Moves)
        {
            NodeMCTS node = new NodeMCTS(0, 0, 0, GameManager.CurrentGame.Clone(), new List<Card>());

            foreach (NodeMCTS child in childs)
            {
                node.Games += child.Games;
                node.Wins += child.Wins;
                node.Score += child.Score;
            }

            if (node.Wins == node.Games) node.Wins++; // if all ways to Roma are win, then this is a win
            node.Score = node.Score / node.Games; // average score
            node.Games++; // add visit
            node.Moves = Moves; // add moves

            return node;
        }
        private void PlayMove(List<Card> move)
        {
            foreach (Card card in move)
            {
                GameManager.CurrentGame.CurrentPlayer.Play(card);
            }
        }
    }
}