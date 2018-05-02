using System;

namespace Epine.Infrastructure.Resolvers {

	/// <summary>
	/// 
	/// </summary>
	[type:
		AttributeUsage(AttributeTargets.Field, AllowMultiple = false)
	]
	public class AssociatedTypeAttribute : Attribute {

		#region Instance Properties

		/// <summary>
		/// 
		/// </summary>
		public Type AssociatedType {
			get {
				return
					this._AssociatedType;
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		///		
		/// </summary>
		/// <param name="associatedType"></param>
		public AssociatedTypeAttribute(Type associatedType) {
			this._AssociatedType = associatedType;
		}

		#endregion

		#region Instance Fields

		private readonly Type _AssociatedType;

		#endregion
	}
}
