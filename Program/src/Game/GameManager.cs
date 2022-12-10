using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Program.Game
{
    public static class GameManager
    {
        public static List<Player> Players {get; private set;}
        public static int CurrentPlayerIndex { get; set; }
        public static Player CurrentPlayer => Players[CurrentPlayerIndex];
    }
}