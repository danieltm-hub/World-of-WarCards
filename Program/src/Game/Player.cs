using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST;

namespace GameProgram
{
    public class Player
    {
        public List<State> OnTurnInitStates { get; private set; } = new List<State>();
        public List<State> OnPlayCardStates { get; private set; } = new List<State>();
        public List<State> OnTurnEndStates { get; private set; } = new List<State>();

        public string Name { get; private set; }
        public double Health { get; private set; }
        public double MaxHealth { get; private set; } = 20;
        public double Energy { get; private set; }
        public double MaxEnergy { get; private set; } = 10;

        public HashSet<Card> Cards { get; private set; } = new HashSet<Card>();
        public Player(string name, double health, double energy)
        {
            Name = name;
            Health = health;
            Energy = energy;
        }
        public void AddCards(List<Card> cards)// esto puede ir en el cosntructor mejor
        {
            foreach (Card card in cards)
            {
                Cards.Add(card);
            }
        }

        public bool Play(Card card)
        {
            card.EnergyCost.Evaluate();
            double cardCost = (double)card.EnergyCost.Value;

            if (Energy < cardCost || !Cards.Contains(card)) return false;

            ChangeEnergy(-cardCost);
            Cards.Remove(card);
            card.Play();

            return true;
        }
        public void ChangeHealth(double amount)
        {
            Health += Math.Clamp(Health + amount, 0, MaxHealth);
        }

        public void ChangeEnergy(double amount)
        {
            Energy += Math.Clamp(Energy + amount, 0, MaxEnergy);
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

        public Player Clone()
        {
            Player player = new Player(Name, Health, Energy);

            player.Cards = new HashSet<Card>(Cards);

            foreach (Card card in Cards)
            {
                player.Cards.Add(card.Clone());
            }

            player.OnTurnInitStates = new List<State>();
            player.OnTurnInitStates.AddRange(OnTurnInitStates);

            player.OnPlayCardStates = new List<State>();
            player.OnPlayCardStates.AddRange(OnPlayCardStates);

            player.OnTurnEndStates = new List<State>();
            player.OnTurnEndStates.AddRange(OnTurnEndStates);

            return player;
        }
    }
}