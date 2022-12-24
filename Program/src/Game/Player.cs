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
            if (!Cards.Contains(card))
            {
                throw new Exception($"Error in Play card , Player does not have this card {card.Name}");
            }
            card.Play();
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

        public override string ToString()
        {
            string str = Name + " Health: " + Health + ". Player Cards: \n";
            for (int i = 0; i < Cards.Count; i++)
            {
                str += i + ": " + Cards[i].ToString() + "\n";
            }

            return str;
        }

        public void Print()
        {
            Console.WriteLine(ToString());
        }
    }
}