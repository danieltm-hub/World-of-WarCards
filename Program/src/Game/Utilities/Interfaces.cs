using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST;

namespace GameProgram
{
    public interface IClonable<out T>
    {
        public T Clone();
    }

    public interface IWinCondition
    {
        public Player? Winner { get; }
        public bool CheckWinCondition(Game game);
    }
}