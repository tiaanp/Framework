
namespace Epine.Infrastructure.Contracts {

	/// <summary>
	/// 
	/// </summary>
	public interface IQueueConnectionInfo {

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		string Server		{ get; }

		/// <summary>
		/// 
		/// </summary>
		string Manager		{ get; }

		/// <summary>
		/// 
		/// </summary>
		string Channel		{ get; }

		/// <summary>
		/// 
		/// </summary>
		int Port			{ get; }

		/// <summary>
		/// 
		/// </summary>
		string UserName		{ get; }

		/// <summary>
		/// 
		/// </summary>
		string Password		{ get; }

		/// <summary>
		/// 
		/// </summary>
		string Queue		{ get; }

		#endregion
	}
}
