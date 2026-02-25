
using System;
using System.Reflection;
using Alachisoft.NCache.Client;

class ReflectionTest
{
    static void Main() {
        var type = typeof(ICache);
        Console.WriteLine($"Methods of {type.FullName}:");
        foreach (var method in type.GetMethods()) {
            if (method.Name.Contains("Async")) {
                Console.WriteLine(method.ToString());
            }
        }
    }
}
