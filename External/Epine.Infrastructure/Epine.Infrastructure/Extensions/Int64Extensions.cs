using System;
using System.Globalization;

namespace Epine.Infrastructure.Extensions {

	/// <summary>
	/// 
	/// </summary>
	public static class Int64Extensions {

		#region Class Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public static long SubString(this long @this, int start, int end) {
			return
				Convert
					.ToInt64(
						Convert
							.ToString(
								@this,
								CultureInfo.InvariantCulture)
							.Substring(
								start,
								end));
		}


		#endregion
	}
}
