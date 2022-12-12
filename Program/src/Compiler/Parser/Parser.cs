using System;
using System.IO;
using AST;

namespace Compiler
{
    public class Parser
    {
        public TokenStream Reader { get; private set; }

        public Parser(TokenReader reader)
        {
            Reader = reader;
        }

        public WarCardProgram ParseProgram()
        {
            WarCardProgram program = new WarCardProgram(Reader.Location);

            if (Reader.OUT()) return program;

            while (!Reader.OUT())
            {
                
            }
        }
    }
}