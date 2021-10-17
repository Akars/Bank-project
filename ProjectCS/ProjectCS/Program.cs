using System;
using System.Collections.Generic;


namespace ProjectCS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Client c = new Client();
            JsonHelper json = new JsonHelper();
            
            json.Deserialize();
        }
    }
}