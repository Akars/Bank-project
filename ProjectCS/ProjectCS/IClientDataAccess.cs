using System.Collections.Generic;

namespace ProjectCS
{
    public interface IClientDataAccess
    {
        List<Client> GetAll(); //Return list client
        Client CreateUser(); //Create a Client
        Client GetClient(string guid); //Get the client with guid
        void UpdateClient(Client c); //Update client
        void DeleteClient(int guid); //Delete the client with guid
    }
}