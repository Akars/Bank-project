using System;
using System.Collections.Generic;

namespace ProjectCS
{
    public class App
    {
        private List<Client> clients;
        private Admin admin;
        private bool isAdmin;
        private Client connectedClient;

        public App(ClientJsonAccess jsonAccess)
        {
            clients = jsonAccess.GetAll();
        }
        
        

        public Client ConnectClient()
        {
            return null;
        }

        
    }
}