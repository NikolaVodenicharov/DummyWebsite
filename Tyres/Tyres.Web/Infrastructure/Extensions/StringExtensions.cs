namespace Tyres.Web.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        private const string MoneyFormat = "F2";
        public static string ToMoney (this decimal price)
        {
            return price.ToString(MoneyFormat);
        }
    }
}
