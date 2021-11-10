using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;

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

        public static string Read(int min, int max)
        {
            string choice;
            do
            {
                choice = Console.ReadLine();
            } while (choice.GetHashCode() < min || choice.GetHashCode() > max || string.IsNullOrEmpty(choice));

            return choice;
        }
        public static string Read()
        {
            string choice;
            do
            {
                choice = Console.ReadLine();
            } while (string.IsNullOrEmpty(choice) || choice.ToLower() != "y" && choice.ToLower() != "n");

            return choice;
        }

        public static bool valideCurrency(string currency)
        {
            SQLiteDataReader sqLiteDataReader;
            SQLiteCommand sqLiteCommand;
            sqLiteCommand = ClientDBAccess.LoadConnection().CreateCommand();
            sqLiteCommand.CommandText = "Select 1 from currency where currency = " + "\"" + currency + "\"";
            sqLiteDataReader = sqLiteCommand.ExecuteReader();
            

            return false;
        }
    }
}