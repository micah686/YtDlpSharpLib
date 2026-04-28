namespace YtDlpSharpLib.Options;

/// <summary>
/// Options that control yt-dlp metadata and sidecar output.
/// </summary>
public sealed record MetadataOptions
{
    /// <summary>Maps to <c>--dump-json</c>.</summary>
    [YtDlpArgument("--dump-json", ValueStyle = ArgumentValueStyle.Switch)]
    public bool DumpJson { get; init; }

    /// <summary>Maps to <c>--write-info-json</c>.</summary>
    [YtDlpArgument("--write-info-json", ValueStyle = ArgumentValueStyle.Switch)]
    public bool WriteInfoJson { get; init; }

    /// <summary>Maps to <c>--embed-metadata</c>.</summary>
    [YtDlpArgument("--embed-metadata", ValueStyle = ArgumentValueStyle.Switch)]
    public bool EmbedMetadata { get; init; }

    /// <summary>Maps to <c>--write-thumbnail</c>.</summary>
    [YtDlpArgument("--write-thumbnail", ValueStyle = ArgumentValueStyle.Switch)]
    public bool WriteThumbnail { get; init; }

    /// <summary>Maps to <c>--embed-thumbnail</c>.</summary>
    [YtDlpArgument("--embed-thumbnail", ValueStyle = ArgumentValueStyle.Switch)]
    public bool EmbedThumbnail { get; init; }

    /// <summary>Maps to <c>--skip-download</c>.</summary>
    [YtDlpArgument("--skip-download", ValueStyle = ArgumentValueStyle.Switch)]
    public bool SkipDownload { get; init; }
}
