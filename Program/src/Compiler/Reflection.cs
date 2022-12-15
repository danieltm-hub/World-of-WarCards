using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using AST;

namespace Compiler
{
    public static class Reflection
    {
        public static object Reflect(string key, List<Expression> parameters, CodeLocation location)
        {
            if (!TypeStore.ContainsKey(key)) throw new Exception($"Key: {key} is not in Dictionary");

            Type myType = TypeStore[key];

            ConstructorInfo? constructor = myType.GetConstructor(new Type[] { typeof(List<Expression>), typeof(CodeLocation) });

            if (constructor == null) throw new Exception($"Cannot invoke {key}");

            return constructor.Invoke(new object[] { parameters, location });
        }


        static Dictionary<string, Type> TypeStore = new Dictionary<string, Type>
        {
            {"damage", typeof(ModifyHealth)},
            {"self", typeof(Self)},

        };
    }

}