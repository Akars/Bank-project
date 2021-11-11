using System;

namespace ProjectCS
{
    public class Menu
    {
        public static void PrintWelcomeMsg()
        {
            Console.WriteLine("1: If you're an Admin");
            Console.WriteLine("2: If you're a Client");
            Console.WriteLine("0: Quit");
        }
        
        public static void AdminMenu()
        {
            Console.WriteLine("1: Create Client");
            Console.WriteLine("2: Manage a Client");
            Console.WriteLine("3: View list Client");
            Console.WriteLine("0: Quit");
        }

        public static void ClientMenu()
        {
            Console.WriteLine("1: View GUID and credentials");
            Console.WriteLine("2: View total amount");
            Console.WriteLine("3: Retrieve money");
            Console.WriteLine("4: Add money");
            Console.WriteLine("5: Change pin");
            Console.WriteLine("6: Exchange between currencies");
            Console.WriteLine("0: Quit");
        }
        
        public static void PrintManage()
        {
            Console.WriteLine("1: UnBlock");
            Console.WriteLine("2: Block");
            Console.WriteLine("3: Change pin");
            Console.WriteLine("4: Reset tries");
            Console.WriteLine("5: Delete client");
            Console.WriteLine("0: Quit");
        }
    }
}