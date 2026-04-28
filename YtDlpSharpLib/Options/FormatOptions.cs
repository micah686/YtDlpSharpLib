namespace YtDlpSharpLib.Options;

/// <summary>
/// Options that control yt-dlp format selection and media conversion.
/// </summary>
public sealed record FormatOptions
{
    /// <summary>Maps to <c>--format</c>. Example: <c>"bestvideo+bestaudio/best"</c>.</summary>
    [YtDlpArgument("--format")]
    public FormatSelector? Format { get; init; }

    /// <summary>Maps to <c>--merge-output-format</c>.</summary>
    [YtDlpArgument("--merge-output-format")]
    public VideoContainer? MergeOutputFormat { get; init; }

    /// <summary>Maps to <c>--extract-audio</c>.</summary>
    [YtDlpArgument("--extract-audio", ValueStyle = ArgumentValueStyle.Switch)]
    public bool ExtractAudio { get; init; }

    /// <summary>Maps to <c>--audio-format</c>.</summary>
    [YtDlpArgument("--audio-format")]
    public AudioFormat? AudioFormat { get; init; }
}
