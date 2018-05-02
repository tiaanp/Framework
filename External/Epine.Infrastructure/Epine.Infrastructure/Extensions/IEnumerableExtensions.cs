using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Epine.Infrastructure.Extensions {

	/// <summary>
	///		Defines extension methods for <see cref="IEnumerable&lt;T&gt;"/>-based objects.
	/// </summary>
	public static class IEnumerableExtensions {

		#region Constant

		private const string DELIMITER = ",";

		#endregion

		#region Class Methods

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="this"></param>
		/// <param name="groupCount"></param>
		/// <returns></returns>
		public static IEnumerable<IEnumerable<TEntity>> GroupSplit<TEntity>(this IEnumerable<TEntity> @this, int groupCount) {
			if (groupCount <= 0) throw new ArgumentOutOfRangeException("groupCount", groupCount, "parameter must be greater that 0");

			var position = 0;
			while (@this.Skip(position).Any()) {
				yield return @this.Skip(position).Take(groupCount);
				position += groupCount;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="this"></param>
		/// <param name="groupCount"></param>
		/// <returns></returns>
		public static int GroupSplitCount<TEntity>(this IEnumerable<TEntity> @this, int groupCount) {
			var count = @this.Count();
			return
				((count / groupCount) + (((count % groupCount) > 0) ? 1 : 0));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="col"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static IEnumerable<T> Loop<T>(this T col, int count) {
			return Enumerable.Repeat(col, count);
		}

		/// <summary>
		///		Provides the ability to execute an action 
		///		for each <typeparamref name="T"/>-based 
		///		entity in the <paramref name="collection"/>.
		/// </summary>
		/// <typeparam name="T">
		///		The <see cref="Type"/> of entities in 
		///		the <paramref name="collection"/>.
		/// </typeparam>
		/// <param name="collection">
		///		The <see cref="IEnumerable{T}"/>-based collection 
		///		to iterate an <see cref="Action"/> over.
		/// </param>
		/// <param name="action">
		///		The <typeparamref name="T"/>-based <see cref="Action"/> 
		///		to execute for each element in the <paramref name="collection"/>.
		/// </param>
		public static void	ForEachElement<T>(this IEnumerable<T> collection, Action<T> action) {

			foreach (var item in collection ?? Enumerable.Empty<T>())
			{
				action(item);
			}
		}

       

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> collection) {
			return
				collection != null && collection.Any();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) {
			return
				collection == null || !collection.Any();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="source"></param>
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> source) {
			source
				.ForEachElement(
					item => collection.Add(item));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="source"></param>
		public static void AddRange<T>(this IList<T> collection, IEnumerable<T> source) {
			source
				.ForEachElement(
					item => collection.Add(item));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="source"></param>
		public static void AddRange<T>(this ConcurrentBag<T> collection, IEnumerable<T> source)
		{
			source
				.ForEachElement(
					item => collection.Add(item));
		}

		/// <summary>
		///		Converts all the <typeparamref name="T"/>-based objects in the 
		///		<paramref name="collection"/> to a <see cref="string"/> 
		///		representation separated by the <paramref name="delimiter"/>.
		/// </summary>
		/// <typeparam name="T">
		///		The <see cref="Type"/> of entities in 
		///		the <paramref name="collection"/>.
		/// </typeparam>
		/// <param name="collection">
		///		The <see cref=" IEnumerable{T}"/>-based collection 
		///		to convert into a delimited <see cref="string"/>.
		/// </param>
		/// <param name="delimiter">
		///		The separator to the split the <typeparamref name="T"/>-based 
		///		<paramref name="collection"/> with.
		/// </param>
		/// <returns></returns>
		public static string ToDelimited<T>(this IEnumerable<T> collection, string delimiter) {
			// Validate parameter.
			"collection".IsNotNullArgument(collection);

			return
				String.Join(
					(String.IsNullOrEmpty(delimiter)
						? IEnumerableExtensions.DELIMITER
						: delimiter), 
					collection);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static string ToDelimited<T>(this IEnumerable<T> collection) {
			return
				collection.ToDelimited(IEnumerableExtensions.DELIMITER);
		}

		#endregion
	}
}
