namespace NoPayStationCommon.Tsv
{
    public class TsvFile : List<Dictionary<string, string>>
    {
        #region Fields
        
        /// <summary>
        /// Path of this TSV file.
        /// </summary>
        private string _fileName;

        #endregion

        #region Constructor

        /// <summary>
        /// TSV File Dictionary
        /// </summary>
        /// <param name="fileName"></param>
        public TsvFile(string fileName) : base()
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("File name cannot be null.");
            }

            _fileName = fileName;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Reads a TSV file and stores all its values.
        /// </summary>
        /// <param name="fileName"></param>
        /// <exception cref="InvalidDataException"></exception>
        private void Read()
        {
            using (StreamReader streamReader = new StreamReader(_fileName))
            {
                List<string> keys = new List<string>(ParseLine(streamReader.ReadLine()));
                string? line = string.Empty;
                while (string.IsNullOrEmpty(streamReader.ReadLine()))
                {
                    string[] row = ParseLine(line);
                    if (row.Length != keys.Count)
                    {
                        throw new InvalidDataException($"Line does not contain the right amount of values. Expecting {keys.Count}, got {row.Length}.");
                    }

                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    int valueIndex = 0;
                    foreach (string key in keys)
                    {
                        dict.Add(key, row[valueIndex]);
                    }

                    Add(dict);
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Parses a single line of TSV data.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        private string[] ParseLine(string? line)
        {
            if (line == null)
            {
                throw new InvalidDataException("Supplied line was empty");
            }

            string[] values = line.Split('\t');
            if (values.Length == 0)
            {
                throw new InvalidDataException("Line is not in TSV format");
            }

            return values;
        }

        #endregion
    }
}