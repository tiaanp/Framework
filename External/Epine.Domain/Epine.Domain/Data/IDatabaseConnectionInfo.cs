namespace Epine.Domain.Data {

	/// <summary>
	///		Defines a contract for <c>IDatabaseConnectionInfo</c>, providing uniform 
	///		configuration properties for data base connectivity.
	/// </summary>
	public interface IDatabaseConnectionInfo {

		#region Properties

		/// <summary>
		///		Gets/Sets the <see cref="string"/>-based value representing the <code>Server</code> 
		///		property of the current <see cref="IDatabaseConnectionInfo"/>-based entity.
		/// </summary>
		string Server { get; set; }

		/// <summary>
		///		Gets/Sets the <see cref="string"/>-based value representing the <code>Catalog</code> 
		///		property of the current <see cref="IDatabaseConnectionInfo"/>-based entity.
		/// </summary>
		string Catalog { get; set; }

		/// <summary>
		///		Gets/Sets the <see cref="string"/>-based value representing the <code>UserName</code> 
		///		property of the current <see cref="IDatabaseConnectionInfo"/>-based entity.
		/// </summary>
		string UserName { get; set; }

		/// <summary>
		///		Gets/Sets the <see cref="string"/>-based value representing the <code>Password</code> 
		///		property of the current <see cref="IDatabaseConnectionInfo"/>-based entity.
		/// </summary>
		string Password { get; set; }

		DatabaseType DatabaseType { get; set; }

		#endregion
	}
}
