using Downloader;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NoPayStationCommon.Tsv
{
    [Serializable]
    public class TsvFileSource
    {
        #region Event args

        /// <summary>
        /// Provides data for the DownloadCompleted event.
        /// </summary>
        public class DownloadCompletedEventArgs : EventArgs
        {
            /// <summary>
            /// Has the action concluded with success?
            /// </summary>
            public bool Success { get; set; } = false;

            /// <summary>
            /// Error associated with this event.
            /// </summary>
            public Exception? Error { get; set; }

            public DownloadCompletedEventArgs(bool success, Exception? error)
            {
                Success = success;
                Error = error;
            }
        }

        /// <summary>
        /// Provides data for the DownloadProgress event.
        /// </summary>
        public class DownloadProgressEventArgs : EventArgs
        {
            /// <summary>
            /// Percentage of the file downloaded.
            /// </summary>
            public double ProgressPercentage = 0d;

            /// <summary>
            /// Amount of bytes downloaded.
            /// </summary>
            public long BytesReceived = 0;

            /// <summary>
            /// Amount of bytes to download (Content-Length).
            /// </summary>
            public long TotalBytesToReceive = 0;

            /// <summary>
            /// Average download speed in Bps.
            /// </summary>
            public double AvgBpsSpeed = 0d;

            public DownloadProgressEventArgs(double progressPercentage, long bytesReceived, long totalBytesToReceive, double avgBpsSpeed)
            {
                ProgressPercentage = progressPercentage;
                BytesReceived = bytesReceived;
                TotalBytesToReceive = totalBytesToReceive;
                AvgBpsSpeed = avgBpsSpeed;
            }
        }

        #endregion

        #region Public event handlers

        /// <summary>
        /// Called when the TSV file is successfully downloaded from this source.
        /// </summary>
        public event EventHandler<DownloadCompletedEventArgs>? DownloadCompleted;

        /// <summary>
        /// Called when the TSV file download progress is updated.
        /// </summary>
        public event EventHandler<DownloadProgressEventArgs>? DownloadProgress;

        #endregion

        #region Props

        /// <summary>
        /// TSV file associated with this source.
        /// </summary>
        [JsonIgnore]
        public TsvFile File
        {
            get
            {
                return new TsvFile(LocalFileName);
            }
        }

        /// <summary>
        /// Path to the TSV file.
        /// </summary>
        [JsonIgnore]
        public string LocalFileName
        {
            get
            {
                return AppCore.GetLocalResource($"{SourceType}.tsv");
            }
        }

        /// <summary>
        /// Type of this TSV file source.
        /// </summary>
        public string? SourceType { get; set; }

        /// <summary>
        /// URL of this TSV file source.
        /// </summary>
        public Uri? SourceUrl { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Downloads the TSV file from this source.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public async void DownloadAsync()
        {
            if (SourceUrl == null)
            {
                throw new InvalidOperationException("Cannot start the download of a TSV file, this TSV source object does not contain a source URL.");
            }

            using (DownloadService downloadService = new DownloadService(AppCore.GetDownloadConfiguration()))
            {
                downloadService.DownloadFileCompleted += DownloadService_DownloadFileCompleted;
                downloadService.DownloadProgressChanged += DownloadService_DownloadProgressChanged;
                await downloadService.DownloadFileTaskAsync(SourceUrl?.ToString(), LocalFileName);
            }
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Called when download progress of the download service changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadService_DownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgress?.Invoke(sender, new DownloadProgressEventArgs(e.ProgressPercentage, e.ProgressedByteSize, e.TotalBytesToReceive, e.AverageBytesPerSecondSpeed));
        }

        /// <summary>
        /// Called when the download of the download service is completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadService_DownloadFileCompleted(object? sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            DownloadCompleted?.Invoke(sender, new DownloadCompletedEventArgs(e.Error == null, e.Error));
        }

        #endregion

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
