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
    }

    public abstract class Node 
    {
        public abstract bool CheckSemantic(List<Error> errors);
        protected CodeLocation Location;

        public Node(CodeLocation location)
        {
            Location = location;
        }
    }
}