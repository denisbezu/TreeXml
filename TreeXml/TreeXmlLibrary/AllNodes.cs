using System.Collections.Generic;

namespace TreeXmlLibrary
{
    public class AllNodes<T> where T:class 
    {
        public static List<T> Nodes { get; set; }

        static AllNodes()
        {
            Nodes = new List<T>();
        }

    }
}