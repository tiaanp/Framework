using System;
using System.Globalization;

namespace Epine.Infrastructure.Resolvers {
	internal class StringObjectResolver : ObjectResolver<String> {

		#region ResultResolver Implementation

		protected override void Resolve(object data) {
			base._Result = 
				Convert.ToString(
					data, 
					CultureInfo.InvariantCulture);
		}

		#endregion
	}
}
