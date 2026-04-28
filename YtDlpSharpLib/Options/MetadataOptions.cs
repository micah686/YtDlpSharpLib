namespace YtDlpSharpLib.Options;

/// <summary>
/// Options that control yt-dlp metadata and sidecar output.
/// </summary>
public sealed record MetadataOptions
{
    /// <summary>Maps to <c>--dump-json</c>.</summary>
    [Obsolete("Use YtDlpOptions.VerbositySimulation.DumpJson instead.")]
    [YtDlpArgument("--dump-json", ValueStyle = ArgumentValueStyle.Switch)]
    public bool DumpJson { get; init; }

    /// <summary>Maps to <c>--write-info-json</c>.</summary>
    [Obsolete("Use YtDlpOptions.Filesystem.WriteInfoJson instead.")]
    [YtDlpArgument("--write-info-json", ValueStyle = ArgumentValueStyle.Switch)]
    public bool WriteInfoJson { get; init; }

    /// <summary>Maps to <c>--embed-metadata</c>.</summary>
    [Obsolete("Use YtDlpOptions.PostProcessing.EmbedMetadata instead.")]
    [YtDlpArgument("--embed-metadata", ValueStyle = ArgumentValueStyle.Switch)]
    public bool EmbedMetadata { get; init; }

    /// <summary>Maps to <c>--write-thumbnail</c>.</summary>
    [Obsolete("Use YtDlpOptions.Thumbnail.WriteThumbnail instead.")]
    [YtDlpArgument("--write-thumbnail", ValueStyle = ArgumentValueStyle.Switch)]
    public bool WriteThumbnail { get; init; }

    /// <summary>Maps to <c>--embed-thumbnail</c>.</summary>
    [Obsolete("Use YtDlpOptions.PostProcessing.EmbedThumbnail instead.")]
    [YtDlpArgument("--embed-thumbnail", ValueStyle = ArgumentValueStyle.Switch)]
    public bool EmbedThumbnail { get; init; }

    /// <summary>Maps to <c>--skip-download</c>.</summary>
    [Obsolete("Use YtDlpOptions.VerbositySimulation.SkipDownload instead.")]
    [YtDlpArgument("--skip-download", ValueStyle = ArgumentValueStyle.Switch)]
    public bool SkipDownload { get; init; }
}
