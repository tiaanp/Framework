using System;
using System.Collections.Generic;
using System.IO;

namespace Epine.Infrastructure.Compression
{
	/// <summary>
	/// 
	/// </summary>
    public class RarUtility : ICompressionUtility {

		#region ICompressionUtility Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="mergeStreams"></param>
		/// <returns></returns>
		public IEnumerable<Tuple<string, Stream>> DecompressStreamFiles(Stream stream, ref bool mergeStreams)
        {
             var response = new List<Tuple<string, Stream>>();
            Stream mStream = default(Stream);
            var rarReader = SharpCompress.Readers.Rar.RarReader.Open(stream);
            while (rarReader.MoveToNextEntry())
            {
                mStream = new MemoryStream();
                rarReader.WriteEntryTo(mStream);

				if (!rarReader.Entry.IsDirectory) {

					response.Add(Tuple.Create<string, Stream>(rarReader.Entry.Key.Replace(@"\", " "), mStream));
				}
            }
            return response;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public Stream Compress(Stream stream, string fileName) {
			throw new NotImplementedException();
		}

        public Stream Compress(IDictionary<string, Stream> files) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
