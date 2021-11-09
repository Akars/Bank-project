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
            
            throw new System.NotImplementedException();
        }

        public List<Client> GetAll()
        {
            SQLiteConnection cnn = LoadConnectionString();
            IDataReader sqLiteDataReader;
            var outputClients = cnn.Query<Client>("select * from Client", new DynamicParameters());
            
            var list = outputClients.ToList();

            var queryCurrency = "Select * from Currencies";

            var outputCurrency = cnn.Query(queryCurrency, new DynamicParameters());
            sqLiteDataReader = cnn.ExecuteReader(queryCurrency);
            while (sqLiteDataReader.Read())
            {
                var myreader = sqLiteDataReader.GetData();
                Console.WriteLine(myreader);
            }
            return list.ToList();
        }

        public Client CreateUser()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteClient(int guid)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateClient(Client c)
        {
            throw new System.NotImplementedException();
        }

        public static SQLiteConnection LoadConnectionString()
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