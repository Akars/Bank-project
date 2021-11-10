using System;
using System.Collections.Generic;

namespace ProjectCS
{
    public class Client
    {
        public string guid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int pin { get; set; }
        public List<Currency> currencies { get; set; }
        public string currency { get; set; }

        public Client(string _firstname, string _lastname)
        {
            firstname = _firstname;
            lastname = _lastname;
            guid = Guid.NewGuid().ToString();
            pin = new Random().Next(1000, 9999);
            currencies = new List<Currency>();
            currency = "EUR";
        }
    }
}