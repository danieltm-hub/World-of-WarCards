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

        public static void RegisterDll(string path)
        {
            Assembly assembly = Assembly.LoadFrom(path);
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(Power)))
                {
                    ConstructorInfo? classConstructor = type.GetConstructor(new Type[] { typeof(List<Expression>), typeof(CodeLocation) });

                    if(classConstructor == null) throw new Exception($"Cannot invoke {type.Name}");

                    Power instance = (Power)classConstructor.Invoke(new object[] { new List<Expression>(), new CodeLocation()});
                    
                    TypeStore.Add(instance.Keyword(), type);

                    LexicStore.Keywords.Add(instance.Keyword(), TokenType.Power);
                }

                if (type.IsSubclassOf(typeof(Objective)))
                {
                    ConstructorInfo? classConstructor = type.GetConstructor(new Type[] { typeof(List<Expression>), typeof(CodeLocation) });

                    if(classConstructor == null) throw new Exception($"Cannot invoke {type.Name}");

                    Objective instance = (Objective)classConstructor.Invoke(new object[] { new List<Expression>(), new CodeLocation()});
                    
                    TypeStore.Add(instance.Keyword(), type);

                    LexicStore.Keywords.Add(instance.Keyword(), TokenType.Objective);
                }
            }

            foreach(string key in TypeStore.Keys)
            {
                Console.WriteLine($"{key} : {TypeStore[key]}");
            }

            
        }


        static Dictionary<string, Type> TypeStore = new Dictionary<string, Type>
        {
            
        };
    }

}