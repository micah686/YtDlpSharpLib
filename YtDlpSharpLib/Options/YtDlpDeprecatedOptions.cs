namespace YtDlpSharpLib.Options;

/// <summary>
/// Small compatibility surface for legacy youtube-dl/yt-dlp flags that no longer appear in current help output.
/// Prefer the non-deprecated replacement options for new code.
/// </summary>
public sealed record YtDlpDeprecatedOptions
{
    /// <summary>Legacy title include filter.</summary>
    [Obsolete("Use VideoSelection.MatchFilters instead.")]
    [YtDlpArgument("--match-title", ValueName = "REGEX", Description = "Deprecated legacy title include filter.")]
    public string? MatchTitle { get; init; }

    /// <summary>Legacy title exclude filter.</summary>
    [Obsolete("Use VideoSelection.MatchFilters instead.")]
    [YtDlpArgument("--reject-title", ValueName = "REGEX", Description = "Deprecated legacy title exclude filter.")]
    public string? RejectTitle { get; init; }

    /// <summary>Legacy metadata parsing shortcut.</summary>
    [Obsolete("Use PostProcessing.ParseMetadata instead.")]
    [YtDlpArgument("--metadata-from-title", ValueName = "FORMAT", Description = "Deprecated legacy metadata parsing shortcut.")]
    public string? MetadataFromTitle { get; init; }

    /// <summary>Legacy native HLS downloader selector.</summary>
    [Obsolete("Use Download.Downloader with an m3u8/native selector instead.")]
    [YtDlpArgument("--hls-prefer-native", ValueStyle = ArgumentValueStyle.Switch, Description = "Deprecated legacy native HLS downloader selector.")]
    public bool HlsPreferNative { get; init; }

    /// <summary>Legacy FFmpeg HLS downloader selector.</summary>
    [Obsolete("Use Download.Downloader with an m3u8/ffmpeg selector instead.")]
    [YtDlpArgument("--hls-prefer-ffmpeg", ValueStyle = ArgumentValueStyle.Switch, Description = "Deprecated legacy FFmpeg HLS downloader selector.")]
    public bool HlsPreferFfmpeg { get; init; }

    /// <summary>Legacy avconv preference flag.</summary>
    [Obsolete("Use PostProcessing.FfmpegLocation or yt-dlp compatibility options instead.")]
    [YtDlpArgument("--prefer-avconv", ValueStyle = ArgumentValueStyle.Switch, Description = "Deprecated legacy avconv preference flag.")]
    public bool PreferAvconv { get; init; }

    /// <summary>Legacy FFmpeg preference flag.</summary>
    [Obsolete("Use PostProcessing.FfmpegLocation or yt-dlp compatibility options instead.")]
    [YtDlpArgument("--prefer-ffmpeg", ValueStyle = ArgumentValueStyle.Switch, Description = "Deprecated legacy FFmpeg preference flag.")]
    public bool PreferFfmpeg { get; init; }

    /// <summary>Legacy avconv binary path.</summary>
    [Obsolete("Use PostProcessing.FfmpegLocation instead.")]
    [YtDlpArgument("--avconv-location", ValueName = "PATH", Description = "Deprecated legacy avconv binary path.")]
    public string? AvconvLocation { get; init; }

    /// <summary>Legacy China verification proxy option.</summary>
    [Obsolete("Use GeoRestriction.GeoVerificationProxy instead.")]
    [YtDlpArgument("--cn-verification-proxy", ValueName = "URL", Description = "Deprecated legacy China verification proxy option.")]
    public string? CnVerificationProxy { get; init; }

    /// <summary>Legacy YouTube DASH manifest skip flag.</summary>
    [Obsolete("Use Extractor.ExtractorArgs for YouTube extractor-specific behavior instead.")]
    [YtDlpArgument("--youtube-skip-dash-manifest", ValueStyle = ArgumentValueStyle.Switch, Description = "Deprecated legacy YouTube DASH manifest skip flag.")]
    public bool YoutubeSkipDashManifest { get; init; }

    /// <summary>Legacy YouTube annotations sidecar flag.</summary>
    [Obsolete("YouTube annotations are no longer available.")]
    [YtDlpArgument("--write-annotations", ValueStyle = ArgumentValueStyle.Switch, Description = "Deprecated legacy YouTube annotations sidecar flag.")]
    public bool WriteAnnotations { get; init; }

    /// <summary>Legacy intermediate page dump flag.</summary>
    [Obsolete("Use VerbositySimulation.DumpPages or VerbositySimulation.WritePages instead.")]
    [YtDlpArgument("--load-pages", ValueStyle = ArgumentValueStyle.Switch, Description = "Deprecated legacy intermediate page dump flag.")]
    public bool LoadPages { get; init; }

    /// <summary>Legacy youtube-dl telemetry opt-out flag.</summary>
    [Obsolete("yt-dlp does not use the legacy call-home behavior.")]
    [YtDlpArgument("--no-call-home", ValueStyle = ArgumentValueStyle.Switch, Description = "Deprecated legacy youtube-dl telemetry opt-out flag.")]
    public bool NoCallHome { get; init; }

    /// <summary>Legacy ad download flag.</summary>
    [Obsolete("This legacy youtube-dl option has no supported yt-dlp replacement.")]
    [YtDlpArgument("--include-ads", ValueStyle = ArgumentValueStyle.Switch, Description = "Deprecated legacy ad download flag.")]
    public bool IncludeAds { get; init; }

    /// <summary>Legacy autonumber width option.</summary>
    [Obsolete("Use numeric formatting in Filesystem.Output instead.")]
    [YtDlpArgument("--autonumber-size", ValueName = "NUMBER", Description = "Deprecated legacy autonumber width option.")]
    public int? AutonumberSize { get; init; }
}
