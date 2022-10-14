using System;

namespace YtDlpSharpLib.Options
{
    public partial class OptionSet
    {
        private Option<string> _playlistItems = new Option<string>("I","--playlist-items");
        private Option<string> _minFilesize = new Option<string>("--min-filesize");
        private Option<string> _maxFileSize = new Option<string>("--max-filesize");
        private Option<DateTime> _date = new Option<DateTime>("--date");
        private Option<DateTime> _dateBefore = new Option<DateTime>("--date-before");
        private Option<DateTime> _dateAfter = new Option<DateTime>("--date-after");
        private Option<string> _matchFilters = new Option<string>("--match-filters");
        private Option<bool> _noMatchFilters = new Option<bool>("--no-match-filter");
        private Option<bool> _noPlaylist = new Option<bool>("--no-playlist");
        private Option<bool> _yesPlaylist = new Option<bool>("--yes-playlist");
        private Option<byte> _ageLimit = new Option<byte>("--age-limit");
        private Option<string> _downloadArchive = new Option<string>("--download-archive");
        private Option<bool> _noDownloadArchive = new Option<bool>("--no-download-archive");
        private Option<int> _maxDownloads = new Option<int>("--max-downloads");
        private Option<bool> _breakOnExisting = new Option<bool>("--break-on-existing");
        private Option<bool> _breakOnReject = new Option<bool>("--break-on-reject");
        private Option<bool> _breakPerInput = new Option<bool>("--break-per-input");
        private Option<bool> _noBreakPerInput = new Option<bool>("--no-break-per-input");
        private Option<int> _skipPlaylistAfterErrors = new Option<int>("--skip-playlist-after-errors");

        /// <summary>
        /// Comma separated playlist_index of the videos
        /// to download. You can specify a range using
        /// "[START]:[STOP][:STEP]". For backward
        /// compatibility, START-STOP is also supported.
        /// Use negative indices to count from the right
        /// and negative STEP to download in reverse
        /// order. E.g. "-I 1:3,7,-5::2" used on a
        /// playlist of size 15 will download the videos
        /// at index 1,2,3,7,11,13,15
        /// </summary>
        public string PlaylistItems { get => _playlistItems.Value; set => _playlistItems.Value = value; }
        /// <summary>
        /// Do not download any videos smaller than
        /// SIZE, e.g. 50k or 44.6M
        /// </summary>
        public string MinFileSize { get => _minFilesize.Value; set => _minFilesize.Value = value; }
        /// <summary>
        /// Do not download any videos larger than SIZE,
        /// e.g. 50k or 44.6M
        /// </summary>
        public string MaxFileSize { get => _maxFileSize.Value; set => _maxFileSize.Value = value; }
        /// <summary>
        /// Download only videos uploaded on this date.
        /// The date can be "YYYYMMDD" or in the format 
        /// [now|today|yesterday][-N[day|week|month|year]].
        /// E.g. --date today-2weeks
        /// </summary>
        public DateTime Date { get => _date.Value; set => _date.Value = value; }
        /// <summary>
        /// Download only videos uploaded on or before
        /// this date. The date formats accepted is the
        /// same as --date
        /// </summary>
        public DateTime DateBefore { get => _dateBefore.Value; set => _dateBefore.Value = value; }
        /// <summary>
        /// Download only videos uploaded on or after
        /// this date. The date formats accepted is the
        /// same as --date
        /// </summary>
        public DateTime DateAfter { get => _dateAfter.Value; set => _dateAfter.Value = value; }
        /// <summary>
        /// Generic video filter. Any "OUTPUT TEMPLATE"
        /// field can be compared with a number or a
        /// string using the operators defined in
        /// "Filtering Formats". You can also simply
        /// specify a field to match if the field is
        /// present, use "!field" to check if the field
        /// is not present, and "&amp;" to check multiple
        /// conditions. Use a "\" to escape "&amp;" or
        /// quotes if needed. If used multiple times,
        /// the filter matches if atleast one of the
        /// conditions are met. E.g. --match-filter
        /// !is_live --match-filter "like_count>?100 &amp;
        /// description~='(?i)\bcats \&amp; dogs\b'" matches
        /// only videos that are not live OR those that
        /// have a like count more than 100 (or the like
        /// field is not available) and also has a
        /// description that contains the phrase "cats &amp;
        /// dogs" (caseless). Use "--match-filter -" to
        /// interactively ask whether to download each
        /// video
        /// </summary>
        public string MatchFilters { get => _matchFilters.Value; set => _matchFilters.Value = value; }
        /// <summary>
        /// Do not use generic video filter (default)
        /// </summary>
        public bool NoMatchFilters { get => _noMatchFilters.Value; set => _noMatchFilters.Value = value; }
        /// <summary>
        /// Download only the video, if the URL refers
        /// to a video and a playlist
        /// </summary>
        public bool NoPlaylist { get => _noPlaylist.Value; set => _noPlaylist.Value = value; }
        /// <summary>
        /// Download the playlist, if the URL refers to
        /// a video and a playlist
        /// </summary>
        public bool YesPlaylist { get => _yesPlaylist.Value; set => _yesPlaylist.Value = value; }
        /// <summary>
        /// Download only videos suitable for the given
        /// age
        /// </summary>
        public byte AgeLimit { get => _ageLimit.Value; set => _ageLimit.Value = value; }
        /// <summary>
        /// Download only videos not listed in the
        /// archive file. Record the IDs of all
        /// downloaded videos in it
        /// </summary>
        public string DownloadArchive { get => _downloadArchive.Value; set => _downloadArchive.Value = value; }
        /// <summary>
        /// Do not use archive file (default)
        /// </summary>
        public bool NoDownloadArchive { get => _noDownloadArchive.Value; set => _noDownloadArchive.Value = value; }
        /// <summary>
        /// Abort after downloading NUMBER files
        /// </summary>
        public int MaxDownloads { get => _maxDownloads.Value; set => _maxDownloads.Value = value; }
        /// <summary>
        /// Stop the download process when encountering
        /// a file that is in the archive
        /// </summary>
        public bool BreakOnExisting { get => _breakOnExisting.Value; set => _breakOnExisting.Value = value; }
        /// <summary>
        /// Stop the download process when encountering
        /// a file that has been filtered out
        /// </summary>
        public bool BreakOnReject { get => _breakOnReject.Value; set => _breakOnReject.Value = value; }
        /// <summary>
        /// --break-on-existing, --break-on-reject,
        /// --max-downloads, and autonumber resets per
        /// input URL
        /// </summary>
        public bool BreakPerInput { get => _breakPerInput.Value; set => _breakPerInput.Value = value; }
        /// <summary>
        /// --break-on-existing and similar options
        /// terminates the entire download queue
        /// </summary>
        public bool NoBreakPerInput { get => _noBreakPerInput.Value; set => _noBreakPerInput.Value = value; }
        /// <summary>
        /// Number of allowed failures until the rest of
        /// the playlist is skipped
        /// </summary>
        public int SkipPlaylistAfterErrors { get => _skipPlaylistAfterErrors.Value; set => _skipPlaylistAfterErrors.Value = value; }
    }
}
