using Epine.Infrastructure.Extensions;
using System;

namespace Epine.Infrastructure.Exceptions {

	/// <summary>
	/// 
	/// </summary>
	public class FtpMoveFileException : Exception {

		#region Constants

		private const string MESSSAGE = "Error occurred on server [{0}] :: path [{1}] when moving files [{2}]";

		#endregion

		

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="server"></param>
		/// <param name="path"></param>
		/// <param name="files"></param>
		public FtpMoveFileException(string server, string path, string files)
			: base(FtpMoveFileException.FormatMessage(server, path, files)) {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="server"></param>
		/// <param name="path"></param>
		/// <param name="files"></param>
		/// <param name="innerException"></param>
		public FtpMoveFileException(string server, string path, string files, Exception innerException)
			: base(FtpMoveFileException.FormatMessage(server, path, files), innerException) {
		}

		#endregion

		#region Class Methods

		private static string FormatMessage(string server, string path, string files) {
			return
				FtpMoveFileException
					.MESSSAGE
					.FormatString(
						server,
						path,
						files);
		}

		#endregion
	}
}
