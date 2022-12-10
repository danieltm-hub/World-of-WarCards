using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProgram
{
    public class Player
    {
        string Name;
        double Health;
        public Player(string name)
        {
            Name = name;
        }

        public void ChangeHealth(double amount)
        {
            Health += amount;
        }
    }
}