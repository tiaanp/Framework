
namespace Epine.Domain.Data {

	/// <summary>
	///		A base <c>IDataEntity</c> contract providing
	///		common members across all super implementations.
	/// </summary>
	public interface IDataEntity {

		#region Properties

		/// <summary>
		///		Gets/Sets the <see cref="long"/>-based value representing the <code>Id</code> 
		///		property of the current <see cref="IDataEntity"/>-based entity.
		/// </summary>
		long Id { get; set; }

		/// <summary>
		///		Gets/Sets the <see cref="bool"/>-based value representing the <code>IsDeleted</code> 
		///		property of the current <see cref="IDataEntity"/>-based entity.
		/// </summary>
		bool IsDeleted { get; set; }

		#endregion
	}
}
