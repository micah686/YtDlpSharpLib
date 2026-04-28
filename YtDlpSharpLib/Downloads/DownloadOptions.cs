using YtDlpSharpLib.Options;

namespace YtDlpSharpLib.Downloads;

/// <summary>
/// Options for a generic video download. Wraps a <see cref="YtDlpOptions"/> instance
/// so callers can compose any flag the library exposes.
/// </summary>
public sealed record DownloadOptions
{
    /// <summary>The underlying yt-dlp options used to build the command line.</summary>
    public YtDlpOptions YtDlp { get; init; } = new();
}
