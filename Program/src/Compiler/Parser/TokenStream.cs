using System;
using System.Collections.Generic;
using AST;

namespace Compiler
{
    public class TokenStream
    {
        public List<Token> Tokens { get; private set; }
        public int pos { get; private set; }

        public TokenStream(List<Token> tokens)
        {
            Tokens = tokens;
            pos = -1;
        }
    
        public bool END => pos >= Tokens.Count - 1;
        public bool Start => pos == 0;
        public bool OUT(int k = 0) => (pos + k >= Tokens.Count || pos + k < 0);
        public bool MoveNext(int k = 1)
        {
            if (OUT(k)) return false;
            pos += k;
            return true;
        }
        public bool MoveBack(int k = 1)
        {
            if (OUT(-k)) return false;
            pos -= k;
            return true;
        }

        public bool Match(TokenType type, bool pass = true)
        {
            if (!OUT(1) && Peek(1).Type == type)
            {
                pos += (pass) ? 1 : 0;
                return true;
            }

            return false;
        }

        public Token Peek(int k = 0)
        {
            if (OUT(k)) throw new Exception($"Index {pos + k} OUT in Peek, TokenStream");

            return Tokens[pos + k];
        }
    }
}