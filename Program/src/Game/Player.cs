using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProgram
{
    public class Player
    {
        public string Name { get; private set; }
        public double Health { get; private set; }
        public List<Card> Cards { get; private set; } = new List<Card>();
        public Player(string name, double health, List<Card> cards)
        {
            Name = name;
            Health = health;
            Cards = cards;
        }
        public void AddCard(Card card)
        {
            Cards.Add(card);
        }
        public void PlayCard(Card card)
        {
            Cards.Remove(card);
        }
        public void ChangeHealth(double amount)
        {
            Health += amount;
        }
        public Player Clone()
        {
            List<Card> cardsClone = new List<Card>();
            foreach (Card card in Cards)
            {
                cardsClone.Add(card);
            }
            return new Player(Name, Health, cardsClone);
        }
    }
}