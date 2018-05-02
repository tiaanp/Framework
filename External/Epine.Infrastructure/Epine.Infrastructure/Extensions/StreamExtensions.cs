using System.IO;

namespace Epine.Infrastructure.Extensions
{
	/// <summary>
	/// 
	/// </summary>
	public static class StreamExtensions
	{
		#region Constants

		private const int BYTE_BUFFER = 1024;

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		public static void TryResetStream(this Stream stream)
		{

			if (stream.Position != 0)
			{
				stream.Seek(0, SeekOrigin.Begin);
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static MemoryStream ToMemoryStream(this Stream stream)
		{
			return new MemoryStream(stream.ToByteArray());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static byte[] ToByteArray(this Stream stream)
		{
			var totalLength = stream.Length;
			var content = new byte[totalLength];
			var offSet = 0;
			var byteLength =
				totalLength > StreamExtensions.BYTE_BUFFER
					? StreamExtensions.BYTE_BUFFER
					: (int)totalLength;

			stream.TryResetStream();

			using (var reader = new BinaryReader(stream))
			{
				while (reader.Read(content, offSet, byteLength) > 0)
				{

					offSet += byteLength;
					//Thread.Sleep(500);

					byteLength =
						(totalLength - reader.BaseStream.Position) > StreamExtensions.BYTE_BUFFER
							? StreamExtensions.BYTE_BUFFER
							: (int)(totalLength - reader.BaseStream.Position);
				}
			}


			return content;
		}
	}
}
