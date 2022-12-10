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

    public enum TokenType
    {
        Number,
        Text,
        Keyword,
        ID,
        Symbol,
    }
}