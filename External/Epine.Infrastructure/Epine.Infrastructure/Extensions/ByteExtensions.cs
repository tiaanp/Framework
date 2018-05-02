
using System.Collections;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Epine.Infrastructure.Extensions {

	/// <summary>
	///		Defines extension methods for <see cref="byte"/>-based objects.
	/// </summary>
	public static class ByteExtensions {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static Stream ExtractStream(this byte[] data) {
			var response = default(Stream);

			if (data.IsNotNullOrEmpty())
			{
				response = new MemoryStream(data);
			}

			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static JArray ExtractJsonArray(this byte[] data) {
			var response = default(JArray);

			if (data.IsNotNullOrEmpty())
			{
				response = JArray.Parse(data.ExtractString());
			}

			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static JObject ExtractJsonObject(this byte[] data)
		{
			var response = default(JObject);

			if (data.IsNotNullOrEmpty())
			{
				response = JObject.Parse(data.ExtractString());
			}

			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string ExtractString(this byte[] data)
		{
			return 
				data.ExtractString(
					System.Text.Encoding.Default);
		}

	    public static string ExtractStringUtf8(this byte[] data)
	    {
	        return
	            data.ExtractString(
	                System.Text.Encoding.UTF8);
	    }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ExtractString(this byte[] value, System.Text.Encoding encoding)
		{
			return encoding.GetString(value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static Stream ToStream(this byte[] @this) {
			return
				new MemoryStream(@this);
		}

        public static bool ByteArrayCompare(this byte[] @this, byte[] compareValue)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(@this, compareValue);
        }
    }
}
