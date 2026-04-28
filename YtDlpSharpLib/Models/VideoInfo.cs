using System.Globalization;
using System.Text.Json.Serialization;

namespace YtDlpSharpLib.Models;

/// <summary>
/// Strongly-typed metadata returned by yt-dlp's <c>--dump-json</c> output.
/// </summary>
public sealed record VideoInfo
{
    /// <summary>The yt-dlp video id.</summary>
    public required string Id { get; init; }

    /// <summary>The video title.</summary>
    public required string Title { get; init; }

    /// <summary>Free-form video description.</summary>
    public string? Description { get; init; }

    /// <summary>The yt-dlp extractor name (e.g., <c>"youtube"</c>).</summary>
    public required string Extractor { get; init; }

    /// <summary>The raw upload date string from yt-dlp (typically <c>YYYYMMDD</c>).</summary>
    public string? UploadDate { get; init; }

    /// <summary>The upload date parsed as a <see cref="DateOnly"/>, when available.</summary>
    [JsonIgnore]
    public DateOnly? ParsedUploadDate => DateOnly.TryParseExact(
        UploadDate,
        "yyyyMMdd",
        CultureInfo.InvariantCulture,
        DateTimeStyles.None,
        out var d)
        ? d
        : null;

    /// <summary>Duration in seconds.</summary>
    public long? Duration { get; init; }

    /// <summary>The canonical webpage URL for the video.</summary>
    public required string WebpageUrl { get; init; }

    /// <summary>All available formats yt-dlp discovered.</summary>
    public IReadOnlyList<FormatInfo> Formats { get; init; } = [];

    /// <summary>All thumbnails associated with the video.</summary>
    public IReadOnlyList<ThumbnailInfo>? Thumbnails { get; init; }

    /// <summary>Chapter markers, when present.</summary>
    public IReadOnlyList<ChapterInfo>? Chapters { get; init; }

    /// <summary>Subtitle tracks discovered for the video, keyed by language code.</summary>
    public IReadOnlyDictionary<string, IReadOnlyList<SubtitleTrack>>? Subtitles { get; init; }
}
