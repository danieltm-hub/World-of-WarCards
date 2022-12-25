using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST;

namespace GameProgram
{
    public class Player
    {
        public List<State> OnTurnInitStates {get; private set;} = new List<State>();
        public List<State> OnPlayCardStates {get; private set; } = new List<State>();
        public List<State> OnTurnEndStates {get; private set; } = new List<State>();

        public string Name {get; private set; }
        public double Health {get; private set; }
        public double Energy {get; private set; }
        public Player(string name, double health, double energy)
        {
            Name = name;
            Health = health;
            Energy = energy;
        }

        public void ChangeHealth(double amount)
        {
            Health += Math.Clamp(Health + amount, 0, 20);
        }

        public void ChangeEnergy(double amount)
        {
            Energy += Math.Clamp(Energy + amount, 0, 10);
        }

        public void AddTurnInitState(State state)
        {
            OnTurnInitStates.Add(state);
        }

        public void AddTurnEndState(State state)
        {
            OnTurnEndStates.Add(state);
        }

        public void AddPlayCardState(State state)
        {
            OnPlayCardStates.Add(state);
        }
    }
}