namespace YtDlpSharpLib.Options;

/// <summary>
/// Options that control yt-dlp subtitle and caption behavior.
/// </summary>
public sealed record SubtitleOptions
{
    /// <summary>Maps to <c>--write-subs</c>.</summary>
    [YtDlpArgument("--write-subs", ValueStyle = ArgumentValueStyle.Switch)]
    public bool WriteSubtitles { get; init; }

    /// <summary>Maps to <c>--sub-langs</c>. Example: <c>"en.*,ja"</c>.</summary>
    [YtDlpArgument("--sub-langs")]
    public string? Languages { get; init; }

    /// <summary>Maps to <c>--sub-format</c>.</summary>
    [YtDlpArgument("--sub-format")]
    public SubtitleFormat? Format { get; init; }
}
