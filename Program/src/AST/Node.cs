using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AST
{
    public struct CodeLocation{
        string File;
        int Line;
        int Column;

        public CodeLocation (string file, int line, int column)
        {
            File = file;
            Line = line;
            Column = column;
        }
    }

    public abstract class Node 
    {
        public abstract bool CheckSemantic(List<Error> errors);
       
        public virtual NodeType Type { get; set; }

        protected CodeLocation Location;

        public Node(CodeLocation location)
        {
            Location = location;
        }
    }
    public enum NodeType
    {
        Text,
        Number,
        Error,
        Bool,

        Action,
        Objective,
        Power,

    }


}