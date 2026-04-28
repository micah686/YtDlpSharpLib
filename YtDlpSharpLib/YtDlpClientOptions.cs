namespace YtDlpSharpLib;

/// <summary>
/// Configuration for <see cref="YtDlpClient"/>.
/// </summary>
public sealed record YtDlpClientOptions
{
    /// <summary>Path to the yt-dlp executable. Defaults to <c>"yt-dlp"</c> (resolved via PATH).</summary>
    public string YtDlpExecutablePath { get; init; } = "yt-dlp";

    /// <summary>Path to the ffmpeg executable used by yt-dlp for merging or conversion.</summary>
    public string FfmpegExecutablePath { get; init; } = "ffmpeg";

    /// <summary>Default maximum number of concurrent downloads for the execution scheduler.</summary>
    public int DownloadConcurrency { get; init; } = 2;

    /// <summary>
    /// Grace period given to yt-dlp to clean up after a graceful kill before the process tree is force-killed.
    /// </summary>
    public TimeSpan TerminationGracePeriod { get; init; } = TimeSpan.FromSeconds(5);

    /// <summary>How many lines of stderr to retain for error reporting on a non-zero exit.</summary>
    public int StderrTailLineCount { get; init; } = 100;

    /// <summary>Optional sink for raw yt-dlp stdout. Pass <see cref="Console.Out"/> for verbose console apps.</summary>
    public TextWriter? StdoutForwardingWriter { get; init; }

    /// <summary>Optional sink for raw yt-dlp stderr.</summary>
    public TextWriter? StderrForwardingWriter { get; init; }

    /// <summary>Environment variables to add or override for the yt-dlp child process.</summary>
    public IReadOnlyDictionary<string, string?> EnvironmentVariables { get; init; } =
        new Dictionary<string, string?>(StringComparer.Ordinal);
}
