using System.Collections.Generic;
using System.Data.Entity;

namespace Epine.Infrastructure.Extensions {

	/// <summary>
	///		Defines extension methods for <see cref="DbSet&lt;TEntity&gt;"/>-based objects.
	/// </summary>
	public static class DbSetExtensions {

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="set"></param>
		/// <param name="list"></param>
		public static void Add<TEntity>(this DbSet<TEntity> set, IEnumerable<TEntity> list)
			where TEntity : class {
			list
				.ForEachElement(
					item => set.Add(item));
		}
	}
}
