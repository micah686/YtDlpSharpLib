namespace YtDlpSharpLib.Options;

/// <summary>
/// Wraps a yt-dlp format selector expression (e.g., <c>"bestvideo+bestaudio/best"</c>).
/// </summary>
public readonly record struct FormatSelector(string Value);
