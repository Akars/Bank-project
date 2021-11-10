namespace ProjectCS
{
    public class Currency
    {
        public string currency { get; set; }
        public int amount { get; set; }

        public Currency(string _currency, int _amount)
        {
            currency = _currency;
            amount = _amount;
        }
    }
}   