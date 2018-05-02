using Epine.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;

namespace Epine.Infrastructure.Compression {
	/// <summary>
	/// 
	/// </summary>
	public class ZipUtility : ICompressionUtility
    {

        #region Class Methods


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] CompressFile(byte[] data, string fileName)
        {
            using (MemoryStream output = new MemoryStream())
            {
                using (var zipStream = new ZipOutputStream(output))
                {
                    string s = data.ExtractString();

                    ZipEntry entry = new ZipEntry(fileName);
                    entry.DateTime = DateTime.Now;
                    entry.Size = s.Length;

                    zipStream.PutNextEntry(entry);
                    zipStream.SetLevel(7);
                    zipStream.Write(data, 0, data.Length);
                    zipStream.Finish();

                    return output.ToArray();
                }
            }
        }

        public static byte[] Compress(IDictionary<string, byte[]> files)
        {

            var response = default(byte[]);

            using (MemoryStream output = new MemoryStream())
            {
                using (var zipStream = new ZipOutputStream(output))
                {

                    files.ForEachElement(
                        file =>
                        {

                            ZipEntry entry = new ZipEntry(file.Key);
                            entry.DateTime = DateTime.Now;
                            entry.Size = file.Value.Length;
                            zipStream.PutNextEntry(entry);
                            zipStream.SetLevel(7);
                            zipStream.Write(file.Value, 0, file.Value.Length);
                            //zipStream.CloseEntry();
                        });

                    zipStream.Finish();
                    zipStream.Close();
                }
                response = output.ToArray();
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawdata"></param>
        /// <returns></returns>
        public static byte[] Compress(string rawdata)
        {
            var data = rawdata.ToByteArray();
            using (MemoryStream memoryStream = new MemoryStream())
            {

                using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gZipStream.Write(data, 0, data.Length);
                }
                memoryStream.Position = 0;

                byte[] buffer = new byte[memoryStream.Length];

                memoryStream.Read(buffer, 0, buffer.Length);

                return buffer;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CompressFile(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {

                using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gZipStream.Write(data, 0, data.Length);
                }
                memoryStream.Position = 0;

                byte[] buffer = new byte[memoryStream.Length];

                memoryStream.Read(buffer, 0, buffer.Length);

                return buffer;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] CompressStream(Stream stream)
        {
            var response = default(byte[]);
            var bytes = new byte[1024];
            // Make sure stream position is at the beginning.
            stream.Position = 0;

            using (var memoryStream = new MemoryStream())
            {

                using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {

                    while (stream.Read(bytes, 0, 1024) > 0)
                    {
                        gZipStream.Write(
                            bytes.Where(b => b > 0).ToArray(),
                            0,
                            bytes.Count(b => b > 0));

                        bytes = new byte[1024];
                    }
                }
                // Reset zip stream.
                memoryStream.Position = 0;

                response = new byte[memoryStream.Length];

                memoryStream.Read(response, 0, response.Length);
            }

            return response;
        }

        public static byte[] Compress(byte[] raw)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory,
                CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compress"></param>
        /// <returns></returns>
        public static string Decompress(byte[] compress)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(compress, 0, compress.Length);

                stream.Seek(0, SeekOrigin.Begin);

                using (var gzipstream = new GZipStream(stream, CompressionMode.Decompress))
                using (var reader = new StreamReader(gzipstream, System.Text.Encoding.Default))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compress"></param>
        /// <returns></returns>
        public static StreamReader DecompressStream(byte[] compress)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(compress, 0, compress.Length);

            stream.Seek(0, SeekOrigin.Begin);

            var gzipstream = new GZipStream(stream, CompressionMode.Decompress);
            var reader = new StreamReader(gzipstream, System.Text.Encoding.Default);


            return reader;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compress"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] compress, int bufferSize)
        {
            byte[] temp = new byte[bufferSize];
            var response = new List<byte>();
            var count = 0;

            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(compress, 0, compress.Length);

                stream.Seek(0, SeekOrigin.Begin);

                using (var gzipstream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    while ((count = gzipstream.Read(temp, 0, bufferSize)) > 0)
                    {
                        response.AddRange(temp.Take(count));
                    }
                }
            }
            return response.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compress"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Decompress(byte[] compress, System.Text.Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(compress, 0, compress.Length);

                stream.Seek(0, SeekOrigin.Begin);

                using (var gzipstream = new GZipStream(stream, CompressionMode.Decompress))
                using (var reader = new StreamReader(gzipstream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compress"></param>
        /// <returns></returns>
        public static string DecompressEncodeing(byte[] compress)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(compress, 0, compress.Length);

                stream.Seek(0, SeekOrigin.Begin);

                using (var gzipstream = new GZipStream(stream, CompressionMode.Decompress))
                using (var reader = new StreamReader(gzipstream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipStream"></param>
        /// <returns></returns>
        public static List<byte[]> Decompress(Stream zipStream)
        {
            ZipEntry entry;
            List<byte[]> files = new List<byte[]>();

            try
            {
                bool read = true;
                using (ZipInputStream s = new ZipInputStream(zipStream))
                {
                    while (read)
                    {
                        // Get the next file in the stream
                        entry = s.GetNextEntry();

                        // Read file from stream and add to byte array
                        if (entry != null)
                        {
                            long size = entry.Size;
                            byte[] buffer = new byte[size];
                            size = s.Read(buffer, 0, (int)size);
                            files.Add(buffer);
                        }
                        else
                        {
                            read = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Return list of byte arrays
            return files;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipStream"></param>
        /// <returns></returns>
        public static Dictionary<string, byte[]> DecompressWithFileNames(Stream zipStream)
        {
            ZipEntry entry;
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();

            try
            {
                bool read = true;
                using (ZipInputStream s = new ZipInputStream(zipStream))
                {
                    while (read)
                    {
                        // Get the next file in the stream
                        entry = s.GetNextEntry();

                        // Read file from stream and add to byte array
                        if (entry != null)
                        {
                            long size = entry.Size;
                            byte[] buffer = new byte[size];
                            size = s.Read(buffer, 0, (int)size);
                            files.Add(entry.Name, buffer);
                        }
                        else
                        {
                            read = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Return list of byte arrays
            return files;
        }

        /// <summary>
        ///		Extracts the zipped files contained 
        ///		within parameter <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">
        ///		A <see cref="Stream"/> instance containing 
        ///		a collection of zipped files.
        /// </param>
        /// <returns>
        ///		A <see cref="IEnumerable{Stream}"/> 
        ///		containing the decompressed files.
        /// </returns>
        public static IEnumerable<Stream> DecompressZipStream(Stream stream)
        {

            stream.Position = 0;
            var response = new List<Stream>();
            var entry = default(ZipEntry);
            var buffer = default(byte[]);
            var offset = 0;
            var count = 1024;
            var output = default(MemoryStream);
            var actualCount = 0;

            using (var zipStream = new ZipInputStream(stream))
            {
                // Iterate over zip collection.
                while ((entry = zipStream.GetNextEntry()) != null)
                {

                    output = new MemoryStream();

                    while (offset < entry.Size)
                    {

                        actualCount =
                            ((entry.Size - offset) > 1204)
                                ? count
                                : (int)(entry.Size - offset);

                        offset +=
                            zipStream.Read(
                                (buffer = new byte[actualCount]),
                                0,
                                actualCount);

                        output.Write(buffer, 0, buffer.Length);
                    }

                    response.Add(output);
                }
            }

            // Return accordingly.
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, Stream>> GZipDecompress(Stream stream, string filename)
        {

            var tuples = new List<Tuple<string, Stream>>();
            using (var gzipstream = new GZipStream(stream, CompressionMode.Decompress))
            using (var reader = new StreamReader(gzipstream, System.Text.Encoding.Default))
            {
                tuples.Add(Tuple.Create<string, Stream>(filename, reader.BaseStream));
            }
            return tuples;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, Stream>> DecompressZipStreamFiles(Stream stream)
        {

            stream.Position = 0;
            var response = new List<Tuple<string, Stream>>();
            var entry = default(ZipEntry);
            var buffer = default(byte[]);
            var offset = default(long);
            var count = 1024;
            var output = default(MemoryStream);
            var actualCount = 0;
            var name = default(string);
            var lastIndex = 0;

            using (var zipStream = new ZipInputStream(stream))
            {
                // Iterate over zip collection.
                while ((entry = zipStream.GetNextEntry()) != null)
                {

                    output = new MemoryStream();
                    offset = 0;
                    while (offset < entry.Size)
                    {

                        actualCount =
                            ((entry.Size - offset) > 1204)
                                ? count
                                : (int)(entry.Size - offset);

                        offset +=
                            zipStream.Read(
                                (buffer = new byte[actualCount]),
                                0,
                                actualCount);

                        output.Write(buffer, 0, buffer.Length);
                    }
                    name =
                        entry.Name
                            .Replace("_", String.Empty)
                            .Replace("[", String.Empty)
                            .Replace("]", String.Empty);
                    lastIndex = name.LastIndexOf(".", System.StringComparison.Ordinal);

                    response.Add(
                        Tuple.Create<string, Stream>(
                        name.Substring(0, (lastIndex > 0 ? lastIndex : name.Length)),
                        output));
                }
            }

            // Return accordingly.
            return response;
        }



        /// <summary>
        /// This method is used to decompress/unzip a zipped file to a specified directory.
        /// </summary>
        /// <param name="fileFullPath">The full path of the file you want to unzip.</param>
        /// <param name="targetDirectoryFullPath">The full path of the directory to which you want to unzip the file.</param>
        public static void DecompressToDirectory(string fileFullPath, string targetDirectoryFullPath)
        {
            FastZip fastZip = new FastZip();
            fastZip.ExtractZip(fileFullPath, targetDirectoryFullPath, "");
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="mergeStream"></param>
        /// <returns></returns>
        public IEnumerable<Tuple<string, Stream>> DecompressStreamFiles(Stream stream, ref bool mergeStream)
        {
            stream.Position = 0;
            var response = new List<Tuple<string, Stream>>();
            var entry = default(ZipEntry);
            var buffer = default(byte[]);
            var offset = default(long);
            var count = 1024;
            var output = default(MemoryStream);
            var actualCount = 0;
            var name = default(string);
            var lastIndex = 0;

            using (var zipStream = new ZipInputStream(stream))
            {
                // Iterate over zip collection.
                while ((entry = zipStream.GetNextEntry()) != null)
                {

                    output = new MemoryStream();
                    offset = 0;
                    while (offset < entry.Size)
                    {

                        actualCount =
                            ((entry.Size - offset) > 1204)
                                ? count
                                : (int)(entry.Size - offset);

                        offset +=
                            zipStream.Read(
                                (buffer = new byte[actualCount]),
                                0,
                                actualCount);

                        output.Write(buffer, 0, buffer.Length);
                    }
                    name =
                        entry.Name
                            .Replace("_", String.Empty)
                            .Replace("[", String.Empty)
                            .Replace("]", String.Empty)
                            .Replace(@"\", " ");
                    lastIndex = name.LastIndexOf(".", System.StringComparison.Ordinal);

                    response.Add(
                        Tuple.Create<string, Stream>(
                        name.Substring(0, (lastIndex > 0 ? lastIndex : name.Length)),
                        output));
                }
            }

            // Return accordingly.
            return response;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public Stream Compress(Stream stream, string fileName) {
			var meg = 1048576;
			
			// Allocate a block of unmanaged memory.
			//IntPtr ptr = new IntPtr(stream.Length);
			//IntPtr memIntPtr = Marshal.AllocHGlobal(ptr);

			//// Get a byte pointer from the unmanaged memory block.
			//byte* memBytePtr = (byte*)memIntPtr.ToPointer();


			//var writeStream = new UnmanagedMemoryStream(memBytePtr, stream.Length, stream.Length, FileAccess.ReadWrite);
			var writeStream = new MemoryStream();

			// Make sure stream position is at the beginning.
			stream.Position = 0;

			var readSize = default(int);
			var zipStream = new ZipOutputStream(writeStream);

			var bytes = new byte[meg];


			ZipEntry entry = new ZipEntry(fileName);
			entry.DateTime = DateTime.Now;
			entry.Size = stream.Length;
			zipStream.PutNextEntry(entry);
			zipStream.SetLevel(7);


			var binarReader = new BinaryReader(stream);

			while (binarReader.BaseStream.Position < binarReader.BaseStream.Length) {
				readSize =
					meg < (binarReader.BaseStream.Length - binarReader.BaseStream.Position)
						? meg
						: (int)(binarReader.BaseStream.Length - binarReader.BaseStream.Position);


				zipStream.Write(binarReader.ReadBytes(readSize), 0, readSize);
			}

			zipStream.Finish();
			writeStream.Position = 0;

			return writeStream;
		}


        public Stream Compress(IDictionary<string, Stream> files)
        {

            var response = default(Stream);
            var writeStream = new MemoryStream();



            using (var zipStream = new ZipOutputStream(writeStream))
            {

                files.ForEachElement(
                    file =>
                    {
                        // Make sure stream position is at the beginning.
                        file.Value.Position = 0;

                        var bytes = file.Value.ToByteArray();

                        ZipEntry entry = new ZipEntry(file.Key);
                        entry.DateTime = DateTime.Now;
                        entry.Size = bytes.Length;

                        zipStream.PutNextEntry(entry);
                        zipStream.SetLevel(7);
                        zipStream.Write(bytes, 0, bytes.Length);
                    });

                zipStream.Finish();
                writeStream.Position = 0;

                response = writeStream.ToMemoryStream();
            }

            return response;
        }



        #endregion
    }
}
