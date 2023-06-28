using NoPayStationCommon.Data.Models;
using SQLite;

namespace NoPayStationCommon.Data
{
    public class Database
    {
        #region Static props

        /// <summary>
        /// Connection to the SQLite database.
        /// </summary>
        public static SQLiteConnection? Connection
        {
            get { return _connection; }
        }

        #endregion

        #region Static fields

        /// <summary>
        /// Connection to the SQLite database.
        /// </summary>
        private static SQLiteConnection? _connection;

        #endregion

        #region Public static methods

        /// <summary>
        /// Initializes the SQLite database and creates tables.
        /// </summary>
        public static void Initialize()
        {
            _connection = new SQLiteConnection(AppCore.GetLocalResource("database.db"));
            _connection.CreateTable<TitleModel>();
        }

        #endregion
    }
}
