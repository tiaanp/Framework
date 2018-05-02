using Epine.Infrastructure.Extensions;
using System;

namespace Epine.Infrastructure.Exceptions {

	/// <summary>
	/// 
	/// </summary>
	public class EventManagerTopicException : Exception {

		#region Constants

		private const string MESSSAGE = "Event Manager has not been registered a process for topic {0}";

		#endregion

		

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="topic"></param>
		public EventManagerTopicException(string topic)
			: base(EventManagerTopicException.FormatMessage(topic)) {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="topic"></param>
		/// <param name="innerException"></param>
		public EventManagerTopicException(string topic, Exception innerException)
			: base(EventManagerTopicException.FormatMessage(topic), innerException) {
		}

		#endregion

		#region Class Methods

		private static string FormatMessage(string topic) {
			return
				EventManagerTopicException
					.MESSSAGE
					.FormatString(
						topic);
		}

		#endregion
	}
}
