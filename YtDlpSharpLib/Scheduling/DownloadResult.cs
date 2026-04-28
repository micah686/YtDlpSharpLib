namespace YtDlpSharpLib.Scheduling;

/// <summary>
/// The outcome of a single scheduled download.
/// </summary>
public sealed record DownloadResult
{
    /// <summary>The URL that was processed.</summary>
    public required string Url { get; init; }

    /// <summary>The yt-dlp exit code, when available. <see langword="null"/> when the job failed before exit.</summary>
    public int? ExitCode { get; init; }

    /// <summary>
    /// The path to the produced output file, when known. yt-dlp does not always emit this; consumers
    /// can supplement with <see cref="IYtDlpClient.GetVideoInfoAsync"/> if needed.
    /// </summary>
    public string? OutputFile { get; init; }

    /// <summary>The exception that ended the job, when not successful.</summary>
    public Exception? Error { get; init; }
}
