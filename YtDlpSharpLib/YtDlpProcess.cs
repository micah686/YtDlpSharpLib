//https://github.com/Bluegrams/YoutubeDLSharp
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using YtDlpSharpLib.Helpers;
using YtDlpSharpLib.Options;

namespace YtDlpSharpLib;

/// <summary>
/// A low-level wrapper for the yt-dlp executable.
/// </summary>
public class YtDlpProcess
{
    // the regex used to match the currently downloaded video of a playlist.
    private static readonly Regex RgxPlaylist = new Regex(@"Downloading video (\d+) of (\d+)", RegexOptions.Compiled);
    // the regex used for matching download progress information.
    private static readonly Regex RgxProgress = new Regex(
        @"\[download\]\s+(?:(?<percent>[\d\.]+)%(?:\s+of\s+\~?(?<total>[\d\.\w]+))?\s+at\s+(?:(?<speed>[\d\.\w]+\/s)|[\w\s]+)\s+ETA\s(?<eta>[\d\:]+))?",
        RegexOptions.Compiled
    );
    // the regex used to match the beginning of post-processing.
    private static readonly Regex RgxPost = new Regex(@"\[(\w+)\]\s+", RegexOptions.Compiled);

    /// <summary>
    /// The path to the yt-dlp executable.
    /// </summary>
    public string ExecutablePath { get; set; }

    /// <summary>
    /// Occurs each time yt-dlp writes to the standard output.
    /// </summary>
    public event EventHandler<DataReceivedEventArgs> OutputReceived;
    /// <summary>
    /// Occurs each time yt-dlp writes to the error output.
    /// </summary>
    public event EventHandler<DataReceivedEventArgs> ErrorReceived;

    /// <summary>
    /// Creates a new instance of the YoutubeDLProcess class.
    /// </summary>
    /// <param name="executablePath">The path to the yt-dlp executable.</param>
    public YtDlpProcess(string executablePath = "yt-dlp.exe")
    {
        this.ExecutablePath = executablePath;
    }

    internal string ConvertToArgs(string[] urls, OptionSet options)
        => (urls != null ? string.Join(" ", urls) : string.Empty) + options;

    /// <summary>
    /// Invokes yt-dlp with the specified parameters and options.
    /// </summary>
    /// <param name="urls">The video URLs to be passed to yt-dlp.</param>
    /// <param name="options">An OptionSet specifying the options to be passed to yt-dlp.</param>
    /// <returns>The exit code of the yt-dlp process.</returns>
    public async Task<int> RunAsync(string[] urls, OptionSet options)
        => await RunAsync(urls, options, CancellationToken.None);

    /// <summary>
    /// Invokes yt-dlp with the specified parameters and options.
    /// </summary>
    /// <param name="urls">The video URLs to be passed to yt-dlp.</param>
    /// <param name="options">An OptionSet specifying the options to be passed to yt-dlp.</param>
    /// <param name="ct">A CancellationToken used to cancel the download.</param>
    /// <param name="progress">A progress provider used to get download progress information.</param>
    /// <returns>The exit code of the yt-dlp process.</returns>
    public async Task<int> RunAsync(string[] urls, OptionSet options,
        CancellationToken ct, IProgress<DownloadProgress> progress = null)
    {
        var tcs = new TaskCompletionSource<int>();
        var process = new Process();
        var startInfo = new ProcessStartInfo()
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true

        };
        startInfo.FileName = ExecutablePath;
        startInfo.Arguments = ConvertToArgs(urls, options);
        process.EnableRaisingEvents = true;
        process.StartInfo = startInfo;
        var tcsOut = new TaskCompletionSource<bool>();
        // this variable is used for tracking download states
        bool isDownloading = false;
        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data == null)
            {
                tcsOut.SetResult(true);
                return;
            }
            Match match;
            if ((match = RgxProgress.Match(e.Data)).Success)
            {
                if (match.Groups.Count > 1 && match.Groups[1].Length > 0)
                {
                    float progValue = float.Parse(match.Groups[1].ToString(), CultureInfo.InvariantCulture) / 100.0f;
                    Group totalGroup = match.Groups["total"];
                    string total = totalGroup.Success ? totalGroup.Value : null;
                    Group speedGroup = match.Groups["speed"];
                    string speed = speedGroup.Success ? speedGroup.Value : null;
                    Group etaGroup = match.Groups["eta"];
                    string eta = etaGroup.Success ? etaGroup.Value : null;
                    progress?.Report(
                        new DownloadProgress(
                            DownloadState.Downloading, progress: progValue, totalDownloadSize: total, downloadSpeed: speed, eta: eta
                        )
                    );
                }
                else
                {
                    progress?.Report(new DownloadProgress(DownloadState.Downloading));
                }
                isDownloading = true;
            }
            else if ((match = RgxPlaylist.Match(e.Data)).Success)
            {
                var index = int.Parse(match.Groups[1].Value);
                progress?.Report(new DownloadProgress(DownloadState.PreProcessing, index: index));
                isDownloading = false;
            }
            else if (isDownloading && RgxPost.Match(e.Data).Success)
            {
                progress?.Report(new DownloadProgress(DownloadState.PostProcessing, 1));
                isDownloading = false;
            }
            Debug.WriteLine("[youtube-dl] " + e.Data);
            OutputReceived?.Invoke(this, e);
        };
        var tcsError = new TaskCompletionSource<bool>();
        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data == null)
            {
                tcsError.SetResult(true);
                return;
            }
            Debug.WriteLine("[yt-dlp ERROR] " + e.Data);
            progress?.Report(new DownloadProgress(DownloadState.Error, data: e.Data));
            ErrorReceived?.Invoke(this, e);
        };
        process.Exited += async (_, _) =>
        {
            // Wait for output and error streams to finish
            await tcsOut.Task;
            await tcsError.Task;
            tcs.TrySetResult(process.ExitCode);
            process.Dispose();
        };
        ct.Register(() =>
        {
            if (!tcs.Task.IsCompleted)
                tcs.TrySetCanceled();
            try { if (!process.HasExited) process.KillTree(); }
            catch { }
        });
        Debug.WriteLine("[yt-dlp] Arguments: " + process.StartInfo.Arguments);
        if (!await Task.Run(() => process.Start()))
            tcs.TrySetException(new InvalidOperationException("Failed to start yt-dlp process."));
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        progress?.Report(new DownloadProgress(DownloadState.PreProcessing));
        return await tcs.Task;
    }
}
