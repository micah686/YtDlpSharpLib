namespace YtDlpSharpLib.Options;

/// <summary>
/// Enumerates subtitle formats supported by yt-dlp's <c>--sub-format</c> flag.
/// </summary>
public enum SubtitleFormat
{
    /// <summary>SubRip subtitle format (.srt).</summary>
    Srt,

    /// <summary>WebVTT subtitle format (.vtt).</summary>
    Vtt,

    /// <summary>Advanced SubStation Alpha (.ass).</summary>
    Ass,

    /// <summary>Best available subtitle format selected by yt-dlp.</summary>
    Best
}
