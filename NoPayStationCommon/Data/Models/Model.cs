using System.Text.Json;

namespace NoPayStationCommon.Data.Models
{
    public class Model
    {
        #region Overrides

        /// <summary>
        /// Serialized (JSON) representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            return JsonSerializer.Serialize(this, options);
        }

        #endregion
    }
}
