using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace Epine.Infrastructure.Resolvers {
	internal class JArrayObjectResolver : ObjectResolver<JArray> {

		#region ResultResolver Implementation

		protected override void Resolve(object data) {
			base._Result =
				JArray.Parse(
					Convert.ToString(
						data, 
						CultureInfo.InvariantCulture));
		}

		#endregion
	}
}
