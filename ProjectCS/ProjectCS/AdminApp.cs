using System;

namespace ProjectCS
{
    public class AdminApp
    {
        public Admin admin { get; set; }
        
        public bool ConnectAdmin()
        {
            admin = new Admin();
            int authorizedTry = 0;
            do
            {
                Console.WriteLine("username: ");
                string username = Console.ReadLine();
                Console.WriteLine("password: ");
                string password = Console.ReadLine();
                if (admin.AdminCredential(username, password))
                {
                    return true;
                }

                authorizedTry++;
            } while (authorizedTry < 3);

            return false;
        }
        
        
    }
}