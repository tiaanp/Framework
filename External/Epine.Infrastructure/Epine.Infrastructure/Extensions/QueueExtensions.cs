using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Epine.Infrastructure.Extensions {

	/// <summary>
	/// 
	/// </summary>
	public static class QueueExtensions {

		#region Class Methods

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queue"></param>
		/// <param name="collection"></param>
		public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> collection) {
			collection
				.ForEachElement(
					item => {
						queue.Enqueue(item);
					});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queue"></param>
		/// <param name="collection"></param>
		public static void EnqueueRange<T>(this ConcurrentQueue<T> queue, IEnumerable<T> collection) {
			collection
				.ForEachElement(
					item => {
						queue.Enqueue(item);
					});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="this"></param>
		/// <returns></returns>
		public static IEnumerable<T> DequeueAll<T>(this Queue<T> @this) {
			
			while (@this.IsNotNullOrEmpty()) {
				yield return @this.Dequeue();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="this"></param>
		/// <returns></returns>
		public static IEnumerable<T> DequeueAll<T>(this ConcurrentQueue<T> @this) {
			var response = default(T);
			while (@this.IsNotNullOrEmpty()) {
				@this.TryDequeue(out response);
				yield return response;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="this"></param>
		/// <returns></returns>
		public static T Dequeue<T>(this ConcurrentQueue<T> @this) {
			var response = default(T);
			@this.TryDequeue(out response);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="this"></param>
		/// <returns></returns>
		public static T Peek<T>(this ConcurrentQueue<T> @this) {
			var response = default(T);
			@this.TryPeek(out response);
			return response;
		}

		#endregion
	}
}
