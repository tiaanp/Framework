using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Epine.Infrastructure.Extensions {

	/// <summary>
	///		Defines extension methods for <see cref="JObject"/>-based objects.
	/// </summary>
	public static class JTokenExtensions {

		#region Class Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static byte[] Compress(this JToken @this) {
			return
				@this.ToString(Formatting.None).ToByteArray().Compress();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static int ToInt32(this JToken @this)
		{
			return
				Convert.ToInt32(
					@this == null
						? "0"
						: @this.ToString());
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static long ToInt64(this JToken @this)
        {
            return
                Convert.ToInt64(
                    @this == null
                        ? "0"
                        : @this.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static long ToInt64NoNull(this JToken @this)
        {
            return
                Convert.ToInt64(
                    @this.ToString().Trim() == string.Empty
                        ? "0"
                        : @this.ToString().Trim());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string GetString(this JToken @this) {
			return				
				@this == null
					? String.Empty
					: @this.ToString();
		}

        /// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static string ToStringNone(this JToken @this)
        {
            return
                @this == null
                    ? String.Empty
                    : @this.ToString(Formatting.None);
        }


		public static string GetStringValue(this JToken token) {
			return
				token != null
					? token.ToString()
					: default(string);
		}
		public static string GetStringValue(this JToken token, string property) {
			return
				token.SelectToken(property).GetStringValue();
		}

		public static int GetInt32Value(this JToken token) {
			return
				token != null
					? Convert.ToInt32(token)
					: default(int);
		}
		public static int GetInt32Value(this JToken token, string property) {
			return
				token.SelectToken(property).GetInt32Value();
		}

		public static short GetInt16Value(this JToken token) {
			return
				token != null
					? Convert.ToInt16(token)
					: default(short);
		}

		public static long GetInt64Value(this JToken token) {
			return
				token != null
					? Convert.ToInt64(token)
					: default(int);
		}
		public static long GetInt64Value(this JToken token, string property) {
			return
				token.SelectToken(property).GetInt64Value();
		}

		public static decimal? GetDecimalValue(this JToken token) {
			return
				token != null
					? Convert.ToDecimal(token)
					: default(decimal?);
		}

		public static bool? GetBooleanValue(this JToken token) {
			return
				token != null
					? Convert.ToBoolean(token)
					: default(bool?);
		}
		public static bool? GetBooleanValue(this JToken token, string property) {
			return
				token.SelectToken(property).GetBooleanValue();
		}

		public static DateTime GetDateValue(this JToken token) {
			return
				token != null
					? Convert.ToDateTime(token)
					: default(DateTime);
		}
		public static DateTime GetDateValue(this JToken token, string property) {
			return
				token.SelectToken(property).GetDateValue();
		}

		#endregion

	}
}
