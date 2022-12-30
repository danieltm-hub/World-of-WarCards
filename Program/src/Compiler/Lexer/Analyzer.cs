using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST;

namespace Compiler
{
    public class Analyzer
    {
        public static List<Token> GetTokens(string filename, string source, List<Error> errorCode)
        {
            List<Token> tokens = new List<Token>();

            source = source.ToLower(); // our language is case insensitive

            TokenReader reader = new TokenReader(filename, source);

            while (!reader.EOF)
            {
                string value = "";

                if (reader.ReadWhiteSpace()) continue;

                if (reader.Match("-", false) && !MatchPrevious(tokens, TokenType.Number, TokenType.RParen))
                {
                    reader.Match("-");
                    if (reader.ReadNumber(out value))
                    {
                        //Is necessary a CheckNumber Sintax?
                        tokens.Add(new Token(TokenType.Number, "-" + value, reader.Location));
                        continue;
                    }
                    else
                        reader.ReadBack();
                }

                if (reader.ReadNumber(out value))
                {
                    //Same question
                    tokens.Add(new Token(TokenType.Number, value, reader.Location));
                    continue;
                }

                if (reader.ReadText(out value))
                {
                    tokens.Add(new Token(TokenType.Text, value, reader.Location));
                    continue;
                }

                if (MatchSymbol(reader, tokens))
                {
                    continue;
                }

                if (reader.ReadID(out value))
                {
                    if (LexicStore.MatchKeyword(value))
                        tokens.Add(new Token(LexicStore.Keywords[value], value, reader.Location));

                    else
                        tokens.Add(new Token(TokenType.ID, value, reader.Location));

                    continue;
                }

                value = reader.ReadAny().ToString();
                errorCode.Add(new Error(ErrorCode.Invalid, reader.Location, value));
            }

            return tokens;
        }


        private static bool MatchPrevious(List<Token> tokens, params TokenType[] expected)
        {
            if (tokens.Count == 0) return false;

            foreach (TokenType type in expected)
            {
                if (tokens[tokens.Count - 1].Type == type) return true;
            }

            return false;
        }

        private static bool MatchSymbol(TokenReader reader, List<Token> tokens)
        {
            foreach (var symbol in LexicStore.Symbols.Keys)
            {
                if (reader.Match(symbol))
                {
                    tokens.Add(new Token(LexicStore.Symbols[symbol], symbol, reader.Location));
                    return true;
                }
            }

            return false;
        }




    }
}