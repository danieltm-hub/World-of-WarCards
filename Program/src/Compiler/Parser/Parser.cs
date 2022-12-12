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

        public WarCardProgram ParseProgram()
        {
            WarCardProgram program = new WarCardProgram(new CodeLocation());

            if (Reader.OUT()) return program;

            while (!Reader.OUT())
            {
               if(Reader.Match(TokenType.Card))
               {
                    //random messagen to commit
               } 
            }

            return program;
        }
    }
}