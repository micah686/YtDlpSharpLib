namespace YtDlpSharpLib.Options
{
    public partial class OptionSet
    {
        private Option<bool> _writeSubs = new Option<bool>("--write-subs");
        private Option<bool> _noWriteSubs = new Option<bool>("--no-write-subs");
        private Option<bool> _writeAutoSubs = new Option<bool>("--write-auto-subs");
        private Option<bool> _noWriteAutoSubs = new Option<bool>("--no-write-auto-subs");
        private Option<bool> _listSubs = new Option<bool>("--list-subs");
        private Option<string> _subsFormat = new Option<string>("--sub-format");
        private Option<string> _subsLangs = new Option<string>("--sub-langs");

        /// <summary>
        /// Write subtitle file
        /// </summary>
        public bool WriteSubs { get => _writeSubs.Value; set => _writeSubs.Value = value; }
        /// <summary>
        /// Do not write subtitle file (default)
        /// </summary>
        public bool NoWriteSubs { get => _noWriteSubs.Value; set => _noWriteSubs.Value = value; }
        /// <summary>
        /// Write automatically generated subtitle file
        /// (Alias: --write-automatic-subs)
        /// </summary>
        public bool WriteAutoSubs { get => _writeAutoSubs.Value; set => _writeAutoSubs.Value = value;}
        /// <summary>
        /// Do not write auto-generated subtitles
        /// (default) (Alias: --no-write-automatic-subs)
        /// </summary>
        public bool NoWriteAutoSubs { get => _noWriteAutoSubs.Value; set => _noWriteAutoSubs.Value = value;}
        /// <summary>
        /// List available subtitles of each video.
        /// Simulate unless --no-simulate is used
        /// </summary>
        public bool ListSubs { get => _listSubs.Value;set => _listSubs.Value = value; }
        /// <summary>
        /// Subtitle format; accepts formats preference,
        /// e.g. "srt" or "ass/srt/best"
        /// </summary>
        public string SubsFormat { get => _subsFormat.Value; set => _subsFormat.Value = value; }
        /// <summary>
        /// Languages of the subtitles to download (can
        /// be regex) or "all" separated by commas, e.g.
        /// --sub-langs "en.*,ja". You can prefix the
        /// language code with a "-" to exclude it from
        /// the requested languages, e.g. --sub-langs
        /// all,-live_chat. Use --list-subs for a list
        /// of available language tags
        /// </summary>
        public string SubsLangs { get => _subsLangs.Value; set => _subsLangs.Value = value; }
    }
}
