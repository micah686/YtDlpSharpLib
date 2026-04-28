using YtDlpSharpLib.Options;

namespace YtDlpSharpLib.Downloads;

/// <summary>
/// Options for an audio-only download (maps to <c>-x --audio-format ...</c>).
/// </summary>
public sealed record AudioDownloadOptions
{
    /// <summary>The audio format to extract.</summary>
    public AudioConversionFormat AudioFormat { get; init; } = AudioConversionFormat.M4a;

    /// <summary>The underlying yt-dlp options used to build the command line.</summary>
    public YtDlpOptions YtDlp { get; init; } = new();
}
