using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Linq;
using System.Data.SQLite;
using System.Configuration;
using System.Data;
using Dapper;


namespace ProjectCS
{
    public class ClientDBAccess : IClientDataAccess
    {
        public Client GetClient(string guid)
        {
            SQLiteConnection cnn = LoadConnection();
            var outputClient = cnn.Query<Client>("select * from client where guid = " + "\"" + guid + "\"", new DynamicParameters());

            return outputClient.Single();
        }

        public List<Client> GetAll()
        {
            SQLiteConnection cnn = LoadConnection();
            var outputClients = cnn.Query<Client>("select * from Client", new DynamicParameters());
            
            var clients = outputClients.ToList();

            var queryCurrency = "Select * from Currencies where guid = ";
            foreach (var client in clients)
            {
                client.currencies = cnn.Query<Currency>(queryCurrency + "\"" + client.guid + "\"", new DynamicParameters()).ToList();
            }
            
            return clients.ToList();
        }

        public Client CreateUser()
        {
            SQLiteConnection cnn = LoadConnection();
            
            Console.WriteLine("Fill the information:");
            Console.WriteLine("Firstname: ");
            string firstname = Console.ReadLine();
            Console.WriteLine("Lastname: ");
            string lastname = Console.ReadLine();
            Client client = new Client(firstname, lastname);
            Console.WriteLine("Welcome " + firstname);
            Console.WriteLine("Your ID is : " + client.guid);
            Console.WriteLine("Your pin will be displayed once keep it secret : " + client.pin);
            Console.WriteLine("The default currency is set in : " + client.currency);
            
            Console.WriteLine("Do you want to add some amount of money ? (y/n)");
            string choice = App.Read();

            if (choice == "y")
            {
                while (true)
                {
                    Console.WriteLine("Currency:");
                    string currency = Console.ReadLine();
                    if (!App.valideCurrency(currency))
                    {
                        Console.WriteLine("Wrong currency");
                        continue;
                    }
                    Console.WriteLine("Amount:");
                    int amount = Console.Read();
                    while (amount < 0)
                    {
                        Console.WriteLine("Wrong input");
                        amount = Console.Read();
                    }
                    
                    Console.WriteLine(amount + " " + currency + " is added to your account.");
                    client.currencies[client.currencies.Count] = new Currency(currency, amount);
                    
                    Console.WriteLine("Do you want to continue ? (y/n)");
                    choice = App.Read();
                    if (choice == "n")
                    {
                        break;
                    }
                }
            }

            if (client.currencies.Count > 0)
            {
                foreach(var money in client.currencies)
                {
                    cnn.Execute("insert into Currencies (currency, amount, guid) values(@currency, @amount, @guid)",
                        new {currency = money.currency, amount = money.amount, guid = client.guid});
                }
            }
            
            cnn.Execute("insert into Client (guid, firstname, lastname, pin, currency) values (@guid, @firstname, @lastname, @pin, @currency)", client);

            return client;
        }

        public void DeleteClient(string guid)
        {
            SQLiteConnection cnn = LoadConnection();
            cnn.Execute("delete from client where guid = " + "\"" + guid + "\"");
            cnn.Execute("delete from currencies where guid = " + "\"" + guid + "\"");
        }

        public void UpdateClient(Client c)
        {
            SQLiteConnection cnn = LoadConnection();
            cnn.Execute("update client set firstname=" 
                        + "'" + c.firstname 
                        + "', lastname='" + c.lastname 
                        + "', pin='" + c.pin 
                        +"', currency='" + c.currency 
                        + "' where guid = " + "\"" + c.guid + "\""
                        );
            foreach (var money in c.currencies)
            {
                cnn.Execute("update currencies set currency=" + "'" + money.currency + "', amount='" + "' where guid = " + "\"" + c.guid + "\"");
            }
        }

        public static SQLiteConnection LoadConnection()
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = new SQLiteConnection("Data Source = ./AtmDB.db; Version=3;New=True;Compress=True;");
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return sqlite_conn;
        }
    }
}