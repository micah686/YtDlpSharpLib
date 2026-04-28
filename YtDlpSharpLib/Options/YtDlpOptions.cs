namespace YtDlpSharpLib.Options;

/// <summary>
/// Root options model for a yt-dlp invocation. Each grouped sub-record corresponds
/// to a logical area of the yt-dlp CLI surface.
/// </summary>
public sealed record YtDlpOptions
{
    /// <summary>Controls format selection and media conversion.</summary>
    public FormatOptions Format { get; init; } = new();

    /// <summary>Controls output paths, templates, and filename behavior.</summary>
    public OutputOptions Output { get; init; } = new();

    /// <summary>Controls subtitles and captions.</summary>
    public SubtitleOptions Subtitles { get; init; } = new();

    /// <summary>Controls metadata and sidecar output.</summary>
    public MetadataOptions Metadata { get; init; } = new();

    /// <summary>Controls playlist handling.</summary>
    public PlaylistOptions Playlist { get; init; } = new();

    /// <summary>Advanced escape hatch for unsupported yt-dlp flags.</summary>
    public IReadOnlyList<RawYtDlpArgument> AdvancedArguments { get; init; } = [];
}
