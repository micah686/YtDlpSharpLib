namespace YtDlpSharpLib.Options
{
    public partial class OptionSet
    {
        private Option<bool> _extractAudio = new Option<bool>("-x", "--extract-audio");
        private Option<AudioConversionFormat> _audioFormat = new Option<AudioConversionFormat>("--audio-format");
        private Option<string> _audioQuality = new Option<string>("--audio-quality");
        private Option<VideoRecodeFormat> _remuxVideo = new Option<VideoRecodeFormat>("--remux-video");
        private Option<VideoRecodeFormat> _recodeVideo = new Option<VideoRecodeFormat>("--recode-video");
        private Option<string> _postProcessorArgs = new Option<string>("--postprocessor-args");
        private Option<bool> _keepVideo = new Option<bool>("-k","--keep-video");
        private Option<bool> _noKeepVideo = new Option<bool>("--no-keep-video");
        private Option<bool> _postOverwrites = new Option<bool>("--post-overwrites");
        private Option<bool> _noPostOverwrites = new Option<bool>("--no-post-overwrites");
        private Option<bool> _embedSubs = new Option<bool>("--embed-subs");
        private Option<bool> _noEmbedSubs = new Option<bool>("--no-embed-subs");
        private Option<bool> _embedThumbnail = new Option<bool>("--embed-thumbnail");
        private Option<bool> _noEmbedThumbnail = new Option<bool>("--no-embed-thumbnail");
        private Option<bool> _embedMetadata = new Option<bool>("--embed-metadata");
        private Option<bool> _noEmbedMetadata = new Option<bool>("--no-embed-metadata");
        private Option<bool> _embedChapters = new Option<bool>("--embed-chapters");
        private Option<bool> _noEmbedChapters = new Option<bool>("--no-embed-chapters");
        private Option<bool> _embedInfoJson = new Option<bool>("--embed-info-json");
        private Option<bool> _noEmbedInfoJson = new Option<bool>("--no-embed-info-json");
        private Option<string> _parseMetadata = new Option<string>("--parse-metadata");
        private Option<string> _replaceInMetadata = new Option<string>("--replace-in-metadata");
        private Option<bool> _xattrs = new Option<bool>("--xattrs");
        private Option<bool> _concatPlaylist = new Option<bool>("--concat-playlist");
        private Option<string> _fixupPolicy = new Option<string>("--fixup-policy");
        private Option<string> _ffmpegLocation = new Option<string>("--ffmpeg-location");
        private Option<string> _exec = new Option<string>("--exec");
        private Option<bool> _noExec = new Option<bool>("--no-exec");
        private Option<string> _convertSubs = new Option<string>("--convert-subs");
        private Option<string> _convertThumbnails = new Option<string>("--convert-thumbnails");
        private Option<bool> _splitChapters = new Option<bool>("--split-chapters");
        private Option<bool> _noSplitChapters = new Option<bool>("--no-split-chapters");
        private Option<string> _removeChapters = new Option<string>("--remove-chapters");
        private Option<bool> _noRemoveChapters = new Option<bool>("--no-remove-chapters");
        private Option<bool> _forceKeyframesAtCuts = new Option<bool>("--force-keyframes-at-cuts");
        private Option<bool> _noForceKeyframesAtCuts = new Option<bool>("--no-force-keyframes-at-cuts");
        private Option<string> _usePostProcessor = new Option<string>("--use-postprocessor");

        /// <summary>
        /// Convert video files to audio-only files
        /// (requires ffmpeg and ffprobe)
        /// </summary>
        public bool ExtractAudio { get => _extractAudio.Value; set => _extractAudio.Value = value; }
        /// <summary>
        /// Format to convert the audio to when -x is
        /// used. (currently supported: best (default),
        /// aac, alac, flac, m4a, mp3, opus, vorbis,
        /// wav). You can specify multiple rules using
        /// similar syntax as --remux-video
        /// </summary>
        public AudioConversionFormat AudioFormat { get => _audioFormat.Value; set => _audioFormat.Value = value; }
        /// <summary>
        /// Specify ffmpeg audio quality to use when
        /// converting the audio with -x. Insert a value
        /// between 0 (best) and 10 (worst) for VBR or a
        /// specific bitrate like 128K (default 5)
        /// </summary>
        public string AudioQuality { get => _audioQuality.Value; set => _audioQuality.Value = value; }
        /// <summary>
        /// Remux the video into another container if
        /// necessary (currently supported: avi, flv,
        /// mkv, mov, mp4, webm, aac, aiff, alac, flac,
        /// m4a, mka, mp3, ogg, opus, vorbis, wav). If
        /// target container does not support the
        /// video/audio codec, remuxing will fail. You
        /// can specify multiple rules; e.g.
        /// "aac>m4a/mov>mp4/mkv" will remux aac to m4a,
        /// mov to mp4 and anything else to mkv
        /// </summary>
        public VideoRecodeFormat RemuxVideo { get => _recodeVideo.Value;  set => _recodeVideo.Value = value; }
        /// <summary>
        /// Re-encode the video into another format if
        /// necessary. The syntax and supported formats
        /// are the same as --remux-video
        /// </summary>
        public VideoRecodeFormat RecodeVideo { get => _recodeVideo.Value; set => _recodeVideo.Value = value; }
        /// <summary>
        /// Give these arguments to the postprocessors.
        /// Specify the postprocessor/executable name
        /// and the arguments separated by a colon ":"
        /// to give the argument to the specified
        /// postprocessor/executable. Supported PP are:
        /// Merger, ModifyChapters, SplitChapters,
        /// ExtractAudio, VideoRemuxer, VideoConvertor,
        /// Metadata, EmbedSubtitle, EmbedThumbnail,
        /// SubtitlesConvertor, ThumbnailsConvertor,
        /// FixupStretched, FixupM4a, FixupM3u8,
        /// FixupTimestamp and FixupDuration. The
        /// supported executables are: AtomicParsley,
        /// FFmpeg and FFprobe. You can also specify
        /// "PP+EXE:ARGS" to give the arguments to the
        /// specified executable only when being used by
        /// the specified postprocessor. Additionally,
        /// for ffmpeg/ffprobe, "_i"/"_o" can be
        /// appended to the prefix optionally followed
        /// by a number to pass the argument before the
        /// specified input/output file, e.g. --ppa
        /// "Merger+ffmpeg_i1:-v quiet". You can use
        /// this option multiple times to give different
        /// arguments to different postprocessors.
        /// (Alias: --ppa)
        /// </summary>
        public string PostProcessorArgs { get => _postProcessorArgs.Value; set => _postProcessorArgs.Value = value; }
        /// <summary>
        /// Keep the intermediate video file on disk
        /// after post-processing
        /// </summary>
        public bool KeepVideo { get => _keepVideo.Value; set => _keepVideo.Value = value; }
        /// <summary>
        /// Delete the intermediate video file after
        /// post-processing (default)
        /// </summary>
        public bool NoKeepVideo { get => _noKeepVideo.Value; set => _noKeepVideo.Value = value; }
        /// <summary>
        /// Overwrite post-processed files (default)
        /// </summary>
        public bool PostOverwrites { get => _postOverwrites.Value; set => _postOverwrites.Value = value; }
        /// <summary>
        /// Do not overwrite post-processed files
        /// </summary>
        public bool NoPostOverwrites { get => _noPostOverwrites.Value; set => _noPostOverwrites.Value = value; }
        /// <summary>
        /// Embed subtitles in the video (only for mp4,
        /// webm and mkv videos)
        /// </summary>
        public bool EmbedSubs { get => _embedSubs.Value; set => _embedSubs.Value = value; }
        /// <summary>
        /// Do not embed subtitles (default)
        /// </summary>
        public bool NoEmbedSubs { get => _noEmbedSubs.Value; set => _noEmbedSubs.Value = value; }
        /// <summary>
        /// Embed thumbnail in the video as cover art
        /// </summary>
        public bool EmbedThumbnail { get => _embedThumbnail.Value; set => _embedThumbnail.Value = value; }
        /// <summary>
        /// Do not embed thumbnail (default)
        /// </summary>
        public bool NoEmbedThumbnail { get => _noEmbedThumbnail.Value; set => _noEmbedThumbnail.Value = value; }
        /// <summary>
        /// Embed metadata to the video file. Also
        /// embeds chapters/infojson if present unless
        /// --no-embed-chapters/--no-embed-info-json are
        /// used (Alias: --add-metadata)
        /// </summary>
        public bool EmbedMetadata { get => _embedMetadata.Value; set => _embedMetadata.Value = value; }
        /// <summary>
        /// Do not add metadata to file (default)
        /// (Alias: --no-add-metadata)
        /// </summary>
        public bool NoEmbedMetadata { get => _noEmbedMetadata.Value; set => _noEmbedMetadata.Value = value; }
        /// <summary>
        /// Add chapter markers to the video file
        /// (Alias: --add-chapters)
        /// </summary>
        public bool EmbedChapters { get => _embedChapters.Value; set => _embedChapters.Value = value; }
        /// <summary>
        /// Do not add chapter markers (default) (Alias:
        /// --no-add-chapters)
        /// </summary>
        public bool NoEmbedChapters { get => _noEmbedChapters.Value; set => _noEmbedChapters.Value = value; }
        /// <summary>
        /// Embed the infojson as an attachment to
        /// mkv/mka video files
        /// </summary>
        public bool EmbedInfoJson { get => _embedInfoJson.Value; set => _embedInfoJson.Value = value; }
        /// <summary>
        /// Do not embed the infojson as an attachment
        /// to the video file
        /// </summary>
        public bool NoEmbedInfoJson { get => _noEmbedInfoJson.Value; set => _noEmbedInfoJson.Value = value; }
        /// <summary>
        /// Parse additional metadata like title/artist
        /// from other fields; see "MODIFYING METADATA"
        /// for details
        /// </summary>
        public string ParseMetadata { get => _parseMetadata.Value; set => _parseMetadata.Value = value; }
        /// <summary>
        /// Replace text in a metadata field using the
        /// given regex. This option can be used
        /// multiple times
        /// </summary>
        public string ReplaceInMetadata { get => _replaceInMetadata.Value; set => _replaceInMetadata.Value = value; }
        /// <summary>
        /// Write metadata to the video file's xattrs
        /// (using dublin core and xdg standards)
        /// </summary>
        public bool Xattrs { get => _xattrs.Value; set => _xattrs.Value = value; }
        /// <summary>
        /// Concatenate videos in a playlist. One of
        /// "never", "always", or "multi_video"
        /// (default; only when the videos form a single
        /// show). All the video files must have same
        /// codecs and number of streams to be
        /// concatable. The "pl_video:" prefix can be
        /// used with "--paths" and "--output" to set
        /// the output filename for the concatenated
        /// files. See "OUTPUT TEMPLATE" for details
        /// </summary>
        public bool ConcatPlaylists { get => _concatPlaylist.Value; set => _concatPlaylist.Value = value; }
        /// <summary>
        /// Automatically correct known faults of the
        /// file. One of never (do nothing), warn (only
        /// emit a warning), detect_or_warn (the
        /// default; fix file if we can, warn
        /// otherwise), force (try fixing even if file
        /// already exists)
        /// </summary>
        public string FixupPolicy { get => _fixupPolicy.Value; set => _fixupPolicy.Value = value; }
        /// <summary>
        /// Location of the ffmpeg binary; either the
        /// path to the binary or its containing directory
        /// </summary>
        public string FfmpegLocation { get => _ffmpegLocation.Value; set => _ffmpegLocation.Value = value; }
        /// <summary>
        /// Execute a command, optionally prefixed with
        /// when to execute it (after_move if
        /// unspecified), separated by a ":". Supported
        /// values of "WHEN" are the same as that of
        /// --use-postprocessor. Same syntax as the
        /// output template can be used to pass any
        /// field as arguments to the command. After
        /// download, an additional field "filepath"
        /// that contains the final path of the
        /// downloaded file is also available, and if no
        /// fields are passed, %(filepath)q is appended
        /// to the end of the command. This option can
        /// be used multiple times
        /// </summary>
        public string Exec { get => _exec.Value; set => _exec.Value = value; }
        /// <summary>
        /// Remove any previously defined --exec
        /// </summary>
        public bool NoExec { get => _noExec.Value; set => _noExec.Value = value; }
        /// <summary>
        /// Convert the subtitles to another format
        /// (currently supported: ass, lrc, srt, vtt)
        /// (Alias: --convert-subtitles)
        /// </summary>
        public string ConvertSubs { get => _convertSubs.Value; set => _convertSubs.Value = value; }
        /// <summary>
        /// Convert the thumbnails to another format
        /// (currently supported: jpg, png, webp). You
        /// can specify multiple rules using similar
        /// syntax as --remux-video
        /// </summary>
        public string ConvertThumbnails { get => _convertSubs.Value; set => _convertSubs.Value = value; }
        /// <summary>
        /// Split video into multiple files based on
        /// internal chapters. The "chapter:" prefix can
        /// be used with "--paths" and "--output" to set
        /// the output filename for the split files. See
        /// "OUTPUT TEMPLATE" for details
        /// </summary>
        public bool SplitChapters { get => _splitChapters.Value; set => _splitChapters.Value = value; }
        /// <summary>
        /// Do not split video based on chapters (default)
        /// </summary>
        public bool NoSplitChapters { get => _noSplitChapters.Value; set => _noSplitChapters.Value = value; }
        /// <summary>
        /// Remove chapters whose title matches the
        /// given regular expression. The syntax is the
        /// same as --download-sections. This option can
        /// be used multiple times
        /// </summary>
        public string RemoveChapters { get => _removeChapters.Value; set => _removeChapters.Value = value; }
        /// <summary>
        /// Do not remove any chapters from the file
        /// (default)
        /// </summary>
        public bool NoRemoveChapters { get => _noRemoveChapters.Value; set => _noRemoveChapters.Value = value; }
        /// <summary>
        /// Force keyframes at cuts when
        /// downloading/splitting/removing sections.
        /// This is slow due to needing a re-encode, but
        /// the resulting video may have fewer artifacts
        /// around the cuts
        /// </summary>
        public bool ForceKeyframesAtCuts { get => _forceKeyframesAtCuts.Value; set => _forceKeyframesAtCuts.Value = value; }
        /// <summary>
        /// Do not force keyframes around the chapters
        /// when cutting/splitting (default)
        /// </summary>
        public bool NoForceKeyframesAtCuts { get => _noForceKeyframesAtCuts.Value; set => _noForceKeyframesAtCuts.Value = value; }
        /// <summary>
        /// The (case sensitive) name of plugin
        /// postprocessors to be enabled, and
        /// (optionally) arguments to be passed to it,
        /// separated by a colon ":". ARGS are a
        /// semicolon ";" delimited list of NAME=VALUE.
        /// The "when" argument determines when the
        /// postprocessor is invoked. It can be one of
        /// "pre_process" (after video extraction),
        /// "after_filter" (after video passes filter),
        /// "before_dl" (before each video download),
        /// "post_process" (after each video download;
        /// default), "after_move" (after moving video
        /// file to it's final locations), "after_video"
        /// (after downloading and processing all
        /// formats of a video), or "playlist" (at end
        /// of playlist). This option can be used
        /// multiple times to add different postprocessors
        /// </summary>
        public string UsePostProcessor { get => _usePostProcessor.Value; set => _usePostProcessor.Value = value; }
    }
}
