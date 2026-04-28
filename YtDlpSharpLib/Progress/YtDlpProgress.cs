namespace YtDlpSharpLib.Progress;

/// <summary>
/// A single progress event parsed from yt-dlp output.
/// </summary>
public sealed record YtDlpProgress
{
    /// <summary>The lifecycle phase the event belongs to.</summary>
    public ProgressPhase Phase { get; init; }

    /// <summary>Percent complete (0-100), when reported.</summary>
    public double? Percent { get; init; }

    /// <summary>Estimated downloaded bytes, derived from <see cref="Percent"/> and <see cref="TotalBytes"/>.</summary>
    public long? DownloadedBytes { get; init; }

    /// <summary>Total bytes for the active stream, when reported.</summary>
    public long? TotalBytes { get; init; }

    /// <summary>Speed string, e.g., <c>"2.50MiB/s"</c>.</summary>
    public string? Speed { get; init; }

    /// <summary>ETA string, e.g., <c>"00:09"</c>.</summary>
    public string? Eta { get; init; }

    /// <summary>The raw remainder of the line, for non-download phases.</summary>
    public string? AdditionalInfo { get; init; }

    /// <summary>The raw line as emitted by yt-dlp.</summary>
    public string? RawLine { get; init; }
}
