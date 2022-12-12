using System;
using System.IO;
using AST;

namespace Compiler
{
    public class Parser
    {
        public TokenStream Reader { get; private set; }

        public Parser(TokenStream reader)
        {
            Reader = reader;
        }

        public WarCardProgram ParseProgram(List<Error> CompilerErrors)
        {
            WarCardProgram program = new WarCardProgram(new CodeLocation());

            if (Reader.OUT()) return program;

            while (!Reader.OUT())
            {
                if (Reader.Match(TokenType.Card))
                {
                    Card card = ParseCard(CompilerErrors);
                    program.Add(card);
                }
            }

            return program;
        }

        public Card ParseCard(List<Error> CompilerErrors)
        {
            //current token is Card
            string name = "";
            Effector effect = null;
            CodeLocation location = Reader.Peek().Location;
            Reader.MoveNext();

            //name
            if (Reader.Match(TokenType.Card))
            {
                name = Reader.GetToken().Value;
                Reader.MoveNext();
            }
            else
            {
                CompilerErrors.Add(new Error(ErrorCode.Expected, Reader.Peek().Location, "Card Name"));
            }

            // {
            if (Reader.Match(TokenType.LSquareBracket))
            {
                Reader.MoveNext();
            }
            else
            {
                CompilerErrors.Add(new Error(ErrorCode.Expected, Reader.Peek().Location, "{"));
            }

            //Powers (here I have two options, reads to ; or reads to '\n', now I choose to read to ';')
            Effect = ParseEffector(CompilerErrors);

            // }
            if (!Reader.Match(TokenType.RSquareBracket))
            {
                CompilerErrors.Add(new Error(ErrorCode.Expected, Reader.Peek().Location, "}"));
            }

            return new Card(name, effect, Reader.Peek().Location);
        }

        public Effector ParseEffector(List<Error> CompilerErrors)
        {
            //first token is a Power
            while (!Reader.END && !Reader.Match(TokenType.Power))
            {
                CompilerErrors.Add(new Error(ErrorCode.Invalid, Reader.Peek().Location, Reader.Peek().Value));
                MoveNext();
            }

            if (Reader.Match(TokenType.Power))
            {
                Power power = ParsePower(CompilerErrors);
            }


            //second token is Objective
            while (!Reader.END && !Reader.Match(TokenType.Objective))
            {
                CompilerErrors.Add(new Error(ErrorCode.Invalid, Reader.Peek().Location, Reader.Peek().Value));
                MoveNext();
            }
        }

    }
}