using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Epine.Infrastructure.Extensions {

	/// <summary>
	///		Defines extension methods for <see cref="StreamReader"/>-based objects.
	/// </summary>
	public static class StreamReaderExtensions {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="streamReader"></param>
		/// <returns></returns>
		public static IEnumerable<string> ToEnumerable(this StreamReader streamReader) {
			while (!streamReader.EndOfStream) {
				yield return streamReader.ReadLine();
			}
		}

        /// <summary>
		/// 
		/// </summary>
		/// <param name="streamReader"></param>
		/// <returns></returns>
        public static IEnumerable<string> ToEnumerable(this StreamReader streamReader, char delimiter,char ignoreSeparatorBetween, int columnCount)
        {
            var line = "";
            var validLine = false;
            while ((line = streamReader.ReadLine()) != null)
            {

                var currentColumnCount = line.SplitStringFix(delimiter, ignoreSeparatorBetween).Count();

                while ( line.Count(c => c == ignoreSeparatorBetween) % 2 != 0 || currentColumnCount < columnCount)
                {
                    line += streamReader.ReadLine();
                    currentColumnCount = line.SplitStringFix(delimiter, ignoreSeparatorBetween).Count();

                    if(currentColumnCount >= columnCount)
                    {
                        break;
                    }
                }
                if (currentColumnCount > columnCount)
                    throw new System.ApplicationException("Column Count to Large");
                yield return line;
            }
        }
    }
}
