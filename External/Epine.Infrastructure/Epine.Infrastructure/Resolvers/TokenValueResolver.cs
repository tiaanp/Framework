using Epine.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Epine.Infrastructure.Resolvers
{

    /// <summary>
    /// 
    /// </summary>
    public static class TokenValueResolver
    {

        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public const string EPOCH_NOW = "[!-EPOCH+NOW-!]";
        /// <summary>
        /// 
        /// </summary>
        public const string EPOCH_1DAY = "[!-EPOCH+1D-!]";

        /// <summary>
        /// 
        /// </summary>
        public const string EPOCH_2DAY = "[!-EPOCH+2D-!]";

        /// <summary>
        /// 
        /// </summary>
        public const string EPOCH_3DAY = "[!-EPOCH+3D-!]";

        /// <summary>
        /// 
        /// </summary>
        public const string GUID_D = "[!-GUID-D-!]";

        /// <summary>
        /// 
        /// </summary>
        public const string GUID_N = "[!-GUID-N-!]";

        /// <summary>
        /// 
        /// </summary>
        public const string DATE_NOW = "[!-DATE-NOW-!]";

        /// <summary>
        /// 
        /// </summary>
        public const string ONE_TIME_PIN = "[!-ONE-TIME-PIN-!]";

        /// <summary>
        /// 
        /// </summary>
        public const string CURRENT_MONTH_YEAR = "[!-CURRENT_MONTH_YEAR-!]";

        /// <summary>
        /// 
        /// </summary>
        public const string NEXT_MONTH_YEAR = "[!-NEXT_MONTH_YEAR-!]";

        /// <summary>
        /// 
        /// </summary>
        public const string FIRST_DAY_WORK_NEXT_MONTH = "[!-FIRST_DAY_WORK_NEXT_MONTH-!]";

        /// <summary>
		/// 
		/// </summary>
		public const string FIRST_DAY_NEXT_MONTH = "[!-FIRST_DAY_NEXT_MONTH-!]";

        /// <summary>
		/// 
		/// </summary>
        public const string IS_SUNDAY = "[!-IS_SUNDAY-!]";

        /// <summary>
        /// 
        /// </summary>
        public const string FIXED_LENGTH_STRING = "[!-FIXED_LENGTH_STRING-!]";

        #endregion

        #region Class Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assertValue"></param>
        /// <returns></returns>
        public static string Resolve(string assertValue)
        {
            var response = assertValue;

            if (response.IsNotNullOrEmpty() && response.Contains("[!-") && response.Contains("-!]"))
            {
                response =
                    TokenValueResolver.TokenValues[response]();
            }

            return response;
        }

        #endregion

        #region Class Fields

        private static IDictionary<string, Func<string>> TokenValues =
            new Dictionary<string, Func<string>>() {
                { TokenValueResolver.EPOCH_NOW, () => Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds)},
                { TokenValueResolver.EPOCH_1DAY, () => Convert.ToString((DateTime.UtcNow.AddDays(1) - new DateTime(1970, 1, 1)).TotalSeconds)},
                { TokenValueResolver.EPOCH_2DAY, () => Convert.ToString((DateTime.UtcNow.AddDays(2) - new DateTime(1970, 1, 1)).TotalSeconds)},
                { TokenValueResolver.EPOCH_3DAY, () => Convert.ToString((DateTime.UtcNow.AddDays(3) - new DateTime(1970, 1, 1)).TotalSeconds)},
                { TokenValueResolver.GUID_D, () => Guid.NewGuid().ToString("D")},
                { TokenValueResolver.GUID_N, () => Guid.NewGuid().ToString("N")},
                { TokenValueResolver.DATE_NOW, () => DateTime.Now.ToString()},
                { TokenValueResolver.ONE_TIME_PIN, () => Convert.ToString(new Random().Next(1000, 9999))},
                { TokenValueResolver.CURRENT_MONTH_YEAR, () => DateTime.Now.ToString("MMMM yyyy")},
                { TokenValueResolver.NEXT_MONTH_YEAR, () => DateTime.Now.AddMonths(1).ToString("MMMM yyyy")},
                { TokenValueResolver.FIRST_DAY_NEXT_MONTH, () => new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).ToShortDateString()},
                { TokenValueResolver.FIRST_DAY_WORK_NEXT_MONTH, () => {
                    DateTime FirstOfMonth = default(DateTime);
                    DateTime FirstBusinessDay = default(DateTime);
                    FirstOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
                    if (FirstOfMonth.DayOfWeek == DayOfWeek.Sunday)
                    {
                        FirstBusinessDay = FirstOfMonth.AddDays(1);
                    }
                    else if (FirstOfMonth.DayOfWeek == DayOfWeek.Saturday)
                    {
                        FirstBusinessDay = FirstOfMonth.AddDays(2);
                    }
                    else
                    {
                        FirstBusinessDay = FirstOfMonth;
                    }
                    return FirstBusinessDay.ToShortDateString();
                }},
                { TokenValueResolver.IS_SUNDAY, () => {
                    return  (int)DateTime.Today.DayOfWeek == 0 ? "true" : "false";
                }}
                ,
                { TokenValueResolver.FIXED_LENGTH_STRING, () => {
                    var hashKey = default(string);

                    using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                    {
                        // Buffer storage.

                            byte[] data = new byte[4];

                            rng.GetBytes(data);

                            var hashString = Convert.ToString(BitConverter.ToInt32(data, 0)) +
                                             DateTime.Now.ToString("hhMMssfff");



                            using (SHA256Managed sha1 = new SHA256Managed())
                            {
                                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(hashString));

                                //make sure the hash is only alpha numeric to prevent charecters that may break the url
                                hashKey = string.Concat(Convert.ToBase64String(hash).ToCharArray().Where(xx => char.IsLetterOrDigit(xx)));

                            }

                    }
                    return hashKey;
                }}
            };

        #endregion
    }
}
