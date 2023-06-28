using SQLite;

namespace NoPayStationCommon.Data.Models
{
    public class TitleModel : Model
    {
        #region Enums

        /// <summary>
        /// Defines possible regions of a database (title) entry.
        /// </summary>
        public enum TitleRegion
        {
            None,
            EU,
            US,
            JP,
            ASIA
        }

        #endregion

        #region Props

        /// <summary>
        /// Primary key.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Title ID.
        /// </summary>
        public string? TitleId { get; set; }

        /// <summary>
        /// Content ID
        /// </summary>
        public string? ContentId { get; set; }

        /// <summary>
        /// Region of the title.
        /// </summary>
        public TitleRegion Region = TitleRegion.None;

        /// <summary>
        /// Title name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Last modified date.
        /// </summary>
        public DateTime? LastModifiedDate { get; set; } = default;

        /// <summary>
        /// URL of the PKG file.
        /// </summary>
        public Uri? PkgFileUrl { get; set; }

        /// <summary>
        /// Size of the PKG file.
        /// </summary>
        public long? PkgFileSize { get; set; }

        /// <summary>
        /// SHA256 hash of the PKG file.
        /// </summary>
        public string? PkgFileHash { get; set; }

        /// <summary>
        /// RAP (key?).
        /// </summary>
        public string? Rap { get; set; }

        /// <summary>
        /// URL of the RAP file.
        /// </summary>
        public Uri? RapFileUrl { get; set; }

        /// <summary>
        /// Is RAP or RAP file required by this title?
        /// </summary>
        public bool IsRapRequired { get; set; } = false;

        /// <summary>
        /// Is the license unlocked by a DLC?
        /// </summary>
        public bool IsLicenseUnlockedByDlc { get; set; } = false;

        #endregion

        #region Public static methods

        /// <summary>
        /// Get the table for this model.
        /// </summary>
        /// <returns></returns>
        public static TableQuery<TitleModel>? Table() => Database.Connection?.Table<TitleModel>();

        #endregion
    }
}
