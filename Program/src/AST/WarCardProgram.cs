using System;

namespace AST
{
    public class WarCardProgram : Node
    {
        public List<Error> Errors{get; private set;}
        //aqui vendrian los diccionarios correspondientes
        
        public ElementalProgram(CodeLocation location) : base(location)
        {
            Errors = new List<Error>();
            //aqui vendrian la inicializacion de los diccionarios correspondientes
        }
        

    }
}