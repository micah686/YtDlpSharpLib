namespace YtDlpSharpLib.Options;

/// <summary>
/// Identifies the role of a yt-dlp output path mapping (the prefix in <c>--paths home:/downloads</c>).
/// </summary>
public enum YtDlpPathKind
{
    /// <summary>Final output directory for all media.</summary>
    Home,

    /// <summary>Directory used for temporary fragments and intermediate files.</summary>
    Temp,

    /// <summary>Directory for sidecar subtitle files.</summary>
    Subtitle,

    /// <summary>Directory for sidecar thumbnail files.</summary>
    Thumbnail,

    /// <summary>Directory for sidecar info-json files.</summary>
    InfoJson
}
