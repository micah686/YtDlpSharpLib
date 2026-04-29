using YtDlpSharpLib.Downloads;
using YtDlpSharpLib.Models;
using YtDlpSharpLib.Progress;

namespace YtDlpSharpLib;

/// <summary>
/// Primary client for yt-dlp operations.
/// </summary>
public interface IYtDlpClient
{
    /// <summary>
    /// Default output folder for downloads. Per-call <see cref="Options.YtDlpFilesystemOptions.Paths"/> wins when set.
    /// </summary>
    string? OutputFolder { get; set; }

    /// <summary>
    /// Default output filename template. Per-call <see cref="Options.YtDlpFilesystemOptions.Output"/> wins when set.
    /// </summary>
    string? OutputFileTemplate { get; set; }

    /// <summary>
    /// Whether downloads should use restricted filenames by default. Per-call filename flags win when set.
    /// </summary>
    bool RestrictFilenames { get; set; }

    /// <summary>
    /// Whether downloads should overwrite existing files by default. Per-call overwrite flags win when set.
    /// </summary>
    bool OverwriteFiles { get; set; }

    /// <summary>
    /// Whether download errors should be ignored by default. Per-call error-handling flags win when set.
    /// </summary>
    bool IgnoreDownloadErrors { get; set; }

    /// <summary>Retrieves structured video information without downloading any media.</summary>
    Task<VideoInfo> GetVideoInfoAsync(string url, CancellationToken ct = default);

    /// <summary>Streams video metadata for each entry of a playlist.</summary>
    IAsyncEnumerable<VideoInfo> GetPlaylistInfoAsync(string url, CancellationToken ct = default);

    /// <summary>Downloads a video, optionally reporting progress through the supplied callback.</summary>
    Task DownloadAsync(
        string url,
        string outputDirectory,
        DownloadOptions? options = null,
        IProgress<YtDlpProgress>? progress = null,
        CancellationToken ct = default);

    /// <summary>Downloads a video, exposing progress as an asynchronous stream.</summary>
    IAsyncEnumerable<YtDlpProgress> DownloadWithProgressAsync(
        string url,
        string outputDirectory,
        DownloadOptions? options = null,
        CancellationToken ct = default);

    /// <summary>Downloads audio only (maps to <c>-x --audio-format ...</c>).</summary>
    Task DownloadAudioAsync(
        string url,
        string outputDirectory,
        AudioDownloadOptions? options = null,
        IProgress<YtDlpProgress>? progress = null,
        CancellationToken ct = default);

    /// <summary>Downloads every entry of a video playlist.</summary>
    Task DownloadPlaylistAsync(
        string url,
        string outputDirectory,
        PlaylistDownloadOptions? options = null,
        IProgress<YtDlpProgress>? progress = null,
        CancellationToken ct = default);

    /// <summary>Downloads every entry of an audio-only playlist.</summary>
    Task DownloadAudioPlaylistAsync(
        string url,
        string outputDirectory,
        AudioPlaylistDownloadOptions? options = null,
        IProgress<YtDlpProgress>? progress = null,
        CancellationToken ct = default);

    /// <summary>Downloads metadata only (maps to <c>--write-info-json --skip-download</c>).</summary>
    Task DownloadMetadataAsync(
        string url,
        string outputDirectory,
        MetadataDownloadOptions? options = null,
        CancellationToken ct = default);

    /// <summary>Downloads the live-chat replay (maps to <c>--write-subs --sub-langs live_chat --skip-download</c>).</summary>
    Task DownloadLiveChatAsync(
        string url,
        string outputDirectory,
        LiveChatDownloadOptions? options = null,
        CancellationToken ct = default);

    /// <summary>Returns the yt-dlp version string.</summary>
    Task<string> GetVersionAsync(CancellationToken ct = default);

    /// <summary>Runs yt-dlp's self-update passthrough (maps to <c>--update</c>).</summary>
    Task RunUpdateAsync(CancellationToken ct = default);
}
