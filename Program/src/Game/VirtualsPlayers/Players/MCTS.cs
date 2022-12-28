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
        public NodeMCTS(int games, int wins, double score, Game gamestate)
        {
            Games = games;
            Wins = wins;
            Score = score;
            GameState = gamestate;
        }
    }

    public class MCTS
    {
        Player myPlayer;

        public MCTS(Player pcplayer, )
        {
            myPlayer = pcplayer;
        }

        private double FinalScore()
        {
            return 0;
        } 

    }





}