using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compiler
{
    public static class Analyzer
    {
        public static List<Token> GetTokens(string source)
        {
            List<Token> tokens = new List<Token>();

            source = source.ToLower();

            int line = 1;
            int pos = 0;

            while (pos < source.Length)
            {

            }


            return tokens;
        }

        

      
    }
}