using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProjectCS
{
    public class ClientDBAccess : IClientDataAccess
    {
        public Client GetClient(string guid)
        {
            
            throw new System.NotImplementedException();
        }

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

        public void UpdateClient(Client c)
        {
            throw new System.NotImplementedException();
        }
    }
}