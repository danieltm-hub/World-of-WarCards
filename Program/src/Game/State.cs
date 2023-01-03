using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST;

namespace GameProgram
{
    public class State
    {
        public Effect MyEffect { get; private set; }
        public double Duration { get; private set; }

        public State(Effect effect, double duration)
        {
            MyEffect = effect;
            Duration = duration;
        }

        public bool Evaluate()
        {
            if(Duration == 0) return false; 
            
            MyEffect.Evaluate();
            Duration -= 1;
            return true;
        }
    }
}