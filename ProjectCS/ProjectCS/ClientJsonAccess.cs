using System.Collections.Generic;
using System.IO;

namespace ProjectCS
{
    public class ClientJsonAccess : IClientDataAccess
    {
        public List<Client> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Client CreateUser()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteClient(int guid)
        {
            throw new System.NotImplementedException();
        }

        public Client GetClient(string guid)
        {
            var myJsonString = File.ReadAllText(@"../../../Data/data.json");
            var jsObject = JsonHelper.DeserializeFromJson<List<Client>>(myJsonString);

            Client c = jsObject.Find(client => client.id == guid);
            
            return c;
        }

        public void UpdateClient(Client c)
        {
            throw new System.NotImplementedException();
        }
    }
}