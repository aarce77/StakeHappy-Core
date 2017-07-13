using System;

namespace StakHappy.Core.Data.Model.Currency
{
    public class Currency : ICurrency
    {
        public double Value { get; internal set; }

        public Currency(long currency)
        {
            SetCurrency(currency);
        }

        public void SetCurrency(long currency)
        {
            Value = Convert.ToDouble(currency) / 100;
        }
    }
}
