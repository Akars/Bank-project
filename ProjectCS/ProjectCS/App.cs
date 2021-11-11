using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using Dapper;

namespace ProjectCS
{
    public class App
    {
        private List<Client> clients;
        public static List<CurrencyType> currencyTypes;
        private Admin admin = new Admin();
        private bool isAdmin;
        private Client connectedClient = new Client();
        private ClientDBAccess dbb = new ClientDBAccess();
        private ClientJsonAccess JsonAccess = new ClientJsonAccess();

        public App(ClientJsonAccess jsonAccess)
        {
            clients = jsonAccess.GetAll();
        }
        
        public App()
        {
            clients = dbb.GetAll();
            currencyTypes = ApiAccess.GetAllCurrencies();
        }
        
        
        public Client ConnectClient()
        {
            string guid;
            int pin;
            Console.Write("GUID: ");
            guid = Console.ReadLine();
            Client c = clients.Find(x => x.guid == guid);
            while (c == null || c.isBlocked == 1)
            {
                Console.WriteLine("The client doesn't exist or is blocked.");
                Console.WriteLine("GUID: ");
                guid = Console.ReadLine();
                c = clients.Find(x => x.guid == guid);
            }
            Console.Write("PIN: ");
            pin = int.Parse(App.Read(1000, 9999));
            while (!c.ClientCredential(pin))
            {
                Console.WriteLine(pin);
                if (c.isBlocked == 1)
                {
                    Console.WriteLine("Your account is blocked.");
                    return null;
                }
                Console.WriteLine("Wrong pin, remain try: " + c.codeTry);
                Console.WriteLine("PIN: ");
                pin = int.Parse(App.Read(1000, 9999));
            }

            return c;
        }
        
        //Secure read min - max parameters
        public static string Read(int min, int max)
        {
            string choice;
            int value;
            do
            {
                choice = Console.ReadLine();
                while (!int.TryParse(choice, out value))
                {
                    Console.WriteLine("Not a number."); 
                    choice = Console.ReadLine();
                }
            } while (int.Parse(choice) < min || int.Parse(choice) > max || string.IsNullOrEmpty(choice));

            return choice;
        }
        
        //Secure read y/n
        public static string Read()
        {
            string choice;
            do
            {
                choice = Console.ReadLine();
            } while (string.IsNullOrEmpty(choice) || choice.ToLower() != "y" && choice.ToLower() != "n");

            return choice;
        }

        public void Start()
        {
            Console.WriteLine("Hello welcome to the bank !");
            bool key = true;
            while (key)
            {
                Menu.PrintWelcomeMsg();
                string choice;
                choice = Read(0, 3);
                switch (choice)
                {
                    case "0":
                        Console.WriteLine("See you!");
                        key = false;
                        break;
                    case "1":
                        AdminStart();
                        break;
                    case "2":
                        ClientStart();
                        break;
                }
            }
            
        }

        public void AdminStart()
        {
            string username;
            string password;
            Console.Write("Username: ");
            username = Console.ReadLine();
            Console.Write("Password: ");
            password = Console.ReadLine();
            isAdmin = admin.AdminCredential(username, password);
            while (!isAdmin)
            {
                Console.WriteLine("Wrong username or password");
                Console.Write("Username: ");
                username = Console.ReadLine();
                Console.Write("Password: ");
                password = Console.ReadLine();
                isAdmin = admin.AdminCredential(username, password);
            }

            bool key = true;
            Console.WriteLine("\n");
            Console.WriteLine("Hello admin.");
            while (key)
            {
                Menu.AdminMenu();
                string choice;
                string guid;
                choice = Read(0, 3);
                Console.WriteLine("\n");
                switch (choice)
                {
                    case "0":
                        Console.WriteLine("Good bye admin!");
                        Console.WriteLine("\n");
                        key = false;
                        break;
                    case "1":
                        dbb.CreateUser();
                        break;
                    case "2":
                        Console.WriteLine("Select the client.");
                        Console.WriteLine("GUID: ");
                        guid = Console.ReadLine();
                        Client c = clients.Find(x => x.guid == guid);
                        while (c == null)
                        {
                            Console.WriteLine("The client doesn't exist");
                            Console.WriteLine("GUID: ");
                            guid = Console.ReadLine();
                            c = clients.Find(x => x.guid == guid);
                        }
                        ManageClient(c);
                        Console.WriteLine("\n");
                        break;
                    case "3":
                        Console.WriteLine(ToString());
                        Console.WriteLine("\n");
                        break;
                }
            }
        }

        public void ClientStart()
        {
            connectedClient = ConnectClient();
            if (connectedClient.guid == null)
            {
                return;
            }

            bool key = true;
            Console.WriteLine("\n");
            Console.WriteLine("Hello " + connectedClient.firstname);
            while (key)
            {
                Menu.ClientMenu();
                string choice;
                choice = Read(0, 6);
                double amount;
                string currency;
                switch (choice)
                {
                    case "0":
                        Console.WriteLine("Good bye {0}!", connectedClient.firstname);
                        Console.WriteLine("\n");
                        dbb.UpdateClient(connectedClient);
                        key = false;
                        break;
                    case "1":
                        Console.WriteLine("Your GUID: {0}", connectedClient.guid);
                        Console.WriteLine("\n");
                        break;
                    case "2":
                        Console.Write("Seize the preferred currency: ");
                        currency = ReadCurrency();
                        Console.WriteLine(connectedClient.ToStringPreferredCurrency(currency.ToUpper()));
                        Console.WriteLine("\n");
                        break;
                    case "3":
                        Console.Write("Seize the preferred currency: ");
                        currency = ReadCurrency();
                        Console.Write("How much do you want to retrieve: ");
                        amount = double.Parse(Console.ReadLine());
                        Console.WriteLine("\n");
                        connectedClient.RetrieveMoney(currency.ToUpper(), amount);
                        Console.WriteLine("\n");
                        break;
                    case "4":
                        Console.Write("Seize the preferred currency: ");
                        currency = ReadCurrency();
                        Console.Write("How much do you want to add: ");
                        amount = double.Parse(Console.ReadLine());
                        Console.WriteLine("\n");
                        connectedClient.AddMoney(currency.ToUpper(), amount);
                        Console.WriteLine("\n");
                        break;
                    case "5":
                        Console.Write("Old pin: ");
                        int code = int.Parse(App.Read(1000, 9999));
                        Console.WriteLine("\n");
                        while (!connectedClient.ClientCredential(code))
                        {
                            if (connectedClient.isBlocked == 1)
                            {
                                return;
                            }
                            Console.WriteLine("Wrong pin");
                            code = int.Parse(App.Read(1000, 9999));
                        }
                        Console.WriteLine("\n");
                        Console.Write("New pin: ");
                        code = int.Parse(App.Read(1000, 9999));
                        Console.WriteLine();
                        connectedClient.pin = code;
                        Console.WriteLine("Password changed.");
                        break;
                    case "6":
                        string newCurrency;
                        Console.Write("Currency to be exchanged: ");
                        currency = ReadCurrency();
                        Console.Write("Exchange " + currency + " to ");
                        newCurrency = ReadCurrency();
                        Console.WriteLine();
                        Console.Write("Amount to be converted: ");
                        amount = double.Parse(Console.ReadLine());
                        if (connectedClient.RetrieveMoney(currency, amount))
                        {
                            connectedClient.AddMoney(newCurrency, ApiAccess.ExchangeRate(currency, newCurrency) * amount);
                        }
                        Console.WriteLine("\n");
                        break;
                }
            }
        }

        public void ManageClient(Client client)
        {
            Console.WriteLine("Manage " + client.guid);
            Console.WriteLine();
            bool key = true;
            while (key)
            {
                Menu.PrintManage();
                string choice;
                choice = Read(0, 5);
                switch (choice)
                {
                    case "0":
                        dbb.UpdateClient(client);
                        Console.WriteLine("The client is updated.");
                        key = false;
                        return;
                    case "1":
                        if (client.isBlocked != 1)
                        {
                            Console.WriteLine("The client is not blocked");
                        }
                        else
                        {
                            client.isBlocked = 0;
                            goto case "4";
                        }
                        break;
                    case "2":
                        if (client.isBlocked == 1)
                        {
                            Console.WriteLine("The client is already blocked");
                        }
                        else
                        {
                            client.isBlocked = 1;
                        }
                        break;
                    case "3":
                        Console.Write("New pin: ");
                        int newPin = int.Parse(Read(1000, 9999));
                        client.pin = newPin;
                        break;
                    case "4":
                        if (client.isBlocked == 1)
                        {
                            Console.WriteLine("The client is blocked, please unblock him first.");
                        }
                        else
                        {
                            client.codeTry = 3;
                        }
                        break;
                    case "5":
                        Console.WriteLine("Are you sure to delete the client ? (y/n)");
                        string tmp = Read();
                        if (tmp == "y")
                        {
                            dbb.DeleteClient(client.guid);
                        }
                        break;
                }
            }
            
        }

        public static string ReadCurrency()
        {
            string currency;
            currency = Console.ReadLine();
            while (!ValideCurrency(currency))
            {
                Console.WriteLine("Wrong currency, please seize the correct currency: ");
                currency = Console.ReadLine();
            }

            return currency;
        }
        //Validate currency input (not working)
        public static bool ValideCurrency(string currency)
        {
            return currencyTypes.Exists(x => x.Id == currency);
        }
        
        //Print list clients
        public override string ToString()
        {
            Console.WriteLine("Result:");
            var sb = new System.Text.StringBuilder();
            sb.Append(String.Format("{0, 20} {1, 46} {2, 25} {3,15} {4,16}\n\n", "Guid", "Fullname", "Currency", "Try", "Blocked"));
            foreach (var client in clients)
            {
                var fullName = client.firstname + " " + client.lastname;
                sb.Append(String.Format("{0, 20} {1, 30} {2, 25:P1} {3,15} {4,16}\n", client.guid, fullName, client.currency, client.codeTry, client.isBlocked));
            }

            return sb.ToString();
        }
    }
}