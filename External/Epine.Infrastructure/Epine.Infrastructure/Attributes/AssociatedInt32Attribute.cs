using System;

namespace Epine.Infrastructure.Attributes {

	[type:
		AttributeUsage(AttributeTargets.Field, AllowMultiple = false)
	]
	public class AssociatedInt32Attribute : Attribute {

		#region Instance Properties

		public int AssociatedValue {
			get {
				return
					this._Value;
			}
		}

		#endregion


		#region Constructor

		public AssociatedInt32Attribute(int value) {
			this._Value = value;
		}

		#endregion

		#region Instance Fields

		private readonly int _Value;

		#endregion
	}
}
