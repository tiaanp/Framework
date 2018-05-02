
namespace Epine.Domain.Expressions {

	using System;
	using System.Linq.Expressions;

	public static class LongQuery {

		public static Expression<Func<long, long?, bool>> IsEqual =>
			(value, term) =>
				term.HasValue && term > 0
					? value == term
					: true;
	}
}
