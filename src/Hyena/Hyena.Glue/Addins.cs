using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Mono.Addins
{
    // Stub code
    // I don't actually know how Mono.Addins works
    static public class AddinManager
    {
        public static bool IsInitialized => false;

        public static List<TypeExtensionNode> GetExtensionNodes(string exPoint)
        {
            Console.WriteLine($"Extension Point Accessed: {exPoint} - Not Implemented");
            return new List<TypeExtensionNode>();
        }
    }

    public class TypeExtensionNode
    {
        public bool HasId => false;
        public int Id => 0;

        public object CreateInstance(Type t) => throw new NotImplementedException("Mono.Addins is not implemented! See Hyena.Glue/Addins.cs");
    }
}