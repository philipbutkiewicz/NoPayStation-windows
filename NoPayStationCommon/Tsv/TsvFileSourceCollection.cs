using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NoPayStationCommon.Tsv
{
    [Serializable]
    public class TsvFileSourceCollection : List<TsvFileSource>
    {

        #region Public static methods

        /// <summary>
        /// Creates TsvFileSourceCollection from a serialized JSON file.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static TsvFileSourceCollection? FromFile(string fileName)
        {
            return FromString(File.ReadAllText(fileName));
        }

        /// <summary>
        /// Creates TsvFileSourceCollection from a serialized JSON object.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static TsvFileSourceCollection? FromString(string str)
        {
            return JsonSerializer.Deserialize<TsvFileSourceCollection>(str);
        }

        #endregion
    }
}
