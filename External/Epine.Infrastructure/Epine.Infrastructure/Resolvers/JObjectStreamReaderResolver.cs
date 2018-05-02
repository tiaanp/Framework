using Epine.Infrastructure.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Epine.Infrastructure.Resolvers {
	internal class JObjectStreamReaderResolver : StreamReaderResolver<JObject> {

		#region ResultResolver Implementation

		protected override void Resolve(StreamReader data) {
			var value = data.ReadToEnd();

			try {
				base._Result = JObject.Parse(value);
			}
			catch (Exception exception) {
				throw new StreamReaderResolverException(typeof(JObject), value, exception);
			}
		}

		#endregion
	}
}
