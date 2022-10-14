namespace YtDlpSharpLib.Options
{
    public partial class OptionSet
    {
        private Option<string> _format = new Option<string>("-f", "--format");
        private Option<string> _formatSort = new Option<string>("-S","--format-sort");
        private Option<bool> _formatSortForce = new Option<bool>("--format-sort-force");
        private Option<bool> _noFormatSortForce = new Option<bool>("--format-sort-force");
        private Option<bool> _videoMultiStreams = new Option<bool>("--video-multistreams");
        private Option<bool> _noVideoMultiStreams = new Option<bool>("--no-video-multistreams");
        private Option<bool> _audioMultiStreams = new Option<bool>("--audio-multistreams");
        private Option<bool> _noAudioMultiStreams = new Option<bool>("--no-audio-multistreams");
        private Option<bool> _preferFreeFormats = new Option<bool>("--prefer-free-formats");
        private Option<bool> _noPreferFreeFormats = new Option<bool>("--no-prefer-free-formats");
        private Option<bool> _checkFormats = new Option<bool>("--check-formats");
        private Option<bool> _checkAllFormats = new Option<bool>("--check-all-formats");
        private Option<bool> _noCheckFormats = new Option<bool>("--no-check-formats");
        private Option<bool> _listFormats = new Option<bool>("-F","--list-formats");
        private Option<DownloadMergeFormat> _mergeOutputFormat = new Option<DownloadMergeFormat>("--merge-output-format");

        /// <summary>
        /// Video format code, see "FORMAT SELECTION"
        /// for more details
        /// </summary>
        public string Format { get => _format.Value; set => _format.Value = value; }
        /// <summary>
        /// Sort the formats by the fields given, see
        /// "Sorting Formats" for more details
        /// </summary>
        public string FormatSort { get => _formatSort.Value; set => _formatSort.Value = value; }
        /// <summary>
        /// Force user specified sort order to have
        /// precedence over all fields, see "Sorting
        /// Formats" for more details (Alias: --S-force)
        /// </summary>
        public bool FormatSortForce { get => _formatSortForce.Value; set => _formatSortForce.Value = value; }
        /// <summary>
        /// Some fields have precedence over the user
        /// specified sort order (default)
        /// </summary>
        public bool NoFormatSortForce { get => _formatSortForce.Value; set => _formatSortForce.Value = value; }
        /// <summary>
        /// Allow multiple video streams to be merged
        /// into a single file
        /// </summary>
        public bool VideoMultiStreams { get => _videoMultiStreams.Value; set => _videoMultiStreams.Value = value; }
        /// <summary>
        /// Only one video stream is downloaded for each
        /// output file (default)
        /// </summary>
        public bool NoVideoMultiStreams { get => _noVideoMultiStreams.Value; set => _noVideoMultiStreams.Value = value; }
        /// <summary>
        /// Allow multiple audio streams to be merged
        /// into a single file
        /// </summary>
        public bool AudioMultiStreams { get => _audioMultiStreams.Value; set => _audioMultiStreams.Value = value; }
        /// <summary>
        /// Only one audio stream is downloaded for each
        /// output file (default)
        /// </summary>
        public bool NoAudioMultiStreams { get => _noAudioMultiStreams.Value; set => _noAudioMultiStreams.Value = value; }
        /// <summary>
        /// Prefer video formats with free containers
        /// over non-free ones of same quality. Use with
        /// "-S ext" to strictly prefer free containers
        /// irrespective of quality
        /// </summary>
        public bool PreferFreeFormats { get => _preferFreeFormats.Value; set => _preferFreeFormats.Value = value; }
        /// <summary>
        /// Don't give any special preference to free
        /// containers (default)
        /// </summary>
        public bool NoPreferFreeFormats { get => _noPreferFreeFormats.Value; set => _noPreferFreeFormats.Value = value; }
        /// <summary>
        /// Make sure formats are selected only from
        /// those that are actually downloadable
        /// </summary>
        public bool CheckFormats { get => _checkFormats.Value; set => _checkFormats.Value = value; }
        /// <summary>
        /// Check all formats for whether they are
        /// actually downloadable
        /// </summary>
        public bool CheckAllFormats { get => _checkAllFormats.Value; set => _checkAllFormats.Value = value; }
        /// <summary>
        /// Do not check that the formats are actually
        /// downloadable
        /// </summary>
        public bool NoCheckFormats { get => _noCheckFormats.Value; set => _noCheckFormats.Value = value; }
        /// <summary>
        /// List available formats of each video.
        /// Simulate unless --no-simulate is used
        /// </summary>
        public bool ListFormats { get => _listFormats.Value; set => _listFormats.Value = value; }
        /// <summary>
        /// Containers that may be used when merging
        /// formats, separated by "/", e.g. "mp4/mkv".
        /// Ignored if no merge is required. (currently
        /// supported: avi, flv, mkv, mov, mp4, webm)
        /// </summary>
        public DownloadMergeFormat MergeOutputFormat { get => _mergeOutputFormat.Value; set => _mergeOutputFormat.Value = value; }
    }
}
