using System;
using System.Collections.Generic;


namespace ProjectCS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int guid = Guid.NewGuid().GetHashCode();
            Console.WriteLine(guid);

            var jsonAccess = new ClientJsonAccess();
            Console.WriteLine(jsonAccess.GetClient("293e4d46-5dff-4645-bd4c-b5365548b567"));
            Console.WriteLine(jsonAccess.GetClient("293e4d46-5dff-4645-bd4c-b5365548b567").firstname);
        }
    }
}