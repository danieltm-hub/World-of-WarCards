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
        public static object Reflect(string key, List<Node> parameters, CodeLocation location)
        {
            if (!TypeStore.ContainsKey(key)) throw new Exception($"Key: {key} is not in Dictionary");

            Type myType = TypeStore[key];

            ConstructorInfo? constructor = myType.GetConstructor(new Type[] { typeof(List<Node>), typeof(CodeLocation) });

            if (constructor == null) throw new Exception($"Cannot invoke {key}");

            return constructor.Invoke(new object[] { parameters, location });
        }

        public static void RegisterDll(Assembly assembly)
        {
            string Separator = '\n' + new string('=', 50) + '\n';

            System.Console.WriteLine(Separator);
            System.Console.WriteLine(assembly.FullName + "  loading...");

            foreach (Type type in assembly.GetExportedTypes())
            {
                
                if (type.IsSubclassOf(typeof(Power)))
                {
                    System.Console.WriteLine(type.Name + " is a Power");

                    ConstructorInfo? classConstructor = type.GetConstructor(new Type[] { typeof(List<Node>), typeof(CodeLocation) });

                    if(classConstructor == null) throw new Exception($"Cannot invoke {type.Name}");

                    Power instance = (Power)classConstructor.Invoke(new object[] { new List<Node>(), new CodeLocation()});
                    
                    RegistrerType(instance.Keyword(), type);

                    LexicStore.RegistrerKeyword(instance.Keyword(), TokenType.Power);
                }

                else if (type.IsSubclassOf(typeof(Objective)))
                {
                    System.Console.WriteLine(type.Name + " is an Objective");

                    ConstructorInfo? classConstructor = type.GetConstructor(new Type[] { typeof(List<Node>), typeof(CodeLocation) });

                    if(classConstructor == null) throw new Exception($"Cannot invoke {type.Name}");

                    Objective instance = (Objective)classConstructor.Invoke(new object[] { new List<Node>(), new CodeLocation()});
                    
                    RegistrerType(instance.Keyword(), type);

                    LexicStore.RegistrerKeyword(instance.Keyword(), TokenType.Objective);
                }

                else
                {
                    System.Console.WriteLine(type.Name + " is not a Power or Objective");
                }
            }

            System.Console.WriteLine(Separator);            
        }

        public static void RegistrerType(string keyword, Type type)
        {
            if(TypeStore.ContainsKey(keyword)) throw new Exception($"Key: {keyword} is already in Dictionary");
            TypeStore.Add(keyword, type);
        }

        static Dictionary<string, Type> TypeStore = new Dictionary<string, Type>
        {
            
        };
    }

}