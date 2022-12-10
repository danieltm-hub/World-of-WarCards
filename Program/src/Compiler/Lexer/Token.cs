using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST;

namespace Compiler
{
    public class Token
    {
        public string Value { get; private set; }
        public TokenType Type { get; private set; }
        public CodeLocation Location { get; private set; }
        public Token(TokenType type, string value, CodeLocation location)
        {
            Type = type;
            Value = value;
            Location = location;
        }

        public override string ToString()
        {
            return string.Format($"{Type} {Value}");
        }

    }

    public class TokenReader
    {
        string Filename;
        string Code;
        int pos;
        int line;
        int lastLB;

        public TokenReader(string filename, string code)
        {
            Filename = filename;
            Code = code;
            pos = 0;
            line = 1;
            lastLB = -1;
        }

        public CodeLocation Location => new CodeLocation(Filename, line, pos - lastLB);

        public char Peek()
        {
            if (pos < 0 || pos >= Code.Length) throw new Exception("Index out in Peek in TokenReader");

            return Code[pos];
        }

        public bool EOF => pos >= Code.Length;

        public bool EOL => EOF || Code[pos] == '\n';

        public bool ContinuesWith(string prefix)
        {
            if (prefix.Length + pos > Code.Length) return false;

            for (int i = 0; i < prefix.Length; i++)
            {
                if (Code[pos + i] != prefix[i]) return false;
            }

            return true;
        }

        public bool Match (string prefix)
        {
            if (ContinuesWith(prefix))
            {
                pos += prefix.Length;
                return true;
            }
            return false;
        }

       //continuar el codigo, basandonos en lo que hizo el profe
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
        Mul,
        Div,
        Pow,
        Equal,
        Smaller,
        Greater,
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

        Ignore,

        Effector,
        Power,
        Objective,
        Conditional,
        ID,
        Symbol,
        Epsilon,

    }
}