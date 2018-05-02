using System.IO;

namespace Epine.Infrastructure.Resolvers {
	internal abstract class StreamReaderResolver<TResult> : StreamReaderResolver {

		#region Instance Methods

		internal TResult GetResult(StreamReader data) {
			this.Resolve(data);
			
			return
				this._Result;
		}

		#endregion

		#region Instance Fields

		protected TResult _Result;

		#endregion
	}

	internal abstract class StreamReaderResolver {

		#region Instance Methods

		protected abstract void Resolve(StreamReader data);

		#endregion
	}
}
