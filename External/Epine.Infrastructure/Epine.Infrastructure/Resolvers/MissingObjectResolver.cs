using System.Reflection;

namespace Epine.Infrastructure.Resolvers {
	internal class MissingObjectResolver : ObjectResolver<Missing> {

		#region ResultResolver Implementation

		protected override void Resolve(object data) {
			base._Result = Missing.Value;
		}

		#endregion
	}
}
