using System;

namespace AST
{
    public class WarCardProgram : Node
    {
        public List<Error> Errors{get; private set;}
        //aqui vendrian los diccionarios correspondientes
        
        public WarCardProgram(CodeLocation location) : base(location)
        {
            Errors = new List<Error>();
            //aqui vendrian la inicializacion de los diccionarios correspondientes
        }

        public override bool CheckSemantic(List<Error>errors)
        {
            throw new Exception("CheckSemantic was't implemented");
        }
        
    }
}