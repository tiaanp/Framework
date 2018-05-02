using Epine.Infrastructure.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using SharpCompress.Readers;

namespace Epine.Infrastructure.Extensions {

	/// <summary>
	/// 
	/// </summary>
	public static class CompressionExtensions {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="mergeStreams"></param>
		/// <param name="filename"></param>
		/// <param name="insideCompretionExtention"></param>
		/// <returns></returns>
		public static IEnumerable<Tuple<string, Stream>> DecompressStreamFiles(this Stream stream, out bool mergeStreams,string filename,string insideCompretionExtention) {
			mergeStreams = false;

			var response = new List<Tuple<string, Stream>>();
			Stream mStream = new MemoryStream();
			stream.TryResetStream();
			int count = 0;
			int counter = 0;
			string line = "";

			using (var zipReader = SharpCompress.Readers.ReaderFactory.Open(stream)) {
				
				try {
					while (zipReader.MoveToNextEntry()) {
						if (insideCompretionExtention.IsNullOrEmpty() || zipReader.Entry.Key.ToUpperInvariant().Contains(insideCompretionExtention.ToUpperInvariant()))
						{
							int filecounter = 0;
							mStream = new MemoryStream();
							var writer = new StreamWriter(mStream, Encoding.Default);
							using (var t = zipReader.OpenEntryStream())
							{
								StreamReader sr = new StreamReader(t, Encoding.Default);

								while ((line = sr.ReadLine()) != null)
								{

									writer.WriteLine(line);

									count++;
									counter++;
									if (count == 100000)
									{
										mergeStreams = true;
										count = 0;

										writer.Flush();
										mStream.Flush();
										if (!zipReader.Entry.IsDirectory)
										{

											response.Add(Tuple.Create<string, Stream>(filename.Append(filecounter > 0 ? (filecounter).ToString() : String.Empty).Append(" ", zipReader.Entry.Key).Replace(@"\", " ").Replace(@"/", " "), mStream));
											filecounter++;
										}

										mStream = new MemoryStream();
										writer = new StreamWriter(mStream, Encoding.Default);

									}
								}
								writer.Flush();
								mStream.Flush();
								if (mStream.Length > 0 && !zipReader.Entry.IsDirectory)
								{
									response.Add(
										Tuple.Create<string, Stream>(filename.Append(filecounter > 0 ? (filecounter).ToString() : String.Empty).Append(" ", zipReader.Entry.Key).Replace(@"\", " ").Replace(@"/", " "), mStream));
								}

							}

						}
						
					}
				}
				catch (Exception) {
					/*swallow Read funny error on 7 gig files
					 */
				}
			}

			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="filename"></param>
		/// <param name="insideCompretionExtention"></param>
		/// <returns></returns>
		public static IEnumerable<Tuple<string, Stream>> DecompressStreamFiles(this Stream stream, string filename, string insideCompretionExtention)
		{
			
			var response = new List<Tuple<string, Stream>>();
			Stream mStream = default(Stream);
			var compressionReader = SharpCompress.Readers.ReaderFactory.Open(stream);
			while (compressionReader.MoveToNextEntry()) {
				if (insideCompretionExtention.IsNullOrEmpty() || compressionReader.Entry.Key.ToUpperInvariant().Contains(insideCompretionExtention.ToUpperInvariant()))
				{
					mStream = new MemoryStream();
					compressionReader.WriteEntryTo(mStream);

					if (!compressionReader.Entry.IsDirectory)
					{

						response
							.Add(
								Tuple.Create<string, Stream>(
									filename.Append(" ", compressionReader.Entry.Key).Replace(@"\", " ").Replace(@"/", " "),
									mStream));

					}
				}
			}
			return response;
		}

		private static PgpPrivateKey FindSecretKey(PgpSecretKeyRingBundle pgpSec, long keyId, char[] pass) {
			PgpSecretKey pgpSecKey = pgpSec.GetSecretKey(keyId);

			if (pgpSecKey == null) {
				return null;
			}

			return pgpSecKey.ExtractPrivateKey(pass);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="inputStream"></param>
		/// <param name="filename"></param>
		/// <param name="privateKeyStream"></param>
		/// <param name="passPhrase"></param>
		/// <returns></returns>
		public static IEnumerable<Tuple<string, Stream>> DecompressStreamFilesPGP(this Stream inputStream, string filename, Stream privateKeyStream, string passPhrase) {
			var response			= new List<Tuple<string, Stream>>();
			var outputMemoryStream	= new MemoryStream();

			try {
				var pgpFactory			= default(PgpObjectFactory);
				var encryptedDataList	= default(PgpEncryptedDataList);
				var obj					= default(PgpObject);
				var privateKey			= default(PgpPrivateKey);
				var encryptedData		= default(PgpPublicKeyEncryptedData);
				var ringBundle			= default(PgpSecretKeyRingBundle);

				pgpFactory = new PgpObjectFactory(PgpUtilities.GetDecoderStream(inputStream));

				// find secret key
				ringBundle = new PgpSecretKeyRingBundle(PgpUtilities.GetDecoderStream(privateKeyStream));

				if (pgpFactory != null) {
					obj = pgpFactory.NextPgpObject();
				}

				// the first object might be a PGP marker packet.
				if (obj is PgpEncryptedDataList) {
					encryptedDataList = (PgpEncryptedDataList)obj;
				} else {
					encryptedDataList = (PgpEncryptedDataList)pgpFactory.NextPgpObject();
				}

				// decrypt
				foreach (PgpPublicKeyEncryptedData pked in encryptedDataList.GetEncryptedDataObjects()) {
					privateKey = FindSecretKey(ringBundle, pked.KeyId, passPhrase.ToCharArray());

					if (privateKey != null) {
						encryptedData = pked;
						break;
					}
				}

				if (privateKey == null) {
					throw new ArgumentException("Secret key for message not found.");
				}

				PgpObjectFactory plainFact = null;

				using (Stream clear = encryptedData.GetDataStream(privateKey)) {
					plainFact = new PgpObjectFactory(clear);
				}

				PgpObject message = plainFact.NextPgpObject();

				if (message is PgpCompressedData) {
					PgpCompressedData cData = (PgpCompressedData)message;
					PgpObjectFactory of = null;

					using (Stream compDataIn = cData.GetDataStream()) {
						of = new PgpObjectFactory(compDataIn);
					}

					message = of.NextPgpObject();
					if (message is PgpOnePassSignatureList) {
						message = of.NextPgpObject();
						PgpLiteralData Ld = null;
						Ld = (PgpLiteralData)message;
						//using (Stream output = File.Create(outputFile))
						//{
						Stream unc = Ld.GetInputStream();
						Streams.PipeAll(unc, outputMemoryStream);
						//}
					} else {
						PgpLiteralData Ld = null;
						Ld = (PgpLiteralData)message;
						//using (Stream output = File.Create(outputFile))
						//{
						Stream unc = Ld.GetInputStream();
						Streams.PipeAll(unc, outputMemoryStream);
						//}
					}
				} else if (message is PgpLiteralData) {
					PgpLiteralData ld = (PgpLiteralData)message;
					string outFileName = ld.FileName;

					//using (Stream fOut = File.Create(outputFile))
					//{
					Stream unc = ld.GetInputStream();
					Streams.PipeAll(unc, outputMemoryStream);
					//}
				} else if (message is PgpOnePassSignatureList) {
					throw new PgpException("Encrypted message contains a signed message - not literal data.");
				} else {
					throw new PgpException("Message is not a simple encrypted file - type unknown.");
				}
			}
			catch (PgpException ex) {
				throw ex;
			}

			response
				.Add(
					Tuple.Create<string, Stream>(
						filename.Replace(@"\", " ").Replace(@"/", " "),
						outputMemoryStream));

			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] Compress(this byte[] data) {
			var buffer = default(byte[]);
			using (MemoryStream memoryStream = new MemoryStream()) {

				using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true)) {
					gZipStream.Write(data, 0, data.Length);
				}
				memoryStream.Position = 0;

				buffer = new byte[memoryStream.Length];

				memoryStream.Read(buffer, 0, buffer.Length);
			}

			return buffer;
		}

	   

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(this JObject data)
		{
			var buffer = default(byte[]);
			using (MemoryStream memoryStream = new MemoryStream())
			{

				using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
				{
					gZipStream.Write(data.ToString(Formatting.None).ToByteArray(), 0, data.ToString(Formatting.None).Length);
				}
				memoryStream.Position = 0;

				buffer = new byte[memoryStream.Length];

				memoryStream.Read(buffer, 0, buffer.Length);
			}

			return buffer;
		}


	
        /// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string Decompress(this byte[] data )
        {
            var response = String.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(data, 0, data.Length);

                stream.Seek(0, SeekOrigin.Begin);

                using (var gzipstream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    using (var reader = new StreamReader(gzipstream, Encoding.Default))
                    {
                        response = reader.ReadToEnd();
                    }
                }
            }
            return response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<string> DecompressList(this byte[] data)
        {
            var response = new List<string>();
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(data, 0, data.Length);

                stream.Seek(0, SeekOrigin.Begin);

                using (var gzipstream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    using (var reader = new StreamReader(gzipstream, Encoding.Default))
                    {

                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            response.Add(line);
                        }
                        
                    }
                }
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compress"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static byte[] Decompress(this byte[] compress, int bufferSize) {
			byte[] temp = new byte[bufferSize];
			var response = new List<byte>();
			var count = 0;

			using (MemoryStream stream = new MemoryStream()) {
				stream.Write(compress, 0, compress.Length);

				stream.Seek(0, SeekOrigin.Begin);

				using (var gzipstream = new GZipStream(stream, CompressionMode.Decompress)) {
					while ((count = gzipstream.Read(temp, 0, bufferSize)) > 0) {
						response.AddRange(temp.Take(count));
					}
				}
			}
			return response.ToArray();
		}


       
    }
}
