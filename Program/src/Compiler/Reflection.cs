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
        static string Separator = "\n" + new string('=', 112) + "\n";
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

            System.Console.WriteLine(Separator + path + " : " + assembly.FullName + " Loading ...\n");

            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(Power)))
                {
                    System.Console.Write($"{type.Name} is a Power :");

                    ConstructorInfo? classConstructor = type.GetConstructor(new Type[] { typeof(List<Expression>), typeof(CodeLocation) });

                    if (classConstructor == null) throw new Exception($"Cannot invoke {type.Name}");

                    Power instance = (Power)classConstructor.Invoke(new object[] { new List<Expression>(), new CodeLocation() });

                    RegistrerType(instance.Keyword(), type);

                    LexicStore.RegistrerKeyword(instance.Keyword(), TokenType.Power);

                    System.Console.WriteLine();
                }

                if (type.IsSubclassOf(typeof(Objective)))
                {
                    System.Console.WriteLine($"{type.Name} is a Objective : ");

                    ConstructorInfo? classConstructor = type.GetConstructor(new Type[] { typeof(List<Expression>), typeof(CodeLocation) });

                    if (classConstructor == null) throw new Exception($"Cannot invoke {type.Name}");

                    Objective instance = (Objective)classConstructor.Invoke(new object[] { new List<Expression>(), new CodeLocation() });

                    RegistrerType(instance.Keyword(), type);

                    LexicStore.RegistrerKeyword(instance.Keyword(), TokenType.Objective);

                    System.Console.WriteLine();
                }

            }
            System.Console.WriteLine("\n" + assembly.FullName + " Loaded " + Separator);
        }

        public static void RegistrerType(string keyword, Type type)
        {
            if (TypeStore.ContainsKey(keyword)) throw new Exception($"Key: {keyword} is already in Dictionary");
            System.Console.WriteLine($"Registered {keyword} as {type.Name} Complete");
            TypeStore.Add(keyword, type);
        }

        static Dictionary<string, Type> TypeStore = new Dictionary<string, Type>
        {

        };
    }

}