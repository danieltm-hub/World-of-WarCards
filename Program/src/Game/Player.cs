using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST;
using Visual;

namespace GameProgram
{
    public class Player : IClonable<Player>
    {
        public List<State> OnTurnInitStates { get; private set; } = new List<State>();
        public List<State> OnPlayCardStates { get; private set; } = new List<State>();
        public List<State> OnTurnEndStates { get; private set; } = new List<State>();

        public List<Card> Cards { get; private set; } = new List<Card>();
        public int[] Cooldowns { get; private set; }
        public string Name { get; private set; }
        public double MaxHealth { get; private set; }
        public double Health { get; private set; }
        public double MaxEnergy { get; private set; }
        public double Energy { get; private set; }
        private int MaxWill;
        public int Will { get; private set; }
        public Handler Controller { get; private set; }

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
            Cooldowns = new int[Cards.Count];
            Controller = new Human(this);
        }

        private Player
        (string name, double health, double maxHealth, double energy, double maxEnergy, int will,
         int maxWill, List<State> onTurnInitStates, List<State> onPlayCardStates, List<State> onTurnEndStates,
         List<Card> cards, int[] cooldown, Handler controller)
        {
            Name = name;
            Health = health;
            MaxHealth = maxHealth;
            Energy = energy;
            MaxEnergy = maxEnergy;
            Will = will;
            MaxWill = maxWill;
            OnTurnInitStates = onTurnInitStates;
            OnPlayCardStates = onPlayCardStates;
            OnTurnEndStates = onTurnEndStates;
            Cards = cards;
            Cooldowns = cooldown;
            Controller = controller;
        }

        public void SetCPU(Handler cpu)
        {
            Controller = cpu;
        }

        public Player Clone()
        {

            List<State> onTurnInit = new List<State>();
            OnTurnInitStates.ForEach(state => onTurnInit.Add(state));

            List<State> onPlayCard = new List<State>();
            OnPlayCardStates.ForEach(state => onPlayCard.Add(state));

            List<State> onTurnEnd = new List<State>();
            OnTurnEndStates.ForEach(state => onTurnEnd.Add(state));

            int[] cooldowns = new int[Cards.Count];

            for (int i = 0; i < Cooldowns.Length; i++)
            {
                cooldowns[i] = Cooldowns[i];
            }

            return
            new Player(Name, Health, MaxHealth, Energy, MaxEnergy, Will,
            MaxWill, onTurnInit, onPlayCard, onTurnEnd, Cards, cooldowns, Controller);
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

        public bool PlayCard(int cardIndex)
        {
            Card card = Cards[cardIndex];

            if (!CanPlay(cardIndex))
            {
                Draw.PrintAt($"no se pudo jugar la carta {card.Name}", Console.BufferWidth / 2 - Console.BufferWidth / 5 + 1, 2, "#8900FF");
                Console.ReadKey();
                return false;
            }

            ChangeEnergy(-card.EnergyCostValue);

            OnPlayCardStates.ForEach(state => state.Evaluate());

            card.Play();

            Will--;
            Cooldowns[cardIndex] = card.CooldownValue;

            Draw.PrintPlayerStats(GameManager.CurrentGame.Players);
            return true;
        }

        public bool CanPlay(int cardIndex)
        {
            return Will > 0 && Cooldowns[cardIndex] == 0 && Cards[cardIndex].EnergyCostValue <= Energy;
        }

        public void FillWill() => Will = MaxWill;
        public void ReduceCooldown() => Cooldowns.Select(cooldown => cooldown = Math.Max(cooldown - 1, 0));
        public bool IsSamePlayer(Player player)
        {
            if (Name != player.Name) return false;
            if (Health != player.Health) return false;
            if (Energy != player.Energy) return false;

            for (int i = 0; i < Cooldowns.Length; i++)
            {
                if (Cooldowns[i] != player.Cooldowns[i]) return false;
            }

            return true;
        }
    }
}
