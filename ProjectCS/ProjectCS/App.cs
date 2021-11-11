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
            Console.WriteLine("GUID: ");
            guid = Console.ReadLine();
            Client c = clients.Find(x => x.guid == guid);
            while (c == null && c.isBlocked)
            {
                Console.WriteLine("The client doesn't exist or is blocked.");
                Console.WriteLine("GUID: ");
                c = clients.Find(x => x.guid == guid);
            }
            Console.WriteLine("PIN: ");
            pin = int.Parse(App.Read(1000, 9999));
            Console.WriteLine(c.pin);
            while (!connectedClient.ClientCredential(pin))
            {
                Console.WriteLine(pin);
                if (c.isBlocked)
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
            do
            {
                choice = Console.ReadLine();
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
            Console.WriteLine("Username:");
            username = Console.ReadLine();
            Console.WriteLine("Password: ");
            password = Console.ReadLine();
            isAdmin = admin.AdminCredential(username, password);
            while (!isAdmin)
            {
                Console.WriteLine("Wrong username or password");
                Console.WriteLine("Username:");
                username = Console.ReadLine();
                Console.WriteLine("Password: ");
                password = Console.ReadLine();
                isAdmin = admin.AdminCredential(username, password);
            }

            bool key = true;
            while (key)
            {
                Menu.AdminMenu();
                string choice;
                choice = Read(0, 3);
                switch (choice)
                {
                    case "0":
                        Console.WriteLine("Good bye admin!");
                        key = false;
                        break;
                    case "1":
                        dbb.CreateUser();
                        break;
                    case "2":
                        //manage client
                        break;
                    case "3":
                        Console.WriteLine(ToString());
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
                        Console.WriteLine("Good bye admin!");
                        key = false;
                        break;
                    case "1":
                        Console.WriteLine("Your GUID: {0}", connectedClient.guid);
                        break;
                    case "2":
                        Console.WriteLine("Seize the preferred currency: ");
                        currency = ReadCurrency();
                        Console.WriteLine(connectedClient.ToStringPreferredCurrency(currency.ToUpper()));
                        break;
                    case "3":
                        Console.WriteLine("Seize the preferred currency: ");
                        currency = ReadCurrency();
                        amount = double.Parse(Console.ReadLine());
                        connectedClient.RetrieveMoney(currency.ToUpper(), amount);
                        break;
                    case "4":
                        Console.WriteLine("Seize the preferred currency: ");
                        currency = ReadCurrency();
                        amount = double.Parse(Console.ReadLine());
                        connectedClient.AddMoney(currency.ToUpper(), amount);
                        break;
                    case "5":
                        Console.WriteLine("Old pin: ");
                        int code = int.Parse(App.Read(1000, 9999));
                        while (!connectedClient.ClientCredential(code))
                        {
                            if (connectedClient.isBlocked)
                            {
                                return;
                            }
                            Console.WriteLine("Wrong pin");
                            code = int.Parse(App.Read(1000, 9999));
                        }
                        Console.WriteLine("New pin: ");
                        code = int.Parse(App.Read(1000, 9999));
                        connectedClient.pin = code;
                        Console.WriteLine("Password changed.");
                        break;
                    case "6":
                        string newCurrency;
                        Console.WriteLine("Currency to be exchanged: ");
                        currency = ReadCurrency();
                        Console.Write("Exchange " + currency + " to ");
                        newCurrency = ReadCurrency();
                        
                        Console.WriteLine("Amount to be converted: ");
                        amount = double.Parse(Console.ReadLine());
                        if (connectedClient.RetrieveMoney(currency, amount))
                        {
                            connectedClient.AddMoney(newCurrency, ApiAccess.ExchangeRate(currency, newCurrency) * amount);
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
            sb.Append(String.Format("{0, 20} {1, 46} {2, 25}\n\n", "Guid", "Fullname", "Currency"));
            foreach (var client in clients)
            {
                var fullName = client.firstname + " " + client.lastname;
                sb.Append(String.Format("{0, 20} {1, 30} {2, 25:P1}\n", client.guid, fullName, client.currency));
            }

            return sb.ToString();
        }
    }
}