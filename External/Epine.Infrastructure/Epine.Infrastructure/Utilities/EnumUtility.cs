using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epine.Infrastructure.Extensions;

namespace Epine.Infrastructure.Utilities
{

    /// <summary>
    /// 
    /// </summary>
    public static class EnumUtility
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(string value)
            => (T)Enum.Parse(typeof(T), value, true);

        /// <summary>
        /// Converts a Bitwise enum int value to a Dictionary<long,string>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<long, string> ParseBitwiseEnum<T>(Int32 value)
            => (!typeof(T).IsEnum) ? throw new InvalidOperationException("<T> must be Enum") :
                BitwiseDictionaryExtraction<T>(Enum.Parse(typeof(T), value.ToString()).ToString());

        /// <summary>
        /// Converts a Bitwise enum string value to a Dictionary<long,string>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<long, string> ParseBitwiseEnum<T>(string stringCollection)
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("<T> must be Enum");

            return BitwiseDictionaryExtraction<T>(stringCollection);
        }

        /// <summary>
        /// Returns a collection of enum values 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stringCollection"></param>
        /// <returns></returns>
        public static IEnumerable<string> ParseBitwiseEnumAsList<T>(string stringCollection)
        {
            var collection = new List<string>();
            var count = stringCollection.Count(x => x == ',');

            for (var i = 0; i < count; i++)
            {
                collection.Add(stringCollection.Substring(0, stringCollection.IndexOf(',')));
                stringCollection = stringCollection.Remove(0, stringCollection.IndexOf(',') + 2);
            }

            if (!stringCollection.IsNotNullOrEmpty()) return collection;

            collection.Add(stringCollection);

            return collection;
        }

        public static object ParseListAsBitwiseEnum<T>(List<string> collection)
        {
            var flatCollectionString = string.Join(", ", collection);            
            return Enum.Parse(typeof(T), (flatCollectionString.IsNullOrEmpty()) ? "None" : flatCollectionString, true);
        }

        /// <summary>
        /// Helper function for bitwise dictionary operations
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stringCollection"></param>
        /// <returns></returns>
        private static Dictionary<long, string> BitwiseDictionaryExtraction<T>(string stringCollection)
        {
            var collection = new Dictionary<long, string>();
            var count = stringCollection.Count(x => x == ',');

            for (var i = 0; i < count; i++)
            {
                collection.Add((Int32)Enum.Parse(typeof(T), stringCollection.Substring(0, stringCollection.IndexOf(',')), true),
                    stringCollection.Substring(0, stringCollection.IndexOf(',')));
                stringCollection = stringCollection.Remove(0, stringCollection.IndexOf(',') + 1);
            }

            if (!stringCollection.IsNotNullOrEmpty()) return collection;

            stringCollection = stringCollection.Remove(0, stringCollection.IndexOf(',') + 1);
            var key = (Int32)Enum.Parse(typeof(T), stringCollection, true);
            collection.Add(key, stringCollection);

            return collection;
        }
    }
}
