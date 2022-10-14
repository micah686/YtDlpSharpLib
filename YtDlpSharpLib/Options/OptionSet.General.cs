namespace YtDlpSharpLib.Options
{
    public partial class OptionSet
    {
        private Option<bool> _help = new Option<bool>("-h", "--help");
        private Option<bool> _version = new Option<bool>("--version");
        private Option<bool> _update = new Option<bool>("-U", "--update");
        private Option<bool> _noUpdate = new Option<bool>("--no-update");
        private Option<bool> _ignoreErrors = new Option<bool>("-i", "--ignore-errors");
        private Option<bool> _noAbortOnError = new Option<bool>("--no-abort-on-error");
        private Option<bool> _abortOnError = new Option<bool>("--abort-on-error");
        private Option<bool> _dumpUserAgent = new Option<bool>("--dump-user-agent");
        private Option<bool> _listExtractors = new Option<bool>("--list-extractors");
        private Option<bool> _extractorDescriptions = new Option<bool>("--extractor-descriptions");
        private Option<string> _useExtractors = new Option<string>("--use-extractors");
        private Option<string> _defaultSearch = new Option<string>("--default-search");
        private Option<bool> _ignoreConfig = new Option<bool>("--ignore-config");
        private Option<bool> _noConfigLocations = new Option<bool>("--no-config-locations");
        private Option<string> _configLocations = new Option<string>("--config-locations");
        private Option<bool> _flatPlaylist = new Option<bool>("--flat-playlist");
        private Option<bool> _noFlatPlaylist = new Option<bool>("--no-flat-playlist");
        private Option<bool> _liveFromStart = new Option<bool>("--live-from-start");
        private Option<bool> _noLiveFromStart = new Option<bool>("--no-live-from-start");
        private Option<string> _waitForVideo = new Option<string>("--wait-for-video");
        private Option<bool> _noWaitForVideo = new Option<bool>("--no-wait-for-video");
        private Option<bool> _markWatched = new Option<bool>("--mark-watched");
        private Option<bool> _noMarkWatched = new Option<bool>("--no-mark-watched");
        private Option<bool> _noColors = new Option<bool>("--no-colors");
        private Option<string> _compatOptions = new Option<string>("--compat-options");
        private Option<string> _alias = new Option<string>("--alias");

        /// <summary>
        /// Print this help text and exit
        /// </summary>
        public bool Help { get => _help.Value; set => _help.Value = value; }
        /// <summary>
        /// Print program version and exit
        /// </summary>
        public bool Version { get => _version.Value; set => _version.Value = value; }
        /// <summary>
        /// Update this program to the latest version
        /// </summary>
        public bool Update { get => _update.Value; set => _update.Value = value; }
        /// <summary>
        /// Do not check for updates (default)
        /// </summary>
        public bool NoUpdate { get => _noUpdate.Value; set => _noUpdate.Value = value; }
        /// <summary>
        /// Ignore download and postprocessing errors.
        /// The download will be considered successful
        /// even if the postprocessing fails
        /// </summary>
        public bool IgnoreErrors { get => _ignoreErrors.Value; set => _ignoreErrors.Value = value; }
        /// <summary>
        /// Continue with next video on download errors;
        /// e.g. to skip unavailable videos in a
        /// playlist (default)
        /// </summary>
        public bool NoAbortOnError { get => _noAbortOnError.Value; set => _noAbortOnError.Value = value; }
        /// <summary>
        /// Abort downloading of further videos if an
        /// error occurs (Alias: --no-ignore-errors)
        /// </summary>
        public bool AbortOnError { get => _abortOnError.Value; set => _abortOnError.Value = value; }
        /// <summary>
        /// Display the current user-agent and exit
        /// </summary>
        public bool DumpUserAgent { get => _dumpUserAgent.Value; set => _dumpUserAgent.Value = value; }
        /// <summary>
        /// List all supported extractors and exit
        /// </summary>
        public bool ListExtractors { get => _listExtractors.Value; set => _listExtractors.Value = value; }
        /// <summary>
        /// Output descriptions of all supported
        /// extractors and exit
        /// </summary>
        public bool ExtractorDescriptions { get => _extractorDescriptions.Value; set => _extractorDescriptions.Value = value; }
        /// <summary>
        /// Extractor names to use separated by commas.
        /// You can also use regexes, "all", "default"
        /// and "end" (end URL matching); e.g. --ies
        /// "holodex.*,end,youtube". Prefix the name
        /// with a "-" to exclude it, e.g. --ies
        /// default,-generic. Use --list-extractors for
        /// a list of extractor names. (Alias: --ies)
        /// </summary>
        public string UseExtractors { get => _useExtractors.Value; set => _useExtractors.Value = value; }
        /// <summary>
        /// Use this prefix for unqualified URLs. E.g.
        /// "gvsearch2:python" downloads two videos from
        /// google videos for the search term "python".
        /// Use the value "auto" to let yt-dlp guess
        /// ("auto_warning" to emit a warning when
        /// guessing). "error" just throws an error. The
        /// default value "fixup_error" repairs broken
        /// URLs, but emits an error if this is not
        /// possible instead of searching
        /// </summary>
        public string DefaultSearch { get => _defaultSearch.Value; set => _defaultSearch.Value = value; }
        /// <summary>
        /// Don't load any more configuration files
        /// except those given by --config-locations.
        /// For backward compatibility, if this option
        /// is found inside the system configuration
        /// file, the user configuration is not loaded.
        /// (Alias: --no-config)
        /// </summary>
        public bool IgnoreConfig { get => _ignoreConfig.Value; set => _ignoreConfig.Value = value; }
        /// <summary>
        /// Do not load any custom configuration files
        /// (default). When given inside a configuration
        /// defined in the current file
        /// </summary>
        public bool NoConfigLocations { get => _noConfigLocations.Value; set => _noConfigLocations.Value = value; }
        /// <summary>
        /// Location of the main configuration file;
        /// either the path to the config or its
        /// containing directory ("-" for stdin). Can be
        /// used multiple times and inside other
        /// configuration files
        /// </summary>
        public string ConfigLocations { get => _configLocations.Value; set => _configLocations.Value = value; }
        /// <summary>
        /// Do not extract the videos of a playlist,
        /// only list them
        /// </summary>
        public bool FlatPlaylist { get => _flatPlaylist.Value; set => _flatPlaylist.Value = value; }
        /// <summary>
        /// Extract the videos of a playlist
        /// </summary>
        public bool NoFlatPlaylist { get => _noFlatPlaylist.Value; set => _noFlatPlaylist.Value = value; }
        /// <summary>
        /// Download livestreams from the start.
        /// Currently only supported for YouTube
        /// (Experimental)
        /// </summary>
        public bool LiveFromStart { get => _liveFromStart.Value; set => _liveFromStart.Value = value; }
        /// <summary>
        /// Download livestreams from the current time
        /// (default)
        /// </summary>
        public bool NoLiveFromStart { get => _noLiveFromStart.Value; set => _noLiveFromStart.Value = value; }
        /// <summary>
        /// Wait for scheduled streams to become
        /// available. Pass the minimum number of
        /// seconds (or range) to wait between retries
        /// </summary>
        public string WaitForVideo { get => _waitForVideo.Value; set => _waitForVideo.Value = value; }
        /// <summary>
        /// Do not wait for scheduled streams (default)
        /// </summary>
        public bool NoWaitForVideo { get => _noWaitForVideo.Value; set => _noWaitForVideo.Value = value; }
        /// <summary>
        /// Mark videos watched (even with --simulate)
        /// </summary>
        public bool MarkWatched { get => _markWatched.Value; set => _markWatched.Value = value; }
        /// <summary>
        /// Do not mark videos watched (default)
        /// </summary>
        public bool NoMarkWatched { get => _noMarkWatched.Value; set => _noMarkWatched.Value = value; }
        /// <summary>
        /// Do not emit color codes in output (Alias:
        /// --no-colours)
        /// </summary>
        public bool NoColors { get => _noColors.Value; set => _noColors.Value = value; }
        /// <summary>
        /// Options that can help keep compatibility
        /// with yt-dlp
        /// configurations by reverting some of the
        /// changes made in yt-dlp. See "Differences in
        /// default behavior" for details
        /// </summary>
        public string CompatOptions { get => _compatOptions.Value; set => _compatOptions.Value = value; }
        /// <summary>
        /// Create aliases for an option string. Unless
        /// an alias starts with a dash "-", it is
        /// prefixed with "--". Arguments are parsed
        /// according to the Python string formatting
        /// mini-language. E.g. --alias get-audio,-X
        /// "-S=aext:{0},abr -x --audio-format {0}"
        /// creates options "--get-audio" and "-X" that
        /// takes an argument (ARG0) and expands to
        /// "-S=aext:ARG0,abr -x --audio-format ARG0".
        /// All defined aliases are listed in the --help
        /// output. Alias options can trigger more
        /// aliases; so be careful to avoid defining
        /// recursive options. As a safety measure, each
        /// alias may be triggered a maximum of 100
        /// times. This option can be used multiple times
        /// </summary>
        public string Alias { get => _alias.Value; set => _alias.Value = value; }        
    }
}
