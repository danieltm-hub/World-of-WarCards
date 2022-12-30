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

        public double MaxHealth { get; private set; } = 20;
        public double MaxEnergy { get; private set; } = 10;
        public double RestoreEnergy { get; private set; } = 1;

        public string Name { get; private set; }
        public double Health { get; private set; }
        public double Energy { get; private set; }
        public List<Card> Cards { get; private set; }
        public double[] ColdownCards { get; private set; }

        public Player(string name, double health, double energy, List<Card> cards)
        {
            Name = name;
            Health = health;
            Energy = energy;
            Cards = new List<Card>();
            AddCards(cards);
            ColdownCards = new double[Cards.Count];
        }

        public void OnTurnInit()
        {
            ChangeEnergy(RestoreEnergy);
            ChangeColdowns();
            EvaluateStates(OnTurnInitStates);
        }

        public void OnPlayCard(Card card)
        {
            EvaluateStates(OnPlayCardStates);
        }

        public void OnTurnEnd()
        {
            EvaluateStates(OnTurnEndStates);
        }

        private void EvaluateStates(List<State> states)
        {
            foreach (State state in states)
            {
                state.Evaluate();
            }
        }

        private void AddCards(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                Cards.Add(card);
            }
        }

        public bool Play(Card card)
        {
            int cardIndex = GetCardIndex(card);

            if (cardIndex == -1) throw new Exception(card.Name + "Card not found"); // si no contiene la carta deberia ser una excepcion?

            card.EnergyCost.Evaluate();
            double cardCost = (double)card.EnergyCost.Value;

            if (ColdownCards[cardIndex] != 0 || Energy < cardCost)
            {
                return false;
            }

            ChangeEnergy(-cardCost);
            OnPlayCard(card);
            card.Play();
            AddColdown(card, cardIndex);

            return true;
        }

        public int GetCardIndex(Card wantedCard)
        {
            int index = -1;

            for (int i = 0; i < Cards.Count; i++)
            {
                if (Cards[i].Name == wantedCard.Name)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }
        private void ChangeColdowns()
        {
            for (int i = 0; i < ColdownCards.Length; i++)
            {
                ColdownCards[i] = Math.Max(ColdownCards[i] - 1, 0);
            }
        }
        private void AddColdown(Card card, int index)
        {
            card.Coldown.Evaluate();
            ColdownCards[index] = (double)card.Coldown.Value;
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
            List<Card> cardsClone = new List<Card>();

            foreach (Card card in Cards) // clonar las cartas   
            {
                cardsClone.Add(card.Clone());
            }

            Player player = new Player(Name, Health, Energy, cardsClone);

            for (int i = 0; i < ColdownCards.Length; i++) //Clonar los coldowns
            {
                player.ColdownCards[i] = ColdownCards[i];
            }

            foreach (State state in OnTurnInitStates) // Clonar los estados
            {
                player.AddTurnInitState(state);
            }

            foreach (State state in OnPlayCardStates)
            {
                player.AddPlayCardState(state);
            }

            foreach (State state in OnTurnEndStates)
            {
                player.AddTurnEndState(state);
            }

            return player;
        }

        public bool EqualPlayer(Player player)
        {
            if (Name != player.Name) return false;
            if (Health != player.Health) return false;
            if (Energy != player.Energy) return false;
            if (Cards.Count != player.Cards.Count) return false;

            foreach (Card card in Cards)
            {
                if (!player.Cards.Contains(card)) return false;
            }

            return true;
        }
    }
}