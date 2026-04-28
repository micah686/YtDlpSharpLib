namespace YtDlpSharpLib.Options;

/// <summary>
/// Options that control yt-dlp subtitle and caption behavior.
/// </summary>
public sealed record SubtitleOptions
{
    /// <summary>Maps to <c>--write-subs</c>.</summary>
    [Obsolete("Use YtDlpOptions.Subtitle.WriteSubs instead.")]
    [YtDlpArgument("--write-subs", ValueStyle = ArgumentValueStyle.Switch)]
    public bool WriteSubtitles { get; init; }

    /// <summary>Maps to <c>--sub-langs</c>. Example: <c>"en.*,ja"</c>.</summary>
    [Obsolete("Use YtDlpOptions.Subtitle.SubLangs instead.")]
    [YtDlpArgument("--sub-langs")]
    public string? Languages { get; init; }

    /// <summary>Maps to <c>--sub-format</c>.</summary>
    [Obsolete("Use YtDlpOptions.Subtitle.SubFormat instead.")]
    [YtDlpArgument("--sub-format")]
    public SubtitleFormat? Format { get; init; }
}
