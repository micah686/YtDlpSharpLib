using YtDlpSharpLib.Options;

namespace YtDlpSharpLib.Downloads;

/// <summary>
/// Options for downloading every entry of a playlist.
/// </summary>
public sealed record PlaylistDownloadOptions
{
    /// <summary>Optional playlist item selector, e.g., <c>"1-5,8,10-"</c>.</summary>
    public string? PlaylistItems { get; init; }

    /// <summary>The underlying yt-dlp options used to build the command line.</summary>
    public YtDlpOptions YtDlp { get; init; } = new();
}
