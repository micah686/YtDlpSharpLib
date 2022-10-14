namespace YtDlpSharpLib.Options
{
    public partial class OptionSet
    {
        private Option<string> _encoding = new Option<string>("--encoding");
        private Option<bool> _legacyServer = new Option<bool>("--legacy-server");
        private Option<bool> _noCheckCertificates = new Option<bool>("--no-check-certificates");
        private Option<bool> _preferInsecure = new Option<bool>("--prefer-insecure");
        private Option<string> _addHeader = new Option<string>("--add-header");
        private Option<bool> _bidiWorkaround = new Option<bool>("--bidi-workaround");
        private Option<int> _sleepRequests = new Option<int>("--sleep-requests");
        private Option<int> _sleepInterval = new Option<int>("--sleep-interval");
        private Option<int> _maxSleepInterval = new Option<int>("--max-sleep-interval");
        private Option<int> _sleepSubtitles = new Option<int>("--sleep-subtitles");

        /// <summary>
        /// Force the specified encoding (experimental)
        /// </summary>
        public string Encoding { get => _encoding.Value; set => _encoding.Value = value; }
        /// <summary>
        /// Explicitly allow HTTPS connection to servers
        /// that do not support RFC 5746 secure
        /// renegotiation
        /// </summary>
        public bool LegacyServer { get => _legacyServer.Value; set => _legacyServer.Value = value; }
        /// <summary>
        /// Suppress HTTPS certificate validation
        /// </summary>
        public bool NoCheckCertificates { get => _noCheckCertificates.Value; set => _noCheckCertificates.Value = value; }
        /// <summary>
        /// Use an unencrypted connection to retrieve
        /// information about the video (Currently
        /// supported only for YouTube)
        /// </summary>
        public bool PreferInsecure { get => _preferInsecure.Value; set => _preferInsecure.Value = value; }
        /// <summary>
        /// Specify a custom HTTP header and its value,
        /// separated by a colon ":". You can use this
        /// option multiple times
        /// </summary>
        public string AddHeader { get => _addHeader.Value; set => _addHeader.Value = value; }
        /// <summary>
        /// Work around terminals that lack
        /// bidirectional text support. Requires bidiv
        /// or fribidi executable in PATH
        /// </summary>
        public bool BidiWorkaround { get => _bidiWorkaround.Value; set => _bidiWorkaround.Value = value; }
        /// <summary>
        /// Number of seconds to sleep between requests
        /// during data extraction
        /// </summary>
        public int SleepRequests { get => _sleepRequests.Value; set => _sleepRequests.Value = value; }
        /// <summary>
        /// Number of seconds to sleep before each
        /// download. This is the minimum time to sleep
        /// when used along with --max-sleep-interval
        /// (Alias: --min-sleep-interval)
        /// </summary>
        public int SleepInterval { get => _sleepInterval.Value; set => _sleepInterval.Value = value; }
        /// <summary>
        /// Maximum number of seconds to sleep. Can only
        /// be used along with --min-sleep-interval
        /// </summary>
        public int MaxSleepInterval { get => _maxSleepInterval.Value; set => _maxSleepInterval.Value = value; }
        /// <summary>
        /// Number of seconds to sleep before each
        /// subtitle download
        /// </summary>
        public int SleepSubtitles { get => _sleepSubtitles.Value; set => _sleepSubtitles.Value = value; }
    }
}
