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
            Client client = new Client();
            client.firstname = firstname;
            client.lastname = lastname;
            client.guid = Guid.NewGuid().ToString();
            client.pin = new Random().Next(1000, 9999);
            client.currencies = new List<Currency>();
            client.currency = "EUR";
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
                    string currency = App.ReadCurrency();
                    
                    Console.WriteLine("Amount:");
                    int amount = int.Parse(Console.ReadLine());
                    while (amount < 0)
                    {
                        Console.WriteLine("Wrong input");
                        amount = int.Parse(Console.ReadLine());
                    }
                    
                    Console.WriteLine(amount + " " + currency + " is added to your account.");
                    var money = new Currency();
                    money.amount = amount;
                    money.currency = currency;
                    client.currencies.Add(money);
                    
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
                        + "', currency='" + c.currency
                        + "', isBlocked='" + c.isBlocked
                        + "', codeTry='" + c.codeTry
                        + "' where guid = " + "\"" + c.guid + "\""
                        );
            foreach (var money in c.currencies)
            {
                cnn.Execute("update currencies set "
                            + "amount='" + money.amount
                            + "' where guid = " + "\"" + c.guid + "\""
                            + " and currency = " + "\"" + money.currency + "\"");
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