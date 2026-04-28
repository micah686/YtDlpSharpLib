using YtDlpSharpLib.Options;

namespace YtDlpSharpLib.Downloads;

/// <summary>
/// Options for downloading the live-chat replay of a stream
/// (maps to <c>--write-subs --sub-langs live_chat --skip-download</c>).
/// </summary>
public sealed record LiveChatDownloadOptions
{
    /// <summary>The underlying yt-dlp options used to build the command line.</summary>
    public YtDlpOptions YtDlp { get; init; } = new();
}
