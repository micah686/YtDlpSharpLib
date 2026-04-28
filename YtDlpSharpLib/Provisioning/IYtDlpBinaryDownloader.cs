namespace YtDlpSharpLib.Provisioning;

/// <summary>
/// Downloads the external binaries that yt-dlp depends on (yt-dlp itself, ffmpeg/ffprobe, Deno).
/// Implementations must be safe to use as a singleton.
/// </summary>
public interface IYtDlpBinaryDownloader
{
    /// <summary>
    /// Downloads the latest yt-dlp release asset for the current OS/architecture.
    /// </summary>
    /// <param name="directory">Target directory; <see langword="null"/> uses the configured default.</param>
    /// <param name="progress">Optional progress callback.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The absolute path of the saved executable.</returns>
    Task<string> DownloadYtDlpAsync(
        string? directory = null,
        IProgress<BinaryDownloadProgress>? progress = null,
        CancellationToken ct = default);

    /// <summary>Downloads the latest ffmpeg build for the current OS/architecture.</summary>
    Task<string> DownloadFfmpegAsync(
        string? directory = null,
        IProgress<BinaryDownloadProgress>? progress = null,
        CancellationToken ct = default);

    /// <summary>Downloads the latest ffprobe build for the current OS/architecture.</summary>
    Task<string> DownloadFfprobeAsync(
        string? directory = null,
        IProgress<BinaryDownloadProgress>? progress = null,
        CancellationToken ct = default);

    /// <summary>Downloads the latest Deno release for the current OS/architecture.</summary>
    Task<string> DownloadDenoAsync(
        string? directory = null,
        IProgress<BinaryDownloadProgress>? progress = null,
        CancellationToken ct = default);

    /// <summary>
    /// Downloads any combination of yt-dlp, ffmpeg, ffprobe, and Deno according to <paramref name="options"/>.
    /// Failures bubble up as exceptions; partial results are not returned.
    /// </summary>
    Task<BinaryDownloadResult> DownloadAllAsync(
        BinaryDownloadOptions? options = null,
        IProgress<BinaryDownloadProgress>? progress = null,
        CancellationToken ct = default);
}
