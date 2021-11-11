using System;
using System.Collections.Generic;
using System.Linq;

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
        public bool isBlocked { get; set; }
        public int codeTry { get; set; }

        public string ToStringPreferredCurrency(string toCurrency)
        {
            Console.WriteLine("Sold:");
            var sb = new System.Text.StringBuilder();
            sb.Append(String.Format("{0, 10} {1, 20}\n\n", "Currency", "Amount"));
            var sold = 0.0;
            
            foreach (var money in currencies)
            {
                var convertedAmount = money.ConvertCurrency(ApiAccess.ExchangeRate(money.currency, toCurrency));
                sold += convertedAmount;
                sb.Append(String.Format("{0, 10} {1, 20}\n", money.currency, money.amount));
                string convertTo = money.currency + "->" + toCurrency;
                sb.Append(String.Format("{0, 10} {1, 20}\n\n", convertTo, convertedAmount));
            }

            sb.Append(String.Format("{0, 10} {1, 20}", "Total " + toCurrency, sold));

            return sb.ToString();
        }

        public bool RetrieveMoney(string currencyName, double amount)
        {
            foreach (var money in currencies)
            {
                if (money.currency.Equals(currencyName))
                {
                    return money.RetrieveMoney(amount);
                }
            }

            return false;
        }

        public void AddMoney(string currencyName, double amount)
        {
            foreach (var money in currencies)
            {
                if (money.currency.Equals(currencyName))
                {
                    money.AddMoney(amount);
                    return;
                }
            }

            Currency newCurrency = new Currency();
            newCurrency.currency = currencyName;
            newCurrency.amount = amount;
            currencies.Add(newCurrency);
        }

        public bool ClientCredential(int code)
        {
            if (code == pin)
            {
                return true;
            }
            
            codeTry--;
            if (codeTry == 0)
            {
                isBlocked = true;
            }
            return false;
        }
    }
}