using System.IO;
using System.Reflection;

namespace Epine.Infrastructure.Resolvers {
	internal class MissingStreamReaderResolver : StreamReaderResolver<Missing> {

		#region ResultResolver Implementation

		protected override void Resolve(StreamReader data) {
			base._Result = Missing.Value;
		}

		#endregion
	}
}
