
namespace Epine.Infrastructure.Resolvers {
	internal abstract class ObjectResolver<TResult> : ObjectResolver {

		#region Instance Methods

		internal TResult GetResult(object data) {
			this.Resolve(data);
			
			return
				this._Result;
		}

		#endregion

		#region Instance Fields

		protected TResult _Result;

		#endregion
	}

	internal abstract class ObjectResolver {

		#region Instance Methods

		protected abstract void Resolve(object data);

		#endregion
	}
}
