namespace YtDlpSharpLib.Options;

/// <summary>
/// An advanced escape hatch that lets callers append unsupported yt-dlp flags
/// without weakening the typed surface area.
/// </summary>
public sealed record RawYtDlpArgument
{
    /// <summary>The yt-dlp flag (must start with <c>--</c>).</summary>
    public required string Name { get; init; }

    /// <summary>An optional value to follow the flag.</summary>
    public string? Value { get; init; }

    /// <summary>
    /// Indicates the value is sensitive (cookies, tokens) and should be redacted in logs/exception messages.
    /// </summary>
    public bool IsSensitive { get; init; }
}
