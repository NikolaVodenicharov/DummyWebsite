using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Tyres.Web.Infrastructure.Extensions
{
    public static class TempDataDictionaryExtensions
    {
        public const string SuccessMessageKey = "SuccessMessage";
        public const string ErrorMessageKey = "ErrorMessage";

        /// <summary>
        /// Can add to the TempData in controllers a success message that is displayed from the Layout view before rendering the body.
        /// We have to add condition if(TempData.ContainsKey(TempDataDictionaryExtensions.SuccessMessageKey)) in to the Layout
        /// </summary>
        public static void AddSuccessMessage(this ITempDataDictionary tempDate, string message)
        {
            tempDate[SuccessMessageKey] = message;
        }

        public static void AddErrrorMessage(this ITempDataDictionary tempDate, string message)
        {
            tempDate[ErrorMessageKey] = message;
        }
    }
}
