using Downloader;
using Microsoft.Extensions.Logging;
using NoPayStationCommon.Data;
using NoPayStationCommon.Settings;
using System.Diagnostics;
using System.Reflection;

namespace NoPayStationCommon
{
    public class AppCore
    {
        #region Static props

        /// <summary>
        /// Logger factory.
        /// </summary>
        public static ILoggerFactory? LogFactory
        {
            get { return _logFactory; }
        }

        #endregion

        #region Static fields

        /// <summary>
        /// Logger factory.
        /// </summary>
        private static ILoggerFactory? _logFactory;

        #endregion

        #region Public static methods

        /// <summary>
        /// Initialize the environment.
        /// </summary>
        public static void Initialize()
        {
            InitializeLogging();
            InitializeDatabase();
        }

        /// <summary>
        /// Returns path to a local resource in the application working directory.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetLocalResource(string path)
        {
            return Path.Combine(GetWorkingDir(), path);
        }

        /// <summary>
        /// Returns download configuration.
        /// </summary>
        /// <param name="isLargeFile"></param>
        /// <returns></returns>
        public static DownloadConfiguration GetDownloadConfiguration(bool isLargeFile = false)
        {
            return new DownloadConfiguration()
            {
                ChunkCount = isLargeFile ? DownloadSettings.Default.ChunkCount : 1,
                MaximumBytesPerSecond = isLargeFile ? DownloadSettings.Default.MaxBps : 0,
                ParallelDownload = isLargeFile ? DownloadSettings.Default.AllowParallelDownload : false,
                ReserveStorageSpaceBeforeStartingDownload = isLargeFile ? DownloadSettings.Default.PreAllocateFile : false,
                RequestConfiguration =
                {
                    UserAgent = GetVersionString()
                }
            };
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Returns the working directory of the application.
        /// </summary>
        /// <returns></returns>
        private static string GetWorkingDir()
        {
            string workingDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NoPayStation");
            if (!Directory.Exists(workingDir))
            {
                Directory.CreateDirectory(workingDir);
            }

            return workingDir;
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Initializes the database.
        /// </summary>
        private static void InitializeDatabase()
        {
            Database.Initialize();
        }

        /// <summary>
        /// Initializes logging.
        /// </summary>
        private static void InitializeLogging()
        {
            _logFactory = LoggerFactory.Create((builder) =>
            {
                builder.SetMinimumLevel(LogLevel.Debug);
            });
        }

        /// <summary>
        /// Returns the version string of currently executing assembly.
        /// </summary>
        /// <returns></returns>
        private static string GetVersionString()
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

            return $"{versionInfo.ProductName}/{versionInfo.ProductMajorPart}.{versionInfo.ProductMinorPart}.{versionInfo.ProductBuildPart}";
        }

        #endregion
    }
}