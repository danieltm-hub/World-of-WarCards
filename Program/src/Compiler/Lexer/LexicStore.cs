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

            //Composite Signs
            {"==", TokenType.Equal},
            {"!=", TokenType.NotEqual},
            {"<=", TokenType.SmallerEqual},
            {">=", TokenType.GreaterEqual},
            {"&&", TokenType.And},
            {"||", TokenType.Or},
            
            //Simple Signs
            {"+", TokenType.Sum},
            {"-", TokenType.Sub},
            {"d", TokenType.DRandom},
            {"*", TokenType.Mul},
            {"/", TokenType.Div},
            {"^", TokenType.Pow},
            {"=", TokenType.Assign},
            {"!", TokenType.Not},
            {"<", TokenType.Smaller},
            {">", TokenType.Greater},
            {"(", TokenType.LParen},
            {")", TokenType.RParen},

            {"[", TokenType.LSquareBracket},
            {"]", TokenType.RSquareBracket},
            {"{", TokenType.LBracket},
            {"}", TokenType.RBracket},
            {",", TokenType.Comma},
            {";", TokenType.Breaker},

            {"\"", TokenType.DoubleCommas},
        };
        public static Dictionary<string, TokenType> Keywords { get; private set; } = new Dictionary<string, TokenType>()
        {
            {"if", TokenType.Conditional},
            {"else", TokenType.Else},

            {"true", TokenType.Bool},
            {"false", TokenType.Bool},
            {"sin", TokenType.Sin},
            {"sen", TokenType.Sin},
            {"cos", TokenType.Cos},
            {"not", TokenType.Not},

            {"card", TokenType.Card},
            {"effector", TokenType.Effector},
            {"effect", TokenType.Effect},
        };

        public static void RegistrerOperator(string op, TokenType type)
        {
            Symbols.Add(op, type);
        }
        public static void RegistrerKeyword(string keyword, TokenType type)
        {
            if (Keywords.ContainsKey(keyword)) throw new Exception($"Keyword {keyword} already exists");
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

    public enum TokenType
    {
        // Atomic Expressions
        Number,
        Bool,
        Text,
        // Unary Operators
        Sin,
        Cos,
        Not,
        // Binary Operators
        Sum,
        Sub,
        DRandom,
        Mul,
        Div,
        Pow,
        Equal,
        NotEqual,
        Smaller,
        SmallerEqual,
        Greater,
        GreaterEqual,
        And,
        Or,

        LParen,
        RParen,
        LBracket,
        RBracket,
        LSquareBracket,
        RSquareBracket,
        Breaker,
        Comma,
        DoubleCommas,

        Ignore,
        Assign,

        Effector,
        Power,
        Objective,
        Effect,
        Conditional,
        Else,
        ID,
        Symbol,
        Epsilon,

        BreakLine,
        Card,

    }
}