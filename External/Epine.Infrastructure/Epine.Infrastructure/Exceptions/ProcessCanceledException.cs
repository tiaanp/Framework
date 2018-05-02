using Epine.Infrastructure.Extensions;
using System;

namespace Epine.Infrastructure.Exceptions {

	/// <summary>
	/// 
	/// </summary>
	public class ProcessCanceledException : Exception {
		#region Constants

		private const string MESSSAGE = "Process Flow {0} has been canceled by an external source.";

		#endregion

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stateId"></param>
		public ProcessCanceledException(long stateId)
			: base(ProcessCanceledException.MESSSAGE.FormatString(stateId)) {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stateId"></param>
		/// <param name="innerException"></param>
		public ProcessCanceledException(long stateId, Exception innerException)
			: base(ProcessCanceledException.MESSSAGE.FormatString(stateId), innerException) {
		}

		#endregion
	}
}
