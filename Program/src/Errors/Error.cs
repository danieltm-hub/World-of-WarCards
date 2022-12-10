using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class Error
    {
        public ErrorCode Code {get; private set;}

        public string Argument {get; private set;}

        public CodeLocation Location {get; private set;}

        public Error(ErrorCode code, CodeLocation location, string argument)
        {
            Code = code;
            Location = location;
            Argument = argument;
        }
    }

    public enum ErrorCode
    {
        Expected,
        Invalid,
    }

    public class Default
    {
        public Default(){}
    }
}