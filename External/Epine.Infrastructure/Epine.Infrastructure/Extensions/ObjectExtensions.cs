
namespace Epine.Infrastructure.Extensions {

	/// <summary>
	///		Defines extension methods for <see cref="object"/>-based objects.
	/// </summary>
	public static class ObjectExtensions {
		/// <summary>
		///		Casts <paramref name="source"/>
		///		to <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">
		///		The Type to cast to.
		/// </typeparam>
		/// <param name="source">
		///		The object source that will be cast.
		/// </param>
		/// <returns>
		///		<paramref name="source"/> cast 
		///		into  <typeparamref name="T"/>.
		/// </returns>
		public static T As<T>(this object source) {
			return
				source != null
					? (T)source
					: default(T);
		}


		//public static bool IsNumeric(this object @this) {
		//	var response = false;

		//	if (@this == null || @this is DateTime)
		//		response = false;

		//	if (@this is Int16 || @this is Int32 || @this is Int64 || @this is Decimal || @this is Single || @this is Double || @this is Boolean)
		//		response = true;

		//	var outValue = default(double);

		//	if (@this is string) {
		//		response = Double.TryParse(@this as string, out outValue);
		//	} else {
		//		response = Double.TryParse(@this.ToString(), out outValue);
		//	}

		//	return response;
		//}
	}
}
