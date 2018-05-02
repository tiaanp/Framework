using Newtonsoft.Json.Linq;
using System.IO;

namespace Epine.Infrastructure.Resolvers {
	internal class JArrayStreamReaderResolver : StreamReaderResolver<JArray> {

		#region ResultResolver Implementation

		protected override void Resolve(StreamReader data) {
			var value = data.ReadToEnd();

			base._Result =
				JArray.Parse(value);
		}

		#endregion
	}
}
