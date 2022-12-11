using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compiler
{
    public static class LexicStore
    {
        public static Dictionary<string, TokenType> Symbols { get; private set; } = new Dictionary<string, TokenType>()
        { //all that starts with a special char

            {"+", TokenType.Sum},
            {"-", TokenType.Sub},
            {"*", TokenType.Mul},
            {"/", TokenType.Div},
            {"^", TokenType.Pow},
            {"=", TokenType.Equal},
            {"<", TokenType.Smaller},
            {">", TokenType.Greater},
            {"&&", TokenType.And},
            {"||", TokenType.Or},
            {"(", TokenType.LParen},
            {")", TokenType.RParen},

            {"[", TokenType.LBracket},
            {"]", TokenType.RBracket},
            {"{", TokenType.LSquareBracket},
            {"}", TokenType.RSquareBracket},
            {",", TokenType.Comma},
            {";", TokenType.Breaker},
        };
        public static Dictionary<string, TokenType> Keywords { get; private set; } = new Dictionary<string, TokenType>()
        {
            {"if", TokenType.Conditional},

            {"true", TokenType.Bool},
            {"false", TokenType.Bool},
            {"sin", TokenType.Sin},
            {"cos", TokenType.Cos},
            {"not", TokenType.Not},

            //Objectives
            {"Self", TokenType.Objective},

            //Powers
            {"Damage", TokenType.Power},
        };

        public static void RegistrerOperator(string op, TokenType type)
        {
            Symbols.Add(op, type);
        }
        public static void RegistrerKeyword(string keyword, TokenType type)
        {
            Keywords.Add(keyword, type);
        }

        public static bool MatchOperator(string op)
        {
            return Symbols.ContainsKey(op);
        }

        public static bool MatchKeyword(string keyword)
        {
            return Keywords.ContainsKey(keyword);
        }
    }
}