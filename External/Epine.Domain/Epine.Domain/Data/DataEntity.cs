using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Epine.Domain.Data {

	/// <summary>
	///		A base <c>DataEntity</c>-based class providing
	///		common members across all super classes.
	/// </summary>
	public abstract class DataEntity : IDataEntity {

		#region Instance Properties

		/// <summary>
		///		Gets/Sets the <see cref="long"/>-based value representing the <code>Id</code> 
		///		property of the current <see cref="DataEntity"/>-based entity.
		/// </summary>
		[property:
			Key,
			DataMember
		]
		public virtual long Id { get; set; }

		/// <summary>
		///		Gets/Sets the <see cref="bool"/>-based value representing the <code>IsDeleted</code> 
		///		property of the current <see cref="DataEntity"/>-based entity.
		/// </summary>
		[property:
			DataMember
		]
		public virtual bool IsDeleted { get; set; }

		#endregion
	}
}
