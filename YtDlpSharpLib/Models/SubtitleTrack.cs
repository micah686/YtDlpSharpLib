namespace YtDlpSharpLib.Models;

/// <summary>
/// Metadata for a single subtitle track variant.
/// </summary>
public sealed record SubtitleTrack
{
    /// <summary>The subtitle file extension (e.g., <c>"srt"</c>).</summary>
    public string? Ext { get; init; }

    /// <summary>The URL where the subtitle data can be retrieved.</summary>
    public string? Url { get; init; }

    /// <summary>Human-readable name of the subtitle track.</summary>
    public string? Name { get; init; }
}
