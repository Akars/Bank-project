using System;
using System.Collections.Generic;


namespace ProjectCS
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            // int guid = Guid.NewGuid().GetHashCode();
            // Console.WriteLine(guid);
            //
            // var jsonAccess = new ClientJsonAccess();
            // Console.WriteLine(jsonAccess.GetClient("293e4d46-5dff-4645-bd4c-b5365548b567"));
            // Console.WriteLine(jsonAccess.GetClient("293e4d46-5dff-4645-bd4c-b5365548b567").firstname);
            // jsonAccess.GetAll();
            //

            var dbAccess = new ClientDBAccess();
            //Console.WriteLine(dbAccess.GetAll()[0].guid);
            //Console.WriteLine(dbAccess.GetClient("293e4d46-5dff-4645-bd4c-b5365548b567").firstname);
            //Console.WriteLine(App.valideCurrency("EUR"));
            Client client = dbAccess.CreateUser();
            client.pin = 2345;
            dbAccess.UpdateClient(client);
            dbAccess.DeleteClient("6475cc94-e137-425c-b2f4-ce7eaae317b4");
        }
    }
}