using System;

namespace ProjectCS
{
    public class Currency
    {
        public string currency { get; set; }
        public double amount { get; set; }

        public double ConvertCurrency(double exchangeRate)
        {
            return amount * exchangeRate;
        }

        public bool RetrieveMoney(double retrievedAmount)
        {
            if (retrievedAmount <= amount)
            {
                amount -= retrievedAmount;
                Console.WriteLine("New sold: " + amount);
                return true;
            }
            else
            {
                Console.WriteLine("-Not enough sold-");
                return false;
            }
        }

        public void AddMoney(double addedMoney)
        {
            amount += addedMoney;
        }
    }
}   