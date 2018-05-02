using Epine.Infrastructure.Extensions;
using System;

namespace Epine.Infrastructure.Exceptions {

	/// <summary>
	/// 
	/// </summary>
	public class StreamReaderResolverException : Exception {

		#region Constants

		private const string MESSSAGE = "An Error occurred resolving type {0} [data:{1}]";

		#endregion
				

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="resolverType"></param>
		/// <param name="data"></param>
		public StreamReaderResolverException(Type resolverType, string data)
			: base(StreamReaderResolverException.FormatMessage(resolverType.Name, data)) {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="resolverType"></param>
		/// <param name="data"></param>
		/// <param name="innerException"></param>
		public StreamReaderResolverException(Type resolverType, string data, Exception innerException)
			: base(StreamReaderResolverException.FormatMessage(resolverType.Name, data), innerException) {
		}

		#endregion

		#region Class Methods

		private static string FormatMessage(string type, string data) {
			return
				StreamReaderResolverException
					.MESSSAGE
					.FormatString(
						type,
						data);
		}

		#endregion
	}
}
