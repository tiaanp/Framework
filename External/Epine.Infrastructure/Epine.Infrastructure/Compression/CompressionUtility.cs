using System;
using System.Collections.Generic;
using System.IO;

namespace Epine.Infrastructure.Compression {

	/// <summary>
	/// 
	/// </summary>
	public class CompressionUtility : ICompressionUtility {

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		public CompressionUtility(string filename) {

			this._CompressionType =
				CompressionUtility
					.GetCompressionType[
						new FileInfo(filename)
							.Extension
							.ToUpperInvariant()];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="compressionType"></param>
		public CompressionUtility(CompressionType compressionType) {
			this._CompressionType = compressionType;
		}

		#endregion

		#region ICompressionUtility Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="mergeStreams"></param>
		/// <returns></returns>
		public IEnumerable<Tuple<string, Stream>> DecompressStreamFiles(Stream stream, ref bool mergeStreams) {
			var response = default(IEnumerable<Tuple<string, Stream>>);

			if (this._CompressionType != CompressionType.None) {
				response = CompressionUtility.GetUtility[this._CompressionType]().DecompressStreamFiles(stream, ref mergeStreams);
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
			var response = default(Stream);

			if (this._CompressionType != CompressionType.None) {
				response = CompressionUtility.GetUtility[this._CompressionType]().Compress(stream, fileName);
			}

			return response;
		}

        public Stream Compress(IDictionary<string, Stream> files) {
            var response = default(Stream);

            if (this._CompressionType != CompressionType.None) {
                response = CompressionUtility.GetUtility[this._CompressionType]().Compress(files);
            }

            return response;
        }


        public Stream Compress(IDictionary<string, byte[]> files, string fileName){
            var response = default(Stream);



            return response;
        }

		#endregion

		#region Class Fields

		private static readonly IDictionary<CompressionType, Func<ICompressionUtility>> GetUtility =
			new Dictionary<CompressionType, Func<ICompressionUtility>>() {
				{CompressionType.Zip, () => new ZipUtility()},
				{CompressionType.Rar, () => new RarUtility()},
				{CompressionType.GZip, () => new GZiprUtility()}
			};

		private static readonly IDictionary<string, CompressionType> GetCompressionType =
			new Dictionary<string, CompressionType>() {
				{".ZIP", CompressionType.Zip},
				{".RAR", CompressionType.Rar},
				{".GZ", CompressionType.GZip}
			};

		#endregion

		#region Instance Fields

		private readonly CompressionType _CompressionType;

		#endregion

	}


}
