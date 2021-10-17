namespace ProjectCS
{
    public interface IClientDataAccess
    {
        Client[] GetAll(); //Return list client
        Client CreateUser(); //Create a Client
        Client GetClient(int guid); //Get the client with guid
        void UpdateClient(Client c); //Update client
        void DeleteClient(int guid); //Delete the client with guid
    }
}