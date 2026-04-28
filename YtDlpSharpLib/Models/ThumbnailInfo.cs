namespace YtDlpSharpLib.Models;

/// <summary>
/// Metadata for a single thumbnail entry.
/// </summary>
public sealed record ThumbnailInfo
{
    /// <summary>An identifier or sequence number for the thumbnail.</summary>
    public string? Id { get; init; }

    /// <summary>The thumbnail URL.</summary>
    public string? Url { get; init; }

    /// <summary>Width in pixels.</summary>
    public int? Width { get; init; }

    /// <summary>Height in pixels.</summary>
    public int? Height { get; init; }

    /// <summary>yt-dlp's preference score; higher is better.</summary>
    public int? Preference { get; init; }
}
