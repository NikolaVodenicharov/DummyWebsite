using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CameraBazaar2.Web.Infrastructure.Extensions
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
