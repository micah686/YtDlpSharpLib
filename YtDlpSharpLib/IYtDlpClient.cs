using YtDlpSharpLib.Downloads;
using YtDlpSharpLib.Models;
using YtDlpSharpLib.Progress;

namespace YtDlpSharpLib;

/// <summary>
/// Primary client for yt-dlp operations.
/// </summary>
public interface IYtDlpClient
{
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
}
