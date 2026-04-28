namespace YtDlpSharpLib.Options;

/// <summary>
/// A yt-dlp output template (e.g., <c>"%(title).200B.%(ext)s"</c>).
/// </summary>
public sealed record OutputTemplate
{
    /// <summary>The raw output template passed to yt-dlp's <c>--output</c> flag.</summary>
    public required string Value { get; init; }
}
