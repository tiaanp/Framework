
namespace Epine.Domain.Expressions {

	using System;
	using System.Linq.Expressions;

	public static class StringQuery {

		public static Expression<Func<string, string, bool>> ContainsStartsWith =>
			(value, term) =>
				term != null && term.Length > 0
					? (value.Contains(term) || value.StartsWith(term))
					: true;
	}
}
