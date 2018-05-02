using Epine.Infrastructure.Extensions;
using ICSharpCode.SharpZipLib.GZip;
using SharpCompress.Readers.GZip;
using System;
using System.Collections.Generic;
using System.IO;

namespace Epine.Infrastructure.Compression {
    internal class GZiprUtility : ICompressionUtility {
        public IEnumerable<Tuple<string, Stream>> DecompressStreamFiles(Stream stream, ref bool mergeStreams) {
            var response = new List<Tuple<string, Stream>>();
            Stream mStream = default(Stream);
            var gzipReader = GZipReader.Open(stream);
            while (gzipReader.MoveToNextEntry()) {
                mStream = new MemoryStream();
                gzipReader.WriteEntryTo(mStream);

                if (!gzipReader.Entry.IsDirectory) {
                    response.Add(Tuple.Create<string, Stream>(gzipReader.Entry.Key.Replace(@"\", " "), mStream));
                }
            }
            return response;

        }


        public Stream Compress(Stream stream, string fileName) {
            var response = default(Stream);
            var writeStream = new MemoryStream();

            // Make sure stream position is at the beginning.
            stream.Position = 0;

			using (var zipStream = new GZipOutputStream(writeStream)) {

				var bytes = stream.ToByteArray();

				zipStream.SetLevel(7);
				zipStream.Write(bytes, 0, bytes.Length);
				zipStream.Finish();
				writeStream.Position = 0;

				response = writeStream.ToMemoryStream();
			}

			return response;
        }

        public Stream Compress(IDictionary<string, Stream> files) {
            throw new NotImplementedException();
        }
    }
}
