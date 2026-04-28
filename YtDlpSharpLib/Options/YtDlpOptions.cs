namespace YtDlpSharpLib.Options;

/// <summary>
/// Root options model for a yt-dlp invocation. The generated partial declaration
/// contains the typed option groups emitted from <c>yt-dlp --help</c>.
/// </summary>
public sealed partial record YtDlpOptions
{
    /// <summary>Advanced escape hatch for unsupported yt-dlp flags.</summary>
    public IReadOnlyList<RawYtDlpArgument> AdvancedArguments { get; init; } = [];
}
