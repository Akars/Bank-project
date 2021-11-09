using System;

namespace ProjectCS
{
    public class Admin
    {
        private string username = "admin";
        private string password = "admin";

        public bool AdminCredential(string username, string password)
        {
            return username.Equals(this.username) && password.Equals(this.password);
        }
    }
}