
namespace StakHappy.Core.Data.Model.Currency
{
    interface ICurrency
    {
        double Value { get; }
        void SetCurrency(long currency);
    }
}
