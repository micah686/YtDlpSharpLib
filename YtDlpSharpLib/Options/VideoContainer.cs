namespace YtDlpSharpLib.Options;

/// <summary>
/// Enumerates the video containers yt-dlp accepts for <c>--merge-output-format</c>.
/// </summary>
public enum VideoContainer
{
    /// <summary>MPEG-4 (.mp4).</summary>
    Mp4,

    /// <summary>Matroska (.mkv).</summary>
    Mkv,

    /// <summary>WebM (.webm).</summary>
    Webm
}
