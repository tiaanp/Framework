using Epine.Infrastructure.Extensions;
using System;

namespace Epine.Infrastructure.Exceptions {

	/// <summary>
	/// 
	/// </summary>
	public class HttpPostException : Exception {

		#region Constants

		private const string MESSSAGE = "An Error occurred when posting to url {0}";

		#endregion

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		public HttpPostException(string url)
			: base(HttpPostException.FormatMessage(url)) {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <param name="innerException"></param>
		public HttpPostException(string url, Exception innerException)
			: base(HttpPostException.FormatMessage(url), innerException) {
		}

		#endregion

		#region Class Methods

		private static string FormatMessage(string url) {
			return
				HttpPostException
					.MESSSAGE
					.FormatString(
						url);
		}

		#endregion
	}
}
