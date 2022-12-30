using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST;

namespace GameProgram
{
    public class Player : IClonable<Player>
    {
        public List<State> OnTurnInitStates {get; private set;} = new List<State>();
        public List<State> OnPlayCardStates {get; private set; } = new List<State>();
        public List<State> OnTurnEndStates {get; private set; } = new List<State>();


        public List<Card> Cards {get; private set; } = new List<Card>();
        public string Name {get; private set; }
        public double MaxHealth {get; private set; }
        public double Health {get; private set; }
        public double MaxEnergy {get; private set; }
        public double Energy {get; private set; }
        public Player(string name, double health, double energy, List<Card> cards)
        {
            Name = name;
            Health = health;
            MaxHealth = health;
            Energy = energy;
            MaxEnergy = energy;
            Cards = cards;
        }

        private Player(string name, double health, double maxHealth, double energy, double maxEnergy, List<Card> cards)
        {
            Name = name;
            Health = health;
            MaxHealth = maxHealth;
            Energy = energy;
            MaxEnergy = maxEnergy;
            Cards = cards;
        }

        public Player Clone()
        {
            List<Card> cards = new List<Card>();

            foreach(Card card in Cards)
            {
                cards.Add(card.Clone());
            }

            return new Player(Name, Health, MaxHealth, Energy, MaxEnergy, cards);
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

        public bool PlayCard(Card card)
        {
            if(!CanPlay(card)) return false;
            
            ChangeEnergy(-card.EnergyCostValue);
            
            OnPlayCardStates.ForEach(state => state.Evaluate());

            card.Play();

            return true;
        }

        public bool CanPlay(Card card)
        {
            return card.EnergyCostValue <= Energy && card.CurrentColdown == 0;
        }

        public void ReduceColdown() => Cards.ForEach(card => card.ReduceColdown());
    }
}