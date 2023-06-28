using Microsoft.Extensions.Logging;
using NoPayStationCommon.Data.Models;
using NoPayStationCommon.Tsv;

namespace NoPayStationCommon.Data.Seeders
{
    public class TsvSeeder
    {
        #region Static fields

        /// <summary>
        /// Local logger.
        /// </summary>
        private static ILogger? _logger = AppCore.LogFactory?.CreateLogger<TsvSeeder>();

        #endregion

        #region Public static methods

        /// <summary>
        /// Seed the database with TSV file contents.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Seed(string fileName)
        {
            _logger?.LogInformation($"Seeding {fileName}");

            TsvFile tsvFile = new TsvFile(fileName);
            foreach (Dictionary<string, string> dict in tsvFile)
            {
                try
                {
                    TitleModel titleModel = new TitleModel()
                    {
                        TitleId = GetTitleId(dict),
                        ContentId = GetContentId(dict),
                        Region = GetTitleRegion(dict),
                        Name = GetName(dict),
                        LastModifiedDate = GetLastModifiedDate(dict),
                        PkgFileUrl = GetPkgFileUrl(dict),
                        PkgFileSize = GetPkgFileSize(dict),
                        PkgFileHash = GetPkgFileHash(dict),
                        Rap = GetRap(dict),
                        RapFileUrl = GetRapFileUrl(dict),
                        IsRapRequired = IsRapRequired(dict),
                        IsLicenseUnlockedByDlc = IsLicenseUnlockedByDlc(dict)
                    };

                    Database.Connection?.InsertOrReplace(titleModel, typeof(TitleModel));

                    _logger?.LogInformation($"Inserted {titleModel}");
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex.ToString());
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Gets title ID from the supplied dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static string GetTitleId(Dictionary<string, string> dict)
        {
            return dict["Title ID"];
        }

        /// <summary>
        /// Gets content ID from the supplied dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static string GetContentId(Dictionary<string, string> dict)
        {
            return dict["Content ID"];
        }

        /// <summary>
        /// Gets title region from the supplied dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static TitleModel.TitleRegion GetTitleRegion(Dictionary<string, string> dict)
        {
            return Enum.Parse<TitleModel.TitleRegion>(dict["Region"]);
        }

        /// <summary>
        /// Gets title name from the supplied dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static string GetName(Dictionary<string, string> dict)
        {
            return dict["Name"];
        }

        /// <summary>
        /// Gets title last modified date from the supplied dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static DateTime GetLastModifiedDate(Dictionary<string, string> dict)
        {
            return DateTime.Parse(dict["Last Modification Date"]);
        }

        /// <summary>
        /// Gets PKG file URL from the supplied dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static Uri? GetPkgFileUrl(Dictionary<string, string> dict)
        {
            return dict["PKG direct link"].Contains("http") ? new Uri(dict["PKG direct link"]) : null;
        }

        /// <summary>
        /// Gets PKG file size from the supplied dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static long GetPkgFileSize(Dictionary<string, string> dict)
        {
            return !string.IsNullOrEmpty(dict["File Size"]) ? long.Parse(dict["File Size"]) : 0;
        }

        /// <summary>
        /// Gets PKG file hash from the supplied dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static string GetPkgFileHash(Dictionary<string, string> dict)
        {
            return dict["SHA256"];
        }

        /// <summary>
        /// Gets RAP from the suppplied dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static string GetRap(Dictionary<string, string> dict)
        {
            return !string.IsNullOrEmpty(dict["RAP"]) || dict["RAP"] != "NOT REQUIRED" || dict["RAP"] != "UNLOCK/LICENSE BY DLC" || dict["RAP"] != "MISSING" ? dict["RAP"] : string.Empty;
        }

        /// <summary>
        /// Gets RAP file URL from the supplied dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static Uri? GetRapFileUrl(Dictionary<string, string> dict)
        {
            return dict["Download .RAP file"].Contains("http") ? new Uri(dict["PKG direct link"]) : null;
        }

        /// <summary>
        /// Checks whether or not RAP is required for a particular entry in the dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static bool IsRapRequired(Dictionary<string, string> dict)
        {
            return GetRap(dict) != null || GetRapFileUrl(dict) != null || dict["RAP"] == "MISSING" || dict["Download .RAP file"] == "MISSING" || dict["RAP"] != "UNLOCK/LICENSE BY DLC" || dict["Download .RAP file"] != "UNLOCK/LICENSE BY DLC";
        }

        /// <summary>
        /// Checks whether or not a license is unlocked by a DLC for a particular entry in the dictionary.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static bool IsLicenseUnlockedByDlc(Dictionary<string, string> dict)
        {
            return dict["RAP"] == "UNLOCK/LICENSE BY DLC" || dict["Download .RAP file"] == "UNLOCK/LICENSE BY DLC";
        }

        #endregion
    }
}
