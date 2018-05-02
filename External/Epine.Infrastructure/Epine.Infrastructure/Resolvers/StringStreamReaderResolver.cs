using System;
using System.IO;

namespace Epine.Infrastructure.Resolvers {
	internal class StringStreamReaderResolver : StreamReaderResolver<String> {

		#region ResultResolver Implementation

		protected override void Resolve(StreamReader data) {
			base._Result = data.ReadToEnd();
		}

		#endregion
	}
}
