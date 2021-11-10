using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectCS
{
    public class ClientJsonAccess : IClientDataAccess
    {
        public List<Client> GetAll()
        {
            var myJsonString = File.ReadAllText(@"../../../Data/data.json");
            var jsObject = JsonHelper.DeserializeFromJson<List<Client>>(myJsonString);
            Console.WriteLine("---------------------------------------------------------------");
            foreach (var client in jsObject)
            {
                Console.WriteLine("Client GUID: " + client.guid);
                Console.WriteLine("Nom:  " + client.firstname + " " + client.lastname);
                Console.WriteLine("Main currency: " + client.currency);
                Console.WriteLine("---------------------------------------------------------------");
            }

            return jsObject;
        }

        public Client CreateUser()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteClient(string guid)
        {
            throw new System.NotImplementedException();
        }

        public Client GetClient(string guid)
        {
            var myJsonString = File.ReadAllText(@"../../../Data/data.json");
            var jsObject = JsonHelper.DeserializeFromJson<List<Client>>(myJsonString);

            Client c = jsObject.Find(client => client.guid == guid);
            
            return c;
        }

        public void UpdateClient(Client c)
        {
            var myJsonString = File.ReadAllText(@"../../../Data/data.json");
            var jsObject = JsonHelper.DeserializeFromJson<List<Client>>(myJsonString);
            
        }
    }
}