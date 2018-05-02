using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace Epine.Infrastructure.Resolvers {
	internal class JObjectObjectResolver : ObjectResolver<JObject> {

		#region ResultResolver Implementation

		protected override void Resolve(object data) {
			base._Result = 
				JObject.Parse(
					Convert.ToString(
						data, 
						CultureInfo.InvariantCulture));
		}

		#endregion
	}
}
