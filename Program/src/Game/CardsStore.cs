using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST;
////////////////////////////////////////TODO ESTO ES PARA BORRAR///////////////////////////////////////////////
namespace GameProgram
{
    public static class CardsStore
    {

        //Helpers
        private static double Abs(double num) => (num < 0) ? num * -1 : num;
        //private static List<T> Lister<T>(params T[] item) => item.ToList();
        private static List<Expression> Lister(params Expression[] item) => item.ToList();
        private static List<Objective> ListerO(params Objective[] item) => item.ToList();
        private static List<Power> ListerP(params Power[] item) => item.ToList();
        private static List<Effect> ListerE(List<Objective> A, List<Power> B) => new List<Effect>() { new Effector(A, B, new CodeLocation()) };

        //Modifiers
        private static ModifyHealth Damage(double amount) => new ModifyHealth(Lister(new Number(Abs(amount) * -1, new CodeLocation())), new CodeLocation());
        private static ModifyHealth Heal(double amount) => new ModifyHealth(Lister(new Number(Abs(amount), new CodeLocation())), new CodeLocation());
        private static Self Self() => new Self(new List<Expression>(), new CodeLocation());
        private static NextPlayer NextPlayer() => new NextPlayer(new List<Expression>(), new CodeLocation());


        ///Cards
        public static Card DamageCard = new Card("Damage", ListerE(ListerO(Self()), ListerP(Damage(10))), new CodeLocation());
        public static Card FireballCard = new Card("Fireball", ListerE(ListerO(NextPlayer()), ListerP(Damage(15))), new CodeLocation());
        public static Card LightningCard = new Card("Lightning", ListerE(ListerO(NextPlayer()), ListerP(Damage(20))), new CodeLocation());
        public static Card HealCard = new Card("Heal", ListerE(ListerO(Self()), ListerP(Heal(5))), new CodeLocation());


        //Decks
        public static List<Card> BasicDeck = new List<Card>() { FireballCard, DamageCard, LightningCard };

    }
}