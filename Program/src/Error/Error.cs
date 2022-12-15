using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public class Error
    {
        public ErrorCode Argument { get; private set; }

        public string Code { get; private set; }

        public CodeLocation Location { get; private set; }

        public Error(ErrorCode argument, CodeLocation location, string code )
        {
            Code = code;
            Location = location;
            Argument = argument;
        }

        public override string ToString()
        {
            return $"Error: {Argument} {Code} at line:{Location.Line} Column:{Location.Column} in file:{Location.File}";
        }
    }

    public enum ErrorCode
    {
        Expected,
        Invalid,
    }

}