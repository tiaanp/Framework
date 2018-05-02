using System;
using System.IO;
using System.Security.Cryptography;

namespace Epine.Infrastructure.Cryptography {
	/// <summary>
	///		Defines members for <c>Encryption</c>.
	/// </summary>
	public abstract class Encryption {

		#region Constants

		/// <summary>
		///		Hash algorithm used to generate password. Allowed values are: "MD5" and
		///		"SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
		///		SHA-1 produces a 160-bit (20-byte) hash value.
		/// </summary>
		const string HASHALGORITHM = "SHA1";
		/// <summary>
		///		Passphrase from which a pseudo-random password will be derived. The
		///		derived password will be used to generate the encryption key.
		///		Passphrase can be any string. In this example we assume that this
		///		passphrase is an ASCII string.
		/// </summary>
		const string PASSPHRASE = "Rock Paper Scissors Lizard Spock";
		/// <summary>
		///		Salt value used along with passphrase to generate password. Salt can
		///		be any string. In this example we assume that salt is an ASCII string.
		/// </summary>
		const string SALTVALUE = "Big Bang Theory";
		/// <summary>
		///		Initialization vector (or IV). This value is required to encrypt the
		///		first block of plaintext data. For RijndaelManaged class IV must be 
		///		exactly 16 ASCII characters long.
		/// </summary>
		const string INITVECTOR = "Finding - Nemo..";
		/// <summary>
		///		Number of iterations used to generate password. One or two iterations
		///		should be enough.
		/// </summary>
		const int PASSWORDITERATIONS = 2;
		/// <summary>
		///		Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
		///		Longer keys are more secure than shorter keys.
		/// </summary>
		const int KEYSIZE = 256;

		#endregion

		#region Constructors

		/// <summary>
		///		Creates a new <see cref="Encryption"/> instance.
		/// </summary>
		/// <param name="passPhrase">
		///		A <see cref="String"/> value representing from which a pseudo-random password will be derived.
		/// </param>
		/// <param name="salt">
		///		A <see cref="String"/> value representing the salt value used along with <paramref name="passPhrase"/> to generate the password.
		/// </param>
		/// <param name="initializationVector">
		///		This value is required to encrypt the first block of plaintext data.
		/// </param>
		/// <remarks>
		///		For RijndaelManaged class IV must be exactly 16 ASCII characters long.
		/// </remarks>
		protected Encryption(string passPhrase, string salt, string initializationVector) {
			if (initializationVector != null && initializationVector.Length != 16) {
				throw new ArgumentException("The length of initializationVector must be 16 characters long", "initializationVector");
			}

			this._Salt = salt ?? Encryption.SALTVALUE;
			this._PassPhrase = passPhrase ?? Encryption.PASSPHRASE;
			this._InitializationVector = initializationVector ?? Encryption.INITVECTOR;
		}

		#endregion

		#region Instance Methods

		/// <summary>
		///		Encrypts specified plaintext using Rijndael symmetric key algorithm
		///		and returns a base64-encoded result.
		/// </summary>
		/// <param name="value">
		///		The <see cref="String"/> value to be encrypted.
		/// </param>
		/// <returns>
		///		Encrypted string.
		/// </returns>
		public string Encrypt(string value) {

			if (value == null) {
				value = String.Empty;
			}

			// Convert our plaintext into a byte array.
			// Let us assume that plaintext contains UTF8-encoded characters.
			byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(value);

			// Define memory stream which will be used to hold encrypted data.
			MemoryStream memoryStream = new MemoryStream();

			// Define cryptographic stream (always use Write mode for encryption).
			CryptoStream cryptoStream =
				new CryptoStream(
						memoryStream,
						this.GetEncryptor(),
						CryptoStreamMode.Write);

			// Start encrypting.
			cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

			// Finish encrypting.
			cryptoStream.FlushFinalBlock();

			// Convert our encrypted data from a memory stream into a byte array.
			byte[] cipherTextBytes = memoryStream.ToArray();

			// Close both streams.
			memoryStream.Close();
			cryptoStream.Close();

			// Convert encrypted data into a base64-encoded string.
			string cipherText = Convert.ToBase64String(cipherTextBytes);

			// Return encrypted string.
			return cipherText;
		}

		/// <summary>
		///		Decrypts specified ciphertext using Rijndael symmetric key algorithm.
		/// </summary>
		/// <param name="cipherText">
		///		Base64-formatted ciphertext value.
		/// </param>
		/// <returns>
		///		Decrypted string value.
		/// </returns>
		public string Decrypt(string cipherText) {

			if (cipherText == null) {
				cipherText = String.Empty;
			}

			// Convert our ciphertext into a byte array.
			byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

			// Define memory stream which will be used to hold encrypted data.
			MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

			// Define cryptographic stream (always use Read mode for encryption).
			CryptoStream cryptoStream =
				new CryptoStream(
					memoryStream,
				// Generate decryptor from the existing key bytes and initialization 
				// vector. Key size will be defined based on the number of the key 
				// bytes.
					this.GetDecryptor(),
					CryptoStreamMode.Read);

			// Since at this point we don't know what the size of decrypted data
			// will be, allocate the buffer long enough to hold ciphertext;
			// plaintext is never longer than ciphertext.
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];

			// Start decrypting.
			int decryptedByteCount =
				cryptoStream.Read(
					plainTextBytes,
					0,
					plainTextBytes.Length);

			// Close both streams.
			memoryStream.Close();
			cryptoStream.Close();

			// Convert decrypted data into a string. 
			// Let us assume that the original plaintext string was UTF8-encoded.
			string plainText =
				System.Text
					.Encoding
					.UTF8
					.GetString(
						plainTextBytes,
						0,
						decryptedByteCount);

			// Return decrypted string.   
			return plainText;
		}


		/// <summary>
		///		First, we must create a password, from which the key will be derived.
		///		This password will be generated from the specified passphrase and 
		///		salt value. The password will be created using the specified hash 
		///		algorithm. Password creation can be done in several iterations.
		/// </summary>
		/// <returns></returns>
		private PasswordDeriveBytes GetPassword() {
			return
				new PasswordDeriveBytes(
						this._PassPhrase,
						System.Text.Encoding.ASCII.GetBytes(this._Salt),
						Encryption.HASHALGORITHM,
						Encryption.PASSWORDITERATIONS);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private ICryptoTransform GetDecryptor() {
			// Create uninitialized Rijndael encryption object.
			var symmetricKey = new RijndaelManaged();

			// It is reasonable to set encryption mode to Cipher Block Chaining
			// (CBC). Use default options for other symmetric key parameters.
			symmetricKey.Mode = CipherMode.CBC;

			// Convert strings defining encryption key characteristics into byte
			// arrays. Let us assume that strings only contain ASCII codes.
			// If strings include Unicode characters, use Unicode, UTF7, or UTF8
			// encoding.
			return
				symmetricKey.CreateDecryptor(
					this.GetPassword().GetBytes(Encryption.KEYSIZE / 8),
					System.Text.Encoding.ASCII.GetBytes(this._InitializationVector));
		}

		/// <summary>
		///		Use the password to generate pseudo-random bytes for the encryption
		///		key. Specify the size of the key in bytes (instead of bits).
		/// </summary>
		/// <returns></returns>
		private ICryptoTransform GetEncryptor() {
			// Create uninitialized Rijndael encryption object.
			var symmetricKey = new RijndaelManaged();

			// It is reasonable to set encryption mode to Cipher Block Chaining
			// (CBC). Use default options for other symmetric key parameters.
			symmetricKey.Mode = CipherMode.CBC;

			// Convert strings defining encryption key characteristics into byte
			// arrays. Let us assume that strings only contain ASCII codes.
			// If strings include Unicode characters, use Unicode, UTF7, or UTF8
			// encoding.
			return
				symmetricKey.CreateEncryptor(
					this.GetPassword().GetBytes(Encryption.KEYSIZE / 8),
					System.Text.Encoding.ASCII.GetBytes(this._InitializationVector));
		}

		#endregion

		#region Instance Fields

		private readonly string _PassPhrase;
		private readonly string _Salt;
		private readonly string _InitializationVector;

		#endregion
	}
}
