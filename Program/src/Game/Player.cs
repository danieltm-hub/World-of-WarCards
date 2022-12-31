using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST;

namespace GameProgram
{
    public class Player : IClonable<Player>
    {
        public List<State> OnTurnInitStates { get; private set; } = new List<State>();
        public List<State> OnPlayCardStates { get; private set; } = new List<State>();
        public List<State> OnTurnEndStates { get; private set; } = new List<State>();


        public List<Card> Cards { get; private set; } = new List<Card>();
        public string Name { get; private set; }
        public double MaxHealth { get; private set; }
        public double Health { get; private set; }
        public double MaxEnergy { get; private set; }
        public double Energy { get; private set; }
        private int MaxWill;
        private int Will;
        public Handler? CPU { get; private set; }

        public Player(string name, double health, double energy, int will, List<Card> cards)
        {
            Name = name;
            Health = health;
            MaxHealth = health;
            Energy = energy;
            MaxEnergy = energy;
            Will = Math.Clamp(will, 0, 8);
            MaxWill = Will;
            Cards = cards;
        }

        private Player(string name, double health, double maxHealth, double energy, double maxEnergy, int will, int maxWill, List<Card> cards)
        {
            Name = name;
            Health = health;
            MaxHealth = maxHealth;
            Energy = energy;
            MaxEnergy = maxEnergy;
            Will = will;
            MaxWill = maxWill;
            Cards = cards;
        }

        public void SetCPU(Handler cpu)
        {
            CPU = cpu;
        }

        public Player Clone()
        {
            List<Card> cards = new List<Card>();

            foreach (Card card in Cards)
            {
                cards.Add(card.Clone());
            }

            return new Player(Name, Health, MaxHealth, Energy, MaxEnergy, Will, MaxWill, cards);
        }

        public void ChangeHealth(double amount)
        {
            Health = Math.Clamp(Health + amount, 0, MaxHealth);
        }

        public void ChangeEnergy(double amount)
        {
            Energy = Math.Clamp(Energy + amount, 0, MaxEnergy);
        }

        public void ResetEnergy()
        {
            Energy = MaxEnergy;
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
            if (!CanPlay(card)) return false;

            ChangeEnergy(-card.EnergyCostValue);

            OnPlayCardStates.ForEach(state => state.Evaluate());

            card.Play();

            Will--;

            return true;
        }

        public bool CanPlay(Card card)
        {
            return card.EnergyCostValue <= Energy && card.CurrentColdown == 0 && Will > 0;
        }
        public void FillWill() => Will = MaxWill;
        public void ReduceColdown() => Cards.ForEach(card => card.ReduceColdown());

        public bool IsSamePlayer(Player player)
        {
            if (Name != player.Name) return false;
            if (MaxHealth != player.MaxHealth) return false;
            if (Health != player.Health) return false;
            if (MaxEnergy != player.MaxEnergy) return false;
            if (Energy != player.Energy) return false;

            if (player.Cards.Count != Cards.Count) return false;

            for (int i = 0; i < Cards.Count; i++)
            {
                if (!Cards[i].IsSameCard(player.Cards[i])) return false;
            }

            return true;
        }
    }
}