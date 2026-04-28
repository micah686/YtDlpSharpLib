namespace YtDlpSharpLib.Options;

/// <summary>
/// Options that control yt-dlp output paths, templates, and filename behavior.
/// </summary>
public sealed record OutputOptions
{
    /// <summary>Maps to <c>--output</c>. Example: <c>"%(title).200B.%(ext)s"</c>.</summary>
    [YtDlpArgument("--output")]
    public OutputTemplate? Template { get; init; }

    /// <summary>Maps to <c>--restrict-filenames</c>.</summary>
    [YtDlpArgument("--restrict-filenames", ValueStyle = ArgumentValueStyle.Switch)]
    public bool RestrictFilenames { get; init; }

    /// <summary>Maps to <c>--windows-filenames</c>.</summary>
    [YtDlpArgument("--windows-filenames", ValueStyle = ArgumentValueStyle.Switch)]
    public bool WindowsSafeFilenames { get; init; }

    /// <summary>Maps to <c>--paths</c>. Each entry renders as <c>kind:path</c>.</summary>
    [YtDlpArgument("--paths", AllowMultiple = true)]
    public IReadOnlyList<YtDlpPathMapping> Paths { get; init; } = [];
}
