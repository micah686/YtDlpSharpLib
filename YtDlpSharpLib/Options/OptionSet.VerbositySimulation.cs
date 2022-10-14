namespace YtDlpSharpLib.Options
{
    public partial class OptionSet
    {
        private Option<bool> _quiet = new Option<bool>("-q", "--quiet");
        private Option<bool> _noWarnings = new Option<bool>("--no-warnings");
        private Option<bool> _simulate = new Option<bool>("-s", "--simulate");
        private Option<bool> _noSimulate = new Option<bool>("--no-simulate");
        private Option<bool> _ignoreNoFormatsError = new Option<bool>("--ignore-no-formats-error");
        private Option<bool> _noIgnoreNoFormatsError = new Option<bool>("--no-ignore-no-formats-error");
        private Option<bool> _skipDownload = new Option<bool>("--skip-download");
        private Option<string> _print = new Option<string>("-O", "--print");
        private Option<string> _printToFile = new Option<string>("--print-to-file");
        private Option<bool> _dumpJson = new Option<bool>("-j", "--dump-json");
        private Option<bool> _dumpSingleJson = new Option<bool>("-J", "--dump-single-json");
        private Option<bool> _forceWriteArchive = new Option<bool>("--force-write-archive");
        private Option<bool> _newLine = new Option<bool>("--newline");
        private Option<bool> _noProgress = new Option<bool>("--no-progress");
        private Option<bool> _progress = new Option<bool>("--progress");
        private Option<bool> _consoleTitle = new Option<bool>("--console-title");
        private Option<string> _progressTemplate = new Option<string>("--progress-template");
        private Option<bool> _verbose = new Option<bool>("-v", "--verbose");
        private Option<bool> _dumpPages = new Option<bool>("--dump-pages");
        private Option<bool> _writePages = new Option<bool>("--write-pages");
        private Option<bool> _printTraffic = new Option<bool>("--print-traffic");

        /// <summary>
        /// Activate quiet mode. If used with --verbose,
        /// print the log to stderr
        /// </summary>
        public bool Quiet { get => _quiet.Value; set => _quiet.Value = value; }
        /// <summary>
        /// Ignore warnings
        /// </summary>
        public bool NoWarnings { get => _noWarnings.Value; set => _noWarnings.Value = value; }
        /// <summary>
        /// Do not download the video and do not write
        /// anything to disk
        /// </summary>
        public bool Simulate { get => _simulate.Value; set => _simulate.Value = value; }
        /// <summary>
        /// Download the video even if printing/listing
        /// options are used
        /// </summary>
        public bool NoSimulate { get => _noSimulate.Value; set => _noSimulate.Value = value; }
        /// <summary>
        /// Ignore "No video formats" error. Useful for
        /// extracting metadata even if the videos are
        /// not actually available for download
        /// (experimental)
        /// </summary>
        public bool IgnoreNoFormatsError { get => _ignoreNoFormatsError.Value; set => _ignoreNoFormatsError.Value = value;}
        /// <summary>
        /// Throw error when no downloadable video
        /// formats are found (default)
        /// </summary>
        public bool NoIgnoreNoFormatsError { get => _noIgnoreNoFormatsError.Value; set => _noIgnoreNoFormatsError.Value = value;}
        /// <summary>
        /// Do not download the video but write all
        /// related files (Alias: --no-download)
        /// </summary>
        public bool SkipDownload { get => _skipDownload.Value; set => _skipDownload.Value = value; }
        /// <summary>
        /// Field name or output template to print to
        /// screen, optionally prefixed with when to
        /// print it, separated by a ":". Supported
        /// values of "WHEN" are the same as that of
        /// --use-postprocessor, and "video" (default).
        /// Implies --quiet. Implies --simulate unless
        /// --no-simulate or later stages of WHEN are
        /// used. This option can be used multiple times
        /// </summary>
        public string Print { get => _print.Value; set => _print.Value = value; }
        /// <summary>
        /// Append given template to the file. The
        /// values of WHEN and TEMPLATE are same as that
        /// of --print. FILE uses the same syntax as the
        /// output template. This option can be used
        /// multiple times
        /// </summary>
        public string PrintToFile { get => _printToFile.Value; set => _printToFile.Value = value; }
        /// <summary>
        /// Quiet, but print JSON information for each
        /// video. Simulate unless --no-simulate is
        /// used. See "OUTPUT TEMPLATE" for a
        /// description of available keys
        /// </summary>
        public bool DumpJson { get => _dumpJson.Value; set => _dumpJson.Value = value; }
        /// <summary>
        /// Quiet, but print JSON information for each
        /// url or infojson passed. Simulate unless
        /// --no-simulate is used. If the URL refers to
        /// a playlist, the whole playlist information
        /// is dumped in a single line
        /// </summary>
        public bool DumpSingleJson { get => _dumpSingleJson.Value; set => _dumpSingleJson.Value = value;}
        /// <summary>
        /// Force download archive entries to be written
        /// as far as no errors occur, even if -s or
        /// another simulation option is used (Alias:
        /// --force-download-archive)
        /// </summary>
        public bool ForceWriteArchive { get => _forceWriteArchive.Value; set => _forceWriteArchive.Value = value; }
        /// <summary>
        /// Output progress bar as new lines
        /// </summary>
        public bool NewLine { get => _newLine.Value; set => _newLine.Value = value; }
        /// <summary>
        /// Do not print progress bar
        /// </summary>
        public bool NoProgress { get => _noProgress.Value; set => _noProgress.Value = value; }
        /// <summary>
        /// Show progress bar, even if in quiet mode
        /// </summary>
        public bool Progress { get => _progress.Value; set => _progress.Value = value; }
        /// <summary>
        /// Display progress in console titlebar
        /// </summary>
        public bool ConsoleTitle { get => _consoleTitle.Value; set => _consoleTitle.Value = value; }
        /// <summary>
        /// Template for progress outputs, optionally
        /// prefixed with one of "download:" (default),
        /// "download-title:" (the console title),
        /// "postprocess:",  or "postprocess-title:".
        /// The video's fields are accessible under the
        /// "info" key and the progress attributes are
        /// accessible under "progress" key. E.g.
        /// --console-title --progress-template
        /// "download-title:%(info.id)s-%(progress.eta)s"
        /// </summary>
        public string ProgressTemplate { get => _progressTemplate.Value; set => _progressTemplate.Value = value; }
        /// <summary>
        /// Print various debugging information
        /// </summary>
        public bool Verbose { get => _verbose.Value; set => _verbose.Value = value; }
        /// <summary>
        /// Print downloaded pages encoded using base64
        /// to debug problems (very verbose)
        /// </summary>
        public bool DumpPages { get => _dumpPages.Value; set => _dumpPages.Value = value; }
        /// <summary>
        /// Write downloaded intermediary pages to files
        /// in the current directory to debug problems
        /// </summary>
        public bool WritePages { get => _writePages.Value; set => _writePages.Value = value; }
        /// <summary>
        /// Display sent and read HTTP traffic
        /// </summary>
        public bool PrintTraffic { get => _printTraffic.Value; set => _printTraffic.Value = value; }
    }
}
