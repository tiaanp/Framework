using Epine.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Epine.Infrastructure.Cryptography {

	/// <summary>
	///		Utility for validating message data.
	/// </summary>
	public static class Hmac {

		#region Class Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool Validate(byte[] key, byte[] data) {
			var response = true;
			// Initialize the keyed hash object.
			using (HMACSHA512 hmac = new HMACSHA512(key)) {
				
				// Create an array to hold the keyed hash value read from the file. 
				byte[] storedHash = new byte[hmac.HashSize / 8];
				// Create a reader for the source data. 
				using (var inStream = new MemoryStream(data)) {
					// Read in the storedHash.
					inStream.Read(storedHash, 0, storedHash.Length);
					// Compute the hash of the remaining contents of the file. 
					// The stream is properly positioned at the beginning of the content,  
					// immediately after the stored hash value. 
					byte[] computedHash = hmac.ComputeHash(inStream);
					// compare the computed hash with the stored value 

					for (int i = 0; i < storedHash.Length; i++) {
						if (!(_CarriageFeed.Contains(computedHash[i]) && _CarriageFeed.Contains(storedHash[i]))) {
							if (computedHash[i] != storedHash[i]) {
								response = false;
							}
						}
					}
				}
			}
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string GetMessage(byte[] key, byte[] data) {
			var response = default(string);
			// Initialize the keyed hash object.
			using (HMACSHA512 hmac = new HMACSHA512(key)) {
				// Create an array to hold the keyed hash value read from the file. 
				byte[] storedHash = new byte[hmac.HashSize / 8];
				byte[] contents = new byte[data.Length - (hmac.HashSize / 8)];
				// Create a reader for the source data. 
				using (var inStream = new MemoryStream(data)) {
					// Read in the storedHash.
					inStream.Read(storedHash, 0, storedHash.Length);
					// read the remaining contents of the file. 
					// The stream is properly positioned at the beginning of the content,  
					// immediately after the stored hash value. 
					inStream.Read(contents, 0, contents.Length);
					response = contents.ExtractString();
				}
			}
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public static string HashMessage(byte[] key, string message) {
			var response = default(string);
			using (HMACSHA512 hmac = new HMACSHA512(key)) {

						// Compute the hash of the input file. 
				byte[] hashValue = hmac.ComputeHash(message.ToByteArray());

				response = 
					String.Concat(
					hashValue.ExtractString(),
					message);

			}

			return response;
		}

		#endregion

		#region Class Fields

		private static readonly IList<byte> _CarriageFeed = new List<byte> { 10, 13 };

		#endregion
	}
}
