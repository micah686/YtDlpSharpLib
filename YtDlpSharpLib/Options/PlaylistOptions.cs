namespace YtDlpSharpLib.Options;

/// <summary>
/// Options that control how yt-dlp treats playlists.
/// </summary>
public sealed record PlaylistOptions
{
    /// <summary>Maps to <c>--no-playlist</c>.</summary>
    [Obsolete("Use YtDlpOptions.VideoSelection.NoPlaylist instead.")]
    [YtDlpArgument("--no-playlist", ValueStyle = ArgumentValueStyle.Switch)]
    public bool NoPlaylist { get; init; }

    /// <summary>Maps to <c>--yes-playlist</c>.</summary>
    [Obsolete("Use YtDlpOptions.VideoSelection.YesPlaylist instead.")]
    [YtDlpArgument("--yes-playlist", ValueStyle = ArgumentValueStyle.Switch)]
    public bool YesPlaylist { get; init; }

    /// <summary>Maps to <c>--flat-playlist</c>.</summary>
    [Obsolete("Use YtDlpOptions.General.FlatPlaylist instead.")]
    [YtDlpArgument("--flat-playlist", ValueStyle = ArgumentValueStyle.Switch)]
    public bool FlatPlaylist { get; init; }

    /// <summary>Maps to <c>--playlist-items</c>. Example: <c>"1-5,8,10-"</c>.</summary>
    [Obsolete("Use YtDlpOptions.VideoSelection.PlaylistItems instead.")]
    [YtDlpArgument("--playlist-items")]
    public string? PlaylistItems { get; init; }
}
