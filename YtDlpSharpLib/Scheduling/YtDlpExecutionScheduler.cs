using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using YtDlpSharpLib.Downloads;
using YtDlpSharpLib.Exceptions;
using YtDlpSharpLib.Progress;

namespace YtDlpSharpLib.Scheduling;

/// <summary>
/// Default <see cref="IYtDlpExecutionScheduler"/>. Throttles concurrent downloads with a
/// <see cref="SemaphoreSlim"/> sized from <see cref="YtDlpClientOptions.DownloadConcurrency"/>.
/// </summary>
public sealed class YtDlpExecutionScheduler : IYtDlpExecutionScheduler, IDisposable
{
    private readonly IYtDlpClient _client;
    private readonly SemaphoreSlim _semaphore;

    /// <summary>Creates a scheduler from typed options. Suitable for direct (non-DI) usage.</summary>
    public YtDlpExecutionScheduler(IYtDlpClient client, YtDlpClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(options);

        _client = client;
        var concurrency = Math.Max(1, options.DownloadConcurrency);
        _semaphore = new SemaphoreSlim(concurrency, concurrency);
    }

    /// <summary>Creates a scheduler from <see cref="IOptions{TOptions}"/>. Used by the DI container.</summary>
    public YtDlpExecutionScheduler(IYtDlpClient client, IOptions<YtDlpClientOptions> options)
        : this(client, (options ?? throw new ArgumentNullException(nameof(options))).Value)
    {
    }

    /// <inheritdoc />
    public async Task DownloadAsync(
        string url,
        string outputDirectory,
        DownloadOptions? options = null,
        IProgress<YtDlpProgress>? progress = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);

        await _semaphore.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            await _client.DownloadAsync(url, outputDirectory, options, progress, ct).ConfigureAwait(false);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<DownloadResult> ExecuteBulkAsync(
        IEnumerable<DownloadRequest> requests,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(requests);

        var pending = new List<Task<DownloadResult>>();
        foreach (var request in requests)
        {
            ct.ThrowIfCancellationRequested();
            pending.Add(RunWithSemaphoreAsync(request, ct));
        }

        await foreach (var completed in Task.WhenEach(pending).WithCancellation(ct).ConfigureAwait(false))
        {
            yield return await completed.ConfigureAwait(false);
        }
    }

    [SuppressMessage(
        "Design",
        "CA1031:Do not catch general exception types",
        Justification = "Per-job exceptions are surfaced via DownloadResult.Error per the scheduler contract.")]
    private async Task<DownloadResult> RunWithSemaphoreAsync(DownloadRequest request, CancellationToken ct)
    {
        await _semaphore.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            await _client.DownloadAsync(request.Url, request.OutputDirectory, request.Options, null, ct).ConfigureAwait(false);
            return new DownloadResult { Url = request.Url, ExitCode = 0 };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var exitCode = (ex as YtDlpException)?.ExitCode;
            return new DownloadResult
            {
                Url = request.Url,
                Error = ex,
                ExitCode = exitCode
            };
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <inheritdoc />
    public void Dispose() => _semaphore.Dispose();
}
