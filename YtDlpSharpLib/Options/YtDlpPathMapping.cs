namespace YtDlpSharpLib.Options;

/// <summary>
/// Maps a particular yt-dlp output path category to a filesystem location.
/// Rendered as <c>kind:path</c> (e.g., <c>home:/downloads</c>).
/// </summary>
public sealed record YtDlpPathMapping
{
    /// <summary>The category of output this mapping applies to.</summary>
    public required YtDlpPathKind Kind { get; init; }

    /// <summary>The filesystem path for this category.</summary>
    public required string Path { get; init; }
}
