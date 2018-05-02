using System;
using System.Collections.Generic;
using System.IO;

namespace Epine.Infrastructure.Compression
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICompressionUtility
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="mergeStreams"></param>
		/// <returns></returns>
		IEnumerable<Tuple<string, Stream>> DecompressStreamFiles(Stream stream, ref bool mergeStreams);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		Stream Compress(Stream stream, string fileName);

        Stream Compress(IDictionary<string, Stream> files);
    }
}
