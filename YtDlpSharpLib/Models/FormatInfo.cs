namespace YtDlpSharpLib.Models;

/// <summary>
/// Metadata for a single yt-dlp format entry.
/// </summary>
public sealed record FormatInfo
{
    /// <summary>The yt-dlp format identifier (e.g., <c>"137"</c>, <c>"bestaudio"</c>).</summary>
    public string? FormatId { get; init; }

    /// <summary>Human-readable note describing the format (e.g., <c>"1080p"</c>).</summary>
    public string? FormatNote { get; init; }

    /// <summary>The file extension (e.g., <c>"mp4"</c>).</summary>
    public string? Ext { get; init; }

    /// <summary>The video resolution (e.g., <c>"1920x1080"</c>).</summary>
    public string? Resolution { get; init; }

    /// <summary>Filesize in bytes, when known.</summary>
    public long? Filesize { get; init; }

    /// <summary>Total bitrate (kbps), when known.</summary>
    public double? Tbr { get; init; }

    /// <summary>The video codec, when applicable.</summary>
    public string? Vcodec { get; init; }

    /// <summary>The audio codec, when applicable.</summary>
    public string? Acodec { get; init; }
}
