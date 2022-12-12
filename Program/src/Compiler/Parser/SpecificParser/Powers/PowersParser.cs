using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using AST;

namespace Compiler
{/*
    public static class ParserStore
    {
        
        static Dictionary<string, MethodInfo> ParsePower = new Dictionary<Power, MethodInfo>(){
            {"damage", Type.GetMethods(ParseDamage) },
        };

        public static Power ParseDamage(out TokenStream stream, out List<Error> errors)
        {
            //current token is Damage
            stream.MoveNext();

            // (
            if (stream.Match(TokenType.LParen))
            {
                stream.MoveNext();
            }
            else
            {
                errors.Add(new Error(ErrorCode.Expected, stream.Peek().Location, "("));
            }

            //Expression
        }      

    }*/
}