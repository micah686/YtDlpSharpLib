using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using Microsoft.Extensions.Options;
using YtDlpSharpLib.Downloads;
using YtDlpSharpLib.Exceptions;
using YtDlpSharpLib.Models;
using YtDlpSharpLib.Options;
using YtDlpSharpLib.Process;
using YtDlpSharpLib.Progress;
using YtDlpSharpLib.Rendering;
using YtDlpSharpLib.Internal;

namespace YtDlpSharpLib;

/// <summary>
/// Default <see cref="IYtDlpClient"/> implementation. Composes a <see cref="IYtDlpProcessFactory"/>,
/// an <see cref="IYtDlpArgumentRenderer"/>, and a <see cref="TimeProvider"/> to launch and observe
/// yt-dlp child processes.
/// </summary>
public sealed class YtDlpClient : IYtDlpClient
{
    private readonly YtDlpClientOptions _options;
    private readonly IYtDlpProcessFactory _factory;
    private readonly IYtDlpArgumentRenderer _renderer;
    private readonly TimeProvider _timeProvider;

    /// <summary>Creates a client from typed options. Suitable for direct (non-DI) usage.</summary>
    public YtDlpClient(
        YtDlpClientOptions options,
        IYtDlpProcessFactory processFactory,
        IYtDlpArgumentRenderer argumentRenderer,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(processFactory);
        ArgumentNullException.ThrowIfNull(argumentRenderer);
        ArgumentNullException.ThrowIfNull(timeProvider);

        _options = options;
        _factory = processFactory;
        _renderer = argumentRenderer;
        _timeProvider = timeProvider;
    }

    /// <summary>Creates a client from <see cref="IOptions{TOptions}"/>. Used by the DI container.</summary>
    public YtDlpClient(
        IOptions<YtDlpClientOptions> options,
        IYtDlpProcessFactory processFactory,
        IYtDlpArgumentRenderer argumentRenderer,
        TimeProvider timeProvider)
        : this(
            (options ?? throw new ArgumentNullException(nameof(options))).Value,
            processFactory,
            argumentRenderer,
            timeProvider)
    {
    }

    /// <inheritdoc />
    public async Task<VideoInfo> GetVideoInfoAsync(string url, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);

        var args = new List<string>
        {
            "--dump-single-json",
            "--no-playlist",
            url
        };
        var startInfo = BuildBareStartInfo(args);

        var stdout = new StringBuilder();
        await RunInternalAsync(
            startInfo,
            (line, _) =>
            {
                stdout.AppendLine(line);
                return ValueTask.CompletedTask;
            },
            ct).ConfigureAwait(false);

        return DeserializeVideoInfo(stdout.ToString());
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<VideoInfo> GetPlaylistInfoAsync(
        string url,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);

        var args = new List<string>
        {
            "--dump-json",
            "--yes-playlist",
            "--ignore-no-formats-error",
            url
        };
        var startInfo = BuildBareStartInfo(args);

        await foreach (var line in StreamStdoutAsync(startInfo, ct).ConfigureAwait(false))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            yield return DeserializeVideoInfo(line);
        }
    }

    /// <inheritdoc />
    public Task DownloadAsync(
        string url,
        string outputDirectory,
        DownloadOptions? options = null,
        IProgress<YtDlpProgress>? progress = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);

        var ytDlp = ApplyDownloadDefaults((options ?? new DownloadOptions()).YtDlp, outputDirectory);
        return RunDownloadAsync(url, outputDirectory, ytDlp, progress, ct);
    }

    /// <inheritdoc />
    public IAsyncEnumerable<YtDlpProgress> DownloadWithProgressAsync(
        string url,
        string outputDirectory,
        DownloadOptions? options = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);

        var ytDlp = ApplyDownloadDefaults((options ?? new DownloadOptions()).YtDlp, outputDirectory);
        return StreamProgressAsync(url, outputDirectory, ytDlp, ct);
    }

    /// <inheritdoc />
    public Task DownloadAudioAsync(
        string url,
        string outputDirectory,
        AudioDownloadOptions? options = null,
        IProgress<YtDlpProgress>? progress = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);

        var resolved = options ?? new AudioDownloadOptions();
        var ytDlp = ApplyDownloadDefaults(resolved.YtDlp, outputDirectory);
        ytDlp = ytDlp with
        {
            PostProcessing = ytDlp.PostProcessing with
            {
                ExtractAudio = true,
                AudioFormat = RenderAudioFormat(resolved.AudioFormat)
            }
        };
        return RunDownloadAsync(url, outputDirectory, ytDlp, progress, ct);
    }

    /// <inheritdoc />
    public Task DownloadPlaylistAsync(
        string url,
        string outputDirectory,
        PlaylistDownloadOptions? options = null,
        IProgress<YtDlpProgress>? progress = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);

        var resolved = options ?? new PlaylistDownloadOptions();
        var ytDlp = ApplyDownloadDefaults(resolved.YtDlp, outputDirectory);
        ytDlp = ytDlp with
        {
            VideoSelection = ytDlp.VideoSelection with
            {
                YesPlaylist = true,
                PlaylistItems = resolved.PlaylistItems ?? ytDlp.VideoSelection.PlaylistItems
            }
        };
        return RunDownloadAsync(url, outputDirectory, ytDlp, progress, ct);
    }

    /// <inheritdoc />
    public Task DownloadAudioPlaylistAsync(
        string url,
        string outputDirectory,
        AudioPlaylistDownloadOptions? options = null,
        IProgress<YtDlpProgress>? progress = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);

        var resolved = options ?? new AudioPlaylistDownloadOptions();
        var ytDlp = ApplyDownloadDefaults(resolved.YtDlp, outputDirectory);
        ytDlp = ytDlp with
        {
            PostProcessing = ytDlp.PostProcessing with
            {
                ExtractAudio = true,
                AudioFormat = RenderAudioFormat(resolved.AudioFormat)
            },
            VideoSelection = ytDlp.VideoSelection with
            {
                YesPlaylist = true,
                PlaylistItems = resolved.PlaylistItems ?? ytDlp.VideoSelection.PlaylistItems
            }
        };
        return RunDownloadAsync(url, outputDirectory, ytDlp, progress, ct);
    }

    /// <inheritdoc />
    public Task DownloadMetadataAsync(
        string url,
        string outputDirectory,
        MetadataDownloadOptions? options = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);

        var resolved = options ?? new MetadataDownloadOptions();
        var ytDlp = ApplyDownloadDefaults(resolved.YtDlp, outputDirectory);
        ytDlp = ytDlp with
        {
            Filesystem = ytDlp.Filesystem with
            {
                WriteInfoJson = true
            },
            Thumbnail = ytDlp.Thumbnail with
            {
                WriteThumbnail = ytDlp.Thumbnail.WriteThumbnail || resolved.WriteThumbnail
            },
            Subtitle = ytDlp.Subtitle with
            {
                WriteSubs = ytDlp.Subtitle.WriteSubs || resolved.WriteSubtitles,
                SubLangs = ytDlp.Subtitle.SubLangs ?? resolved.SubtitleLanguages
            },
            VerbositySimulation = ytDlp.VerbositySimulation with
            {
                SkipDownload = true
            }
        };

        return RunDownloadAsync(url, outputDirectory, ytDlp, progress: null, ct);
    }

    /// <inheritdoc />
    public Task DownloadLiveChatAsync(
        string url,
        string outputDirectory,
        LiveChatDownloadOptions? options = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);

        var resolved = options ?? new LiveChatDownloadOptions();
        var ytDlp = ApplyDownloadDefaults(resolved.YtDlp, outputDirectory);
        ytDlp = ytDlp with
        {
            Subtitle = ytDlp.Subtitle with
            {
                WriteSubs = true,
                SubLangs = "live_chat"
            },
            VerbositySimulation = ytDlp.VerbositySimulation with
            {
                SkipDownload = true
            }
        };

        return RunDownloadAsync(url, outputDirectory, ytDlp, progress: null, ct);
    }

    /// <inheritdoc />
    public async Task<string> GetVersionAsync(CancellationToken ct = default)
    {
        var startInfo = BuildBareStartInfo(["--version"]);
        var stdout = new StringBuilder();

        await RunInternalAsync(
            startInfo,
            (line, _) =>
            {
                stdout.AppendLine(line);
                return ValueTask.CompletedTask;
            },
            ct).ConfigureAwait(false);

        return stdout.ToString().Trim();
    }

    private Task RunDownloadAsync(
        string url,
        string outputDirectory,
        YtDlpOptions ytDlpOptions,
        IProgress<YtDlpProgress>? progress,
        CancellationToken ct)
    {
        var startInfo = BuildDownloadStartInfo(ytDlpOptions, outputDirectory, url);

        Func<string, CancellationToken, ValueTask>? handler = null;
        if (progress is not null)
        {
            handler = (line, _) =>
            {
                if (ProgressLineParser.TryParse(line, out var parsed))
                {
                    progress.Report(parsed);
                }
                return ValueTask.CompletedTask;
            };
        }

        return RunInternalAsync(startInfo, handler, ct);
    }

    private async IAsyncEnumerable<YtDlpProgress> StreamProgressAsync(
        string url,
        string outputDirectory,
        YtDlpOptions ytDlpOptions,
        [EnumeratorCancellation] CancellationToken ct)
    {
        _ = outputDirectory;
        var startInfo = BuildDownloadStartInfo(ytDlpOptions, outputDirectory, url);

        await foreach (var line in StreamStdoutAsync(startInfo, ct).ConfigureAwait(false))
        {
            if (ProgressLineParser.TryParse(line, out var parsed))
            {
                yield return parsed;
            }
        }
    }

    private async IAsyncEnumerable<string> StreamStdoutAsync(
        YtDlpProcessStartInfo startInfo,
        [EnumeratorCancellation] CancellationToken ct)
    {
        await using var process = _factory.Create(startInfo);
        var stderrBuffer = new RingBuffer<string>(_options.StderrTailLineCount);
        var stderrTask = StartStderrTailingAsync(process, stderrBuffer);

        await process.StartAsync(ct).ConfigureAwait(false);

        var natural = false;
        try
        {
            await foreach (var line in process.StdoutLines.ReadAllAsync(ct).ConfigureAwait(false))
            {
                yield return line;
            }

            natural = true;
        }
        finally
        {
            if (!natural)
            {
                await GracefulCancelAsync(process).ConfigureAwait(false);
            }

            await SafeAwaitAsync(stderrTask).ConfigureAwait(false);
        }

        await process.WaitForExitAsync(CancellationToken.None).ConfigureAwait(false);

        var exitCode = process.ExitCode ?? -1;
        if (exitCode != 0)
        {
            throw new YtDlpProcessException(
                $"yt-dlp exited with code {exitCode.ToString(CultureInfo.InvariantCulture)}.",
                command: BuildCommandSummary(startInfo),
                exitCode: exitCode,
                lastStderrLines: JoinStderr(stderrBuffer));
        }
    }

    private async Task RunInternalAsync(
        YtDlpProcessStartInfo startInfo,
        Func<string, CancellationToken, ValueTask>? handleStdoutLine,
        CancellationToken ct)
    {
        await using var process = _factory.Create(startInfo);
        var stderrBuffer = new RingBuffer<string>(_options.StderrTailLineCount);

        var stderrTask = StartStderrTailingAsync(process, stderrBuffer);
        var stdoutTask = StartStdoutConsumingAsync(process, handleStdoutLine);

        await process.StartAsync(ct).ConfigureAwait(false);

        try
        {
            await process.WaitForExitAsync(ct).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            await GracefulCancelAsync(process).ConfigureAwait(false);
            throw;
        }

        await SafeAwaitAsync(stdoutTask).ConfigureAwait(false);
        await SafeAwaitAsync(stderrTask).ConfigureAwait(false);

        var exitCode = process.ExitCode ?? -1;
        if (exitCode != 0)
        {
            throw new YtDlpProcessException(
                $"yt-dlp exited with code {exitCode.ToString(CultureInfo.InvariantCulture)}.",
                command: BuildCommandSummary(startInfo),
                exitCode: exitCode,
                lastStderrLines: JoinStderr(stderrBuffer));
        }
    }

    private async Task GracefulCancelAsync(IYtDlpProcess process)
    {
        if (process.HasExited)
        {
            return;
        }

        process.Kill(entireProcessTree: false);

        var graceTask = Task.Delay(_options.TerminationGracePeriod, _timeProvider, CancellationToken.None);
        var exitTask = process.WaitForExitAsync(CancellationToken.None);
        var winner = await Task.WhenAny(graceTask, exitTask).ConfigureAwait(false);

        if (winner == graceTask && !process.HasExited)
        {
            process.Kill(entireProcessTree: true);
            await SafeAwaitAsync(process.WaitForExitAsync(CancellationToken.None)).ConfigureAwait(false);
        }
    }

    private static Task StartStderrTailingAsync(IYtDlpProcess process, RingBuffer<string> buffer)
    {
        return Task.Run(async () =>
        {
            await foreach (var line in process.StderrLines.ReadAllAsync(CancellationToken.None).ConfigureAwait(false))
            {
                buffer.Add(line);
            }
        }, CancellationToken.None);
    }

    private static Task StartStdoutConsumingAsync(
        IYtDlpProcess process,
        Func<string, CancellationToken, ValueTask>? handler)
    {
        if (handler is null)
        {
            return Task.Run(async () =>
            {
                await foreach (var _ in process.StdoutLines.ReadAllAsync(CancellationToken.None).ConfigureAwait(false))
                {
                }
            }, CancellationToken.None);
        }

        return Task.Run(async () =>
        {
            await foreach (var line in process.StdoutLines.ReadAllAsync(CancellationToken.None).ConfigureAwait(false))
            {
                await handler(line, CancellationToken.None).ConfigureAwait(false);
            }
        }, CancellationToken.None);
    }

    [SuppressMessage(
        "Design",
        "CA1031:Do not catch general exception types",
        Justification = "Reader tasks are awaited only to drain side effects; failures surface via process exit code or thrown exceptions.")]
    private static async Task SafeAwaitAsync(Task task)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch
        {
        }
    }

    private static string JoinStderr(RingBuffer<string> buffer) =>
        string.Join('\n', buffer.Snapshot());

    private static VideoInfo DeserializeVideoInfo(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new YtDlpParsingException("yt-dlp returned no JSON output.");
        }

        try
        {
            var info = JsonSerializer.Deserialize(json, YtDlpJsonContext.Default.VideoInfo);
            return info ?? throw new YtDlpParsingException("yt-dlp returned a null JSON object.");
        }
        catch (JsonException ex)
        {
            throw new YtDlpParsingException("Failed to deserialize yt-dlp JSON output.", ex);
        }
    }

    private static YtDlpOptions ApplyDownloadDefaults(YtDlpOptions options, string outputDirectory)
    {
        if (!string.IsNullOrWhiteSpace(options.Filesystem.Paths))
        {
            return options;
        }

        return options with
        {
            Filesystem = options.Filesystem with { Paths = $"home:{outputDirectory}" }
        };
    }

    private static string RenderAudioFormat(AudioFormat audioFormat) =>
        audioFormat.ToString().ToLowerInvariant();

    private YtDlpProcessStartInfo BuildDownloadStartInfo(
        YtDlpOptions options,
        string outputDirectory,
        string url)
    {
        var rendered = _renderer.Render(options);
        var arguments = new List<string>(rendered.Count + 1);
        arguments.AddRange(rendered);
        arguments.Add(url);

        return new YtDlpProcessStartInfo
        {
            ExecutablePath = _options.YtDlpExecutablePath,
            Arguments = arguments,
            WorkingDirectory = outputDirectory,
            EnvironmentVariables = _options.EnvironmentVariables,
            RawStdoutWriter = _options.StdoutForwardingWriter,
            RawStderrWriter = _options.StderrForwardingWriter
        };
    }

    private YtDlpProcessStartInfo BuildBareStartInfo(IReadOnlyList<string> arguments) => new()
    {
        ExecutablePath = _options.YtDlpExecutablePath,
        Arguments = arguments,
        EnvironmentVariables = _options.EnvironmentVariables,
        RawStdoutWriter = _options.StdoutForwardingWriter,
        RawStderrWriter = _options.StderrForwardingWriter
    };

    private static string BuildCommandSummary(YtDlpProcessStartInfo startInfo)
    {
        var sb = new StringBuilder();
        sb.Append(startInfo.ExecutablePath);
        foreach (var arg in startInfo.Arguments)
        {
            sb.Append(' ').Append(arg);
        }

        return sb.ToString();
    }
}
