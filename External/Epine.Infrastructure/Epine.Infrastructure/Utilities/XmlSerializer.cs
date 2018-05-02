using Epine.Infrastructure.Extensions;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Epine.Infrastructure.Utilities {

	/// <summary>
	///		XML serialization and de-serialization of <typeparamref name="T"/> 
	///		to/from an XML file or <see cref="Stream"/>.
	/// </summary>
	public static class XmlSerializer<T>
		where T : class {
		
		#region Class methods

		/// <summary>
		///     Loads a <typeparamref name="T"/>-based class from an XML file.
		/// </summary>
		/// <param name="path">
		///     Path of the file to load a <typeparamref name="T"/>-based class from.
		/// </param>
		/// <returns>
		///     <typeparamref name="T"/>-based class loaded from an XML file.
		/// </returns>
		public static T LoadFile(string path) {

			T serializableObject = null;			

			using (TextReader textReader = new StreamReader(path)) {
				serializableObject =
					XmlSerializer<T>
						.CreateXmlSerializer()
						.Deserialize(textReader)
						.As<T>();
			}

			return serializableObject;
		}

		/// <summary>
		///		De-serializes <paramref name="stream"/> 
		///		to a <typeparamref name="T"/>-based class.
		/// </summary>
		/// <param name="stream">
		///		The <see cref="Stream"/> containing
		///		the xml to de-serialize.
		/// </param>
		/// <returns>
		///		<typeparamref name="T"/>-based class 
		///		loaded from <paramref name="stream"/>.
		/// </returns>
		public static T LoadStream(Stream stream) {

			T serializableObject = null;			

			using (TextReader textReader = new StreamReader(stream)) {
				serializableObject =
					XmlSerializer<T>
						.CreateXmlSerializer()
						.Deserialize(textReader)
						.As<T>();
			}

			return serializableObject;
		}

		/// <summary>
		///     Loads a <typeparamref name="T"/>-based 
		///     class from an XML <see cref="string"/>.
		/// </summary>
		/// <param name="xml">
		///     The <see cref="string"/> containing the XML.
		/// </param>
		/// <returns>
		///     <typeparamref name="T"/>-based class loaded 
		///     from an XML <see cref="string"/>.
		/// </returns>
		public static T LoadXml(string xml) {

			T serializableObject = null;			

			using (StringReader reader = new StringReader(xml)) {
				serializableObject =
					XmlSerializer<T>
						.CreateXmlSerializer()
						.Deserialize(reader)
						.As<T>();
			}

			return serializableObject;
		}

		/// <summary>
		///     Saves a <typeparamref name="T"/>-based class to an XML file.
		/// </summary>
		/// <param name="serializableObject">
		///     A Serializable <typeparamref name="T"/>-based 
		///     class to be saved to file.
		/// </param>
		/// <param name="path">
		///     The path to the file.
		/// </param>
		public static void Save(T serializableObject, string path) {
			
			using (TextWriter textWriter = new StreamWriter(path)) {
				XmlSerializer<T>
					.CreateXmlSerializer()
					.Serialize(
						textWriter, 
						serializableObject);
			}
		}

		/// <summary>
		///     Converts a <typeparamref name="T"/>-based class to a XML <see cref="string"/>
		/// </summary>
		/// <param name="serializableObject">
		///     A Serializable <typeparamref name="T"/>-based 
		///     class to be saved to file.
		/// </param>
		public static string Serialize(T serializableObject) {

			return 
				XmlSerializer<T>.Serialize(serializableObject, Encoding.Default);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serializableObject"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static string Serialize(T serializableObject, Encoding encoding) {
			var response = default(string);
			var stream = new MemoryStream();

			using (var xmlWriter = new XmlTextWriter(stream, encoding)) {
				XmlSerializer<T>
					.CreateXmlSerializer()
					.Serialize(
						xmlWriter,
						serializableObject);

				stream = (MemoryStream)xmlWriter.BaseStream;
				response = stream.ToArray().ExtractString();
			}

			return response;
		}

		/// <summary>
		///		Creates a new <see cref="XmlSerializer"/> 
		///		for a <typeparamref name="T"/>-based class.
		/// </summary>
		/// <returns>
		///		A new <see cref="XmlSerializer"/>.
		///	</returns>
		private static XmlSerializer CreateXmlSerializer() {
			return new XmlSerializer(typeof(T));
		}

		#endregion
	}
}
