using Epine.Infrastructure.Attributes;
using Epine.Infrastructure.Resolvers;
using System;
using System.ComponentModel;
using System.Linq;

namespace Epine.Infrastructure.Extensions {

	/// <summary>
	///		Defines extension methods for <see cref="Enum"/>-based objects.
	/// </summary>
	public static class EnumExtensions {
		
		/// <summary>
		///		Retrieves the Description associated with 
		///		specific <see cref="Enum"/> value as declared 
		///		in <see cref="Enum"/>'s <see cref="DescriptionAttribute"/>
		/// </summary>
		/// <param name="this">
		///		The given <see cref="Enum"/> to retrieve a description for.
		/// </param>
		/// <returns>
		///		A <see cref="string"/> defining the description or name of a given <see cref="Enum"/> field.
		/// </returns>
		public static string Description(this Enum @this) {

			"@this".IsNotNullArgument(@this);

			var response = @this.Name();

			
			@this
				.GetType()
				.GetField(@this.ToString())
				.GetCustomAttributes(typeof(DescriptionAttribute), false)
				.Cast<DescriptionAttribute>()
				.ForEachElement(
					description =>
						response = description.Description);

			return response;
		}

		/// <summary>
		///		Retrieves the name of a given <see cref="Enum"/>.
		/// </summary>
		/// <param name="value">
		///		The <see cref="Enum"/> for which to retrieve a name.
		/// </param>
		/// <returns>
		///		A <see cref="string"/> defining the name of a given <see cref="Enum"/> field.
		/// </returns>
		public static string Name(this Enum value) {
			return
				Enum.Format(value.GetType(), value, "f");
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static Type AssociatedType(this Enum @this) {

			"@this".IsNotNullArgument(@this);

			var response = default(Type);
			
			@this
				.GetType()
				.GetField(@this.ToString())				
				.GetCustomAttributes(typeof(AssociatedTypeAttribute), false)
				.Cast<AssociatedTypeAttribute>()
				.ForEachElement(
					description =>
						response = description.AssociatedType);

			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static int AssociatedInt32(this Enum @this) {

			"@this".IsNotNullArgument(@this);

			var response = default(int);

			@this
				.GetType()
				.GetField(@this.ToString())
				.GetCustomAttributes(typeof(AssociatedInt32Attribute), false)
				.Cast<AssociatedInt32Attribute>()
				.ForEachElement(
					attr =>
						response = attr.AssociatedValue);

			return response;
		}
	}
}
