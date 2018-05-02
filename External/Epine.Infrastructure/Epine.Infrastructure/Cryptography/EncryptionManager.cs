using Epine.Infrastructure.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Epine.Infrastructure.Cryptography {

	/// <summary>
	/// 
	/// </summary>
	public sealed class EncryptionManager : Encryption {

		#region Constants

		/// <summary>
		///		This value is required to encrypt the first block of plain-text data.
		/// </summary>
		/// <remarks>
		///		For RijndaelManaged class IV must be exactly 16 ASCII characters long.
		/// </remarks>
		private const string INITIALIZATIONVECTOR = ".Yx0rpNo!tPyrcn3";
		//private const string INITIALIZATIONVECTOR = "YticGrubsennahoj";

		#endregion

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="passPhrase"></param>
		/// <param name="salt"></param>
		public EncryptionManager(string passPhrase, string salt)
			: base(passPhrase, salt, EncryptionManager.INITIALIZATIONVECTOR) {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="passPhrase"></param>
		/// <param name="salt"></param>
		/// <param name="vector"></param>
		public EncryptionManager(string passPhrase, string salt, string vector)
			: base(passPhrase, salt, vector) {
		}

		#endregion
		
		#region Instance Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		public void EncryptEntity(object entity) {
			if (entity != null) {
				//private fields
				entity.GetType()
					.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
					.ForEachElement(
						field => {
							if (field.GetValue(entity) != null) {
								if (field.FieldType == typeof(string)) {

									field
										.SetValue(
											entity,
											base.Encrypt(
												Convert.ToString(
													field.GetValue(entity),
													CultureInfo.InvariantCulture)));

								} else if (field.FieldType.GetInterfaces().Any(x => x == typeof(IEnumerable))) {

									if (field.FieldType.GetInterfaces().Any(x => x == typeof(IEnumerable<string>))) {

										var elements =
											field
												.GetValue(entity)
												.As<IEnumerable<string>>()
												.Select(
													str => base.Encrypt(str));

										if (field.FieldType.IsArray) {
											field
												.SetValue(
													entity,
													elements.ToArray());
										} else if (field.FieldType == typeof(List<string>)) {
											field
												.SetValue(
													entity,
													elements.ToList());
										} else {
											field
												.SetValue(
													entity,
													Activator.CreateInstance(
														field.FieldType
															.GetGenericTypeDefinition()
															.MakeGenericType(typeof(string)),
														elements.ToList()));
										}

									} else if (field.FieldType.GetInterfaces().Any(x => x == typeof(IDictionary))) {

										var genericArguments = field.FieldType.GetGenericArguments();
										var fieldValue = field.GetValue(entity).As<IDictionary>().GetEnumerator();

										if (genericArguments[1] == typeof(string)) {
											var keys = new List<string>();
											var values = new List<string>();

											while (fieldValue.MoveNext()) {
												keys.Add(
													Convert.ToString(
														fieldValue
															.Current
															.GetType()
															.GetProperty(
																"Key",
																BindingFlags.Public | BindingFlags.Instance)
															.GetValue(
																fieldValue.Current)));
												values.Add(
													Convert.ToString(
														fieldValue
															.Current
															.GetType()
															.GetProperty(
																"Value",
																BindingFlags.Public | BindingFlags.Instance)
															.GetValue(
																fieldValue.Current)));
											}

											for (int i = 0; i < keys.Count; i++) {
												values[i] = base.Encrypt(values[i]);
											}

											var dic = new Dictionary<string, string>();

											var genericDefinition = field.FieldType.GetGenericTypeDefinition();
											var genericType = genericDefinition.MakeGenericType(genericArguments[0], genericArguments[1]);

											for (int i = 0; i < keys.Count; i++) {
												dic.Add(keys[i], values[i]);
											}

											field
												.SetValue(
													entity,
													Convert.ChangeType(
														dic,
														genericType,
														CultureInfo.InvariantCulture));
										} else if (genericArguments[1].IsClass) {
											while (fieldValue.MoveNext()) {
												this.EncryptEntity(fieldValue.Current);
											}
										}
									} else {
										var enumerator = field.GetValue(entity).As<IEnumerable>().GetEnumerator();

										while (enumerator.MoveNext()) {
											this.EncryptEntity(enumerator.Current);
										}
									}
								} else if (field.FieldType.IsClass) {
									this.EncryptEntity(field.GetValue(entity));
								}
							}
						});
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		public string EncryptGUID(Guid guid) {
			return
				Uri.EscapeDataString(
					base.Encrypt(
						guid.ToString()
							.Replace(
								"-",
								String.Empty)));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public Guid DecryptGUID(string value) {
			return
				EncryptionManager.Decipher(
					base.Decrypt(
						Uri.UnescapeDataString(value)));
		}

		#endregion

		#region Class Methods
		private static Guid Decipher(string value) {
			return
				new Guid(
					String.Concat(
						value.Substring(0, 8),
						"-",
						value.Substring(8, 4),
						"-",
						value.Substring(12, 4),
						"-",
						value.Substring(16, 4),
						"-",
						value.Substring(20, 12)));
		}

		#endregion
	}
}
