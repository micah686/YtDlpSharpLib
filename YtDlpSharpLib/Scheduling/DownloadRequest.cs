using YtDlpSharpLib.Downloads;

namespace YtDlpSharpLib.Scheduling;

/// <summary>
/// A single download job submitted to the execution scheduler.
/// </summary>
public sealed record DownloadRequest
{
    /// <summary>The URL to download.</summary>
    public required string Url { get; init; }

    /// <summary>The output directory the downloaded media should be written to.</summary>
    public required string OutputDirectory { get; init; }

    /// <summary>Optional download options.</summary>
    public DownloadOptions? Options { get; init; }
}
