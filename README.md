# YtDlpSharpLib

A managed .NET 10 wrapper around the [yt-dlp](https://github.com/yt-dlp/yt-dlp) command-line utility.
The library exposes a strongly-typed `IYtDlpClient`, a reflection-driven argument renderer that
covers every flag in `yt-dlp --help`, an optional binary downloader for yt-dlp/ffmpeg/ffprobe/Deno,
and a small concurrency-limited execution scheduler for batch jobs.

- Targets `net10.0`.
- Async-first; no blocking calls in the public surface.
- Strongly-typed options grouped by yt-dlp help section (`General`, `Network`, `VideoFormat`,
  `PostProcessing`, `Subtitle`, `Authentication`, `SponsorBlock`, …).
- Progress reporting via `IProgress<YtDlpProgress>` or `IAsyncEnumerable<YtDlpProgress>`.
- Typed exceptions (`YtDlpProcessException`, `YtDlpNotFoundException`, `YtDlpValidationException`, …).
- An escape hatch (`RawYtDlpArgument`) for any flag the typed surface does not yet model.

---

## Table of Contents

- [Installation](#installation)
- [Prerequisites](#prerequisites)
- [Quick start (console / non-DI)](#quick-start-console--non-di)
- [Quick start (dependency injection)](#quick-start-dependency-injection)
- [Client API](#client-api)
- [Options model](#options-model)
- [Examples](#examples)
  - [Choosing a format and merge container](#choosing-a-format-and-merge-container)
  - [Audio-only download](#audio-only-download)
  - [Output template and filename rules](#output-template-and-filename-rules)
  - [Playlists](#playlists)
  - [Date and filesize filters](#date-and-filesize-filters)
  - [Authentication, cookies, proxies](#authentication-cookies-proxies)
  - [Subtitles](#subtitles)
  - [Thumbnails and metadata sidecars](#thumbnails-and-metadata-sidecars)
  - [Post-processing: recode, remux, embed](#post-processing-recode-remux-embed)
  - [SponsorBlock](#sponsorblock)
  - [Section downloads and rate limits](#section-downloads-and-rate-limits)
  - [Forwarding raw stdout/stderr](#forwarding-raw-stdoutstderr)
  - [Cancellation](#cancellation)
  - [Advanced (raw) arguments](#advanced-raw-arguments)
- [Progress reporting](#progress-reporting)
- [Batching with the execution scheduler](#batching-with-the-execution-scheduler)
- [Binary provisioning (yt-dlp, ffmpeg, ffprobe, Deno)](#binary-provisioning-yt-dlp-ffmpeg-ffprobe-deno)
- [Exceptions](#exceptions)
- [Configuration reference (`YtDlpClientOptions`)](#configuration-reference-ytdlpclientoptions)

---

## Installation

The library is packaged as `YtDlpSharpLib`. From a project that targets `net10.0`:

```bash
dotnet add package YtDlpSharpLib
```

Or as a project reference while developing locally:

```xml
<ItemGroup>
  <ProjectReference Include="..\YtDlpSharpLib\YtDlpSharpLib.csproj" />
</ItemGroup>
```

## Prerequisites

The library shells out to the real `yt-dlp` binary. You must either:

1. Have `yt-dlp` (and, if you are merging or converting media, `ffmpeg`/`ffprobe`) on `PATH`, or
2. Set `YtDlpClientOptions.YtDlpExecutablePath` to an absolute path, or
3. Use the bundled `IYtDlpBinaryDownloader` to fetch the right binaries at runtime — see
   [Binary provisioning](#binary-provisioning-yt-dlp-ffmpeg-ffprobe-deno).

---

## Quick start (console / non-DI)

`YtDlpClient` has a constructor that takes plain types, so it is usable without any DI container:

```csharp
using YtDlpSharpLib;
using YtDlpSharpLib.Process;
using YtDlpSharpLib.Rendering;

var client = new YtDlpClient(
    new YtDlpClientOptions
    {
        YtDlpExecutablePath = "yt-dlp",   // or an absolute path
        FfmpegExecutablePath = "ffmpeg",
    },
    new YtDlpProcessFactory(),
    new YtDlpArgumentRenderer(),
    TimeProvider.System);

var info = await client.GetVideoInfoAsync("https://www.youtube.com/watch?v=C0DPdy98e4c");
Console.WriteLine($"{info.Title} ({info.Duration}s)");

await client.DownloadAsync(
    "https://www.youtube.com/watch?v=C0DPdy98e4c",
    outputDirectory: Path.Combine(Environment.CurrentDirectory, "downloads"));
```

The four constructor dependencies are intentional — they are the same seams the DI registration
uses, which makes the client easy to test with fakes.

## Quick start (dependency injection)

Register the client (and, optionally, the binary downloader) with `IServiceCollection`:

```csharp
using Microsoft.Extensions.DependencyInjection;
using YtDlpSharpLib;

var services = new ServiceCollection();

services.AddYtDlpClient(opts =>
{
    opts.YtDlpExecutablePath  = "/usr/local/bin/yt-dlp";
    opts.FfmpegExecutablePath = "/usr/local/bin/ffmpeg";
    opts.DownloadConcurrency  = 4;        // used by the scheduler
});

// Optional: register the binary downloader. Pair with HttpClientFactory if you have one.
services.AddYtDlpBinaryDownloader(opts =>
{
    opts.DefaultDirectory = Path.Combine(AppContext.BaseDirectory, "tools");
});

var provider = services.BuildServiceProvider();

var client = provider.GetRequiredService<IYtDlpClient>();
await client.DownloadAsync(
    "https://www.youtube.com/watch?v=C0DPdy98e4c",
    outputDirectory: "downloads");
```

`AddYtDlpClient` registers (all as singletons, all `TryAdd`-style so you can override any of them):

| Service                       | Default implementation        |
|-------------------------------|-------------------------------|
| `IYtDlpClient`                | `YtDlpClient`                 |
| `IYtDlpProcessFactory`        | `YtDlpProcessFactory`         |
| `IYtDlpArgumentRenderer`      | `YtDlpArgumentRenderer`       |
| `IYtDlpExecutionScheduler`    | `YtDlpExecutionScheduler`     |
| `TimeProvider`                | `TimeProvider.System`         |

`AddYtDlpBinaryDownloader` registers `IYtDlpBinaryDownloader` and will use the container's
`HttpClient` if one is registered (e.g. via `services.AddHttpClient<YtDlpBinaryDownloader>()`).
Otherwise the downloader creates and disposes its own.

---

## Client API

`IYtDlpClient` is the entire public download/inspect surface:

| Method                                                                                         | Maps to                                              |
|------------------------------------------------------------------------------------------------|------------------------------------------------------|
| `GetVideoInfoAsync(url, ct)`                                                                   | `--dump-single-json --no-playlist`                   |
| `GetPlaylistInfoAsync(url, ct)`                                                                | `--dump-json --yes-playlist --ignore-no-formats-error` (streamed) |
| `DownloadAsync(url, outputDirectory, options, progress, ct)`                                   | a regular yt-dlp invocation                          |
| `DownloadWithProgressAsync(url, outputDirectory, options, ct)`                                 | the same, exposed as `IAsyncEnumerable<YtDlpProgress>` |
| `DownloadAudioAsync(url, outputDirectory, options, progress, ct)`                              | `-x --audio-format <fmt>`                            |
| `DownloadPlaylistAsync(url, outputDirectory, options, progress, ct)`                           | `--yes-playlist [--playlist-items …]`                |
| `DownloadAudioPlaylistAsync(url, outputDirectory, options, progress, ct)`                      | the union of the previous two                        |
| `DownloadMetadataAsync(url, outputDirectory, options, ct)`                                     | `--write-info-json --skip-download` (+ thumb/subs)   |
| `DownloadLiveChatAsync(url, outputDirectory, options, ct)`                                     | `--write-subs --sub-langs live_chat --skip-download` |
| `GetVersionAsync(ct)`                                                                          | `--version`                                          |

`outputDirectory` is mandatory for every download method. If you do not set
`YtDlpFilesystemOptions.Paths` yourself, the library will set it to `home:<outputDirectory>` so the
files land where you said they should.

## Options model

Everything you can pass on the yt-dlp command line is grouped on `YtDlpOptions`:

```csharp
var ytDlp = new YtDlpOptions
{
    General           = new YtDlpGeneralOptions           { /* ... */ },
    Network           = new YtDlpNetworkOptions           { /* ... */ },
    GeoRestriction    = new YtDlpGeoRestrictionOptions    { /* ... */ },
    VideoSelection    = new YtDlpVideoSelectionOptions    { /* ... */ },
    Download          = new YtDlpDownloadOptions          { /* ... */ },
    Filesystem        = new YtDlpFilesystemOptions        { /* ... */ },
    Thumbnail         = new YtDlpThumbnailOptions         { /* ... */ },
    InternetShortcut  = new YtDlpInternetShortcutOptions  { /* ... */ },
    VerbositySimulation = new YtDlpVerbositySimulationOptions { /* ... */ },
    Workarounds       = new YtDlpWorkaroundsOptions       { /* ... */ },
    VideoFormat       = new YtDlpVideoFormatOptions       { /* ... */ },
    Subtitle          = new YtDlpSubtitleOptions          { /* ... */ },
    Authentication    = new YtDlpAuthenticationOptions    { /* ... */ },
    PostProcessing    = new YtDlpPostProcessingOptions    { /* ... */ },
    SponsorBlock      = new YtDlpSponsorBlockOptions      { /* ... */ },
    Extractor         = new YtDlpExtractorOptions         { /* ... */ },
    AdvancedArguments = [ /* RawYtDlpArgument(...) */ ],
};
```

Every group is a `record` with `init`-only properties, so it composes well with `with`-expressions
and is safe to share across calls.

---

## Examples

### Choosing a format and merge container

```csharp
await client.DownloadAsync(
    "https://www.youtube.com/watch?v=C0DPdy98e4c",
    outputDirectory: "downloads",
    new DownloadOptions
    {
        YtDlp = new YtDlpOptions
        {
            VideoFormat = new YtDlpVideoFormatOptions
            {
                Format            = "bestvideo[ext=mp4]+bestaudio[ext=m4a]/best[ext=mp4]/best",
                MergeOutputFormat = DownloadMergeFormat.Mkv,
            }
        }
    });
```

### Audio-only download

```csharp
await client.DownloadAudioAsync(
    "https://www.youtube.com/watch?v=C0DPdy98e4c",
    outputDirectory: "downloads",
    new AudioDownloadOptions
    {
        AudioFormat = AudioConversionFormat.Mp3,
        YtDlp = new YtDlpOptions
        {
            PostProcessing = new YtDlpPostProcessingOptions
            {
                AudioQuality = "0",       // VBR best
                EmbedThumbnail = true,
                EmbedMetadata  = true,
            }
        }
    });
```

`AudioConversionFormat` covers `Best`, `Aac`, `Alac`, `Flac`, `M4a`, `Mp3`, `Opus`, `Vorbis`, `Wav`.

### Output template and filename rules

```csharp
var ytDlp = new YtDlpOptions
{
    Filesystem = new YtDlpFilesystemOptions
    {
        // Standard yt-dlp template syntax.
        Output             = "%(extractor)s_%(title)s_%(upload_date)s.%(ext)s",
        RestrictFilenames  = true,
        WindowsFilenames   = true,
        TrimFilenames      = 200,
        NoOverwrites       = true,
    }
};
```

If `Filesystem.Paths` is not set explicitly, the client auto-populates it with
`home:<outputDirectory>` so the rendered template resolves under the directory you passed to
`DownloadAsync`.

### Playlists

```csharp
// All entries.
await client.DownloadPlaylistAsync(
    "https://www.youtube.com/playlist?list=PLAYLIST_ID",
    outputDirectory: "downloads",
    new PlaylistDownloadOptions
    {
        PlaylistItems = "1-5,8,10-",      // yt-dlp playlist-items selector
        YtDlp = new YtDlpOptions
        {
            VideoSelection = new YtDlpVideoSelectionOptions { MaxDownloads = 25 },
        }
    });

// Or as audio:
await client.DownloadAudioPlaylistAsync(
    "https://www.youtube.com/playlist?list=PLAYLIST_ID",
    outputDirectory: "downloads",
    new AudioPlaylistDownloadOptions
    {
        AudioFormat  = AudioConversionFormat.Opus,
        PlaylistItems = "1-10",
    });

// Stream metadata for each entry without downloading anything:
await foreach (var entry in client.GetPlaylistInfoAsync("https://...playlist..."))
{
    Console.WriteLine($"{entry.Id} - {entry.Title}");
}
```

### Date and filesize filters

```csharp
var ytDlp = new YtDlpOptions
{
    VideoSelection = new YtDlpVideoSelectionOptions
    {
        Dateafter    = "20240101",   // YYYYMMDD
        Datebefore   = "20241231",
        MinFilesize  = "10M",
        MaxFilesize  = "2G",
        AgeLimit     = 18,
        DownloadArchive = "archive.txt",
    }
};
```

### Authentication, cookies, proxies

```csharp
var ytDlp = new YtDlpOptions
{
    Network = new YtDlpNetworkOptions
    {
        Proxy         = "socks5://127.0.0.1:1080",
        SocketTimeout = 15.0,
        SourceAddress = "0.0.0.0",
    },
    Authentication = new YtDlpAuthenticationOptions
    {
        Username      = "me@example.test",
        Password      = "hunter2",
        Twofactor     = "123456",
        VideoPassword = "secret",
    },
    AdvancedArguments =
    [
        // Cookies are intentionally not in the typed surface; treat as sensitive.
        new RawYtDlpArgument
        {
            Name        = "--cookies",
            Value       = "/path/to/cookies.txt",
            IsSensitive = true,
        }
    ]
};
```

### Subtitles

```csharp
var ytDlp = new YtDlpOptions
{
    Subtitle = new YtDlpSubtitleOptions
    {
        WriteSubs     = true,
        WriteAutoSubs = true,
        SubLangs      = "en,ja,es",
        SubFormat     = SubtitleFormat.Srt,
    },
    PostProcessing = new YtDlpPostProcessingOptions
    {
        ConvertSubs = SubtitleFormat.Vtt,
        EmbedSubs   = true,
    }
};
```

### Thumbnails and metadata sidecars

```csharp
// Just the metadata, no media:
await client.DownloadMetadataAsync(
    "https://...",
    "downloads",
    new MetadataDownloadOptions
    {
        WriteThumbnail    = true,
        WriteSubtitles    = true,
        SubtitleLanguages = "en,ja",
        YtDlp = new YtDlpOptions
        {
            Filesystem = new YtDlpFilesystemOptions { Output = "%(id)s.%(ext)s" }
        }
    });

// Or alongside a real download:
var ytDlp = new YtDlpOptions
{
    Thumbnail  = new YtDlpThumbnailOptions  { WriteThumbnail = true },
    Filesystem = new YtDlpFilesystemOptions { WriteInfoJson  = true, WriteDescription = true },
};
```

### Post-processing: recode, remux, embed

```csharp
var ytDlp = new YtDlpOptions
{
    PostProcessing = new YtDlpPostProcessingOptions
    {
        RemuxVideo       = VideoContainer.Mkv,            // --remux-video mkv
        RecodeVideo      = VideoRecodeFormat.Mp4,         // --recode-video mp4
        EmbedSubs        = true,
        EmbedThumbnail   = true,
        EmbedMetadata    = true,
        EmbedChapters    = true,
        FfmpegLocation   = "/usr/local/bin",              // directory or executable path
    }
};
```

### SponsorBlock

```csharp
var ytDlp = new YtDlpOptions
{
    SponsorBlock = new YtDlpSponsorBlockOptions
    {
        SponsorblockMark   = "all",
        SponsorblockRemove = "sponsor,selfpromo",
    }
};
```

### Section downloads and rate limits

```csharp
var ytDlp = new YtDlpOptions
{
    Download = new YtDlpDownloadOptions
    {
        DownloadSections    = ["*0-30", "*60-90"],   // first 30s and 60-90s
        LimitRate           = "5M",                  // bytes/sec, K/M/G allowed
        ConcurrentFragments = 4,
        Retries             = "infinite",
        FragmentRetries     = "10",
    }
};
```

### Forwarding raw stdout/stderr

For verbose console apps, plumb yt-dlp's raw output straight through:

```csharp
var opts = new YtDlpClientOptions
{
    StdoutForwardingWriter = Console.Out,
    StderrForwardingWriter = Console.Error,
};
```

Each line read from yt-dlp is mirrored to the writer in addition to being parsed/queued. This is
independent of progress reporting — you can use either, both, or neither.

### Cancellation

Every async method takes a `CancellationToken`. Cancellation requests trigger a graceful kill of
the yt-dlp child process; if the process does not exit within
`YtDlpClientOptions.TerminationGracePeriod` (default 5s), the entire process tree is force-killed.

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(2));
await client.DownloadAsync(url, "downloads", ct: cts.Token);
```

### Advanced (raw) arguments

Anything not yet modelled in the typed surface — niche flags, brand-new yt-dlp features, sensitive
values you would rather not log — can be appended via `AdvancedArguments`:

```csharp
var ytDlp = new YtDlpOptions
{
    AdvancedArguments =
    [
        new RawYtDlpArgument { Name = "--no-mtime" },
        new RawYtDlpArgument { Name = "--user-agent", Value = "MyApp/1.0" },
        new RawYtDlpArgument
        {
            Name   = "--external-downloader-args",
            Value  = "aria2c:-x16 -k1M",
        },
        new RawYtDlpArgument
        {
            Name        = "--cookies-from-browser",
            Value       = "firefox",
            IsSensitive = true,    // hint for your own logging
        },
    ]
};
```

Raw arguments must start with `--` (long-form). Renderer validation throws
`YtDlpValidationException` otherwise.

---

## Progress reporting

There are two progress styles. Pick one — they are equivalent.

### Callback (`IProgress<YtDlpProgress>`)

```csharp
var progress = new Progress<YtDlpProgress>(p =>
{
    if (p.Phase == ProgressPhase.Downloading && p.Percent is { } pct)
    {
        Console.WriteLine($"[download] {pct,6:F1}%   {p.Speed}   ETA {p.Eta}");
    }
});

await client.DownloadAsync(url, "downloads", progress: progress);
```

### Async stream (`IAsyncEnumerable<YtDlpProgress>`)

```csharp
await foreach (var p in client.DownloadWithProgressAsync(url, "downloads"))
{
    switch (p.Phase)
    {
        case ProgressPhase.Downloading:    /* p.Percent / p.TotalBytes / p.Speed */ break;
        case ProgressPhase.Merging:        /* ffmpeg merging */                     break;
        case ProgressPhase.ExtractingAudio:/* audio extraction */                   break;
        case ProgressPhase.Converting:     /* recoding */                           break;
        case ProgressPhase.EmbeddingThumbnail: /* */                                break;
        case ProgressPhase.PostProcessing: /* */                                    break;
        case ProgressPhase.Completed:      /* */                                    break;
    }
}
```

> yt-dlp by default rewrites the progress line in place. If you want frequent updates, set
> `VerbositySimulation = new YtDlpVerbositySimulationOptions { Newline = true }` so each progress
> tick is its own line.

`YtDlpProgress` exposes `Percent`, `DownloadedBytes`, `TotalBytes`, `Speed` (e.g. `"2.50MiB/s"`),
`Eta`, an `AdditionalInfo` field for non-download phases, and the `RawLine` for diagnostics.

---

## Batching with the execution scheduler

Use `IYtDlpExecutionScheduler` when you have a queue of independent downloads and want to bound
concurrency:

```csharp
var scheduler = provider.GetRequiredService<IYtDlpExecutionScheduler>();

var requests = new[]
{
    new DownloadRequest { Url = "https://example.test/a", OutputDirectory = "downloads" },
    new DownloadRequest { Url = "https://example.test/b", OutputDirectory = "downloads" },
    new DownloadRequest
    {
        Url             = "https://example.test/c",
        OutputDirectory = "downloads",
        Options         = new DownloadOptions
        {
            YtDlp = new YtDlpOptions
            {
                VideoFormat = new YtDlpVideoFormatOptions { Format = "bestaudio" }
            }
        }
    },
};

await foreach (var result in scheduler.ExecuteBulkAsync(requests))
{
    if (result.Error is null)
    {
        Console.WriteLine($"OK   {result.Url} (exit {result.ExitCode})");
    }
    else
    {
        Console.WriteLine($"FAIL {result.Url}: {result.Error.Message}");
    }
}
```

The concurrency limit comes from `YtDlpClientOptions.DownloadConcurrency` (default `2`). Failures
of individual jobs are surfaced via `DownloadResult.Error`; they do not stop sibling jobs.

---

## Binary provisioning (yt-dlp, ffmpeg, ffprobe, Deno)

`IYtDlpBinaryDownloader` fetches the right release asset for the current OS/architecture, streams
the download with progress reporting, and atomically writes the final file (`.download` staging
file + `File.Move`). It is safe to use as a singleton.

```csharp
using YtDlpSharpLib.Provisioning;

using var downloader = new YtDlpBinaryDownloader(new YtDlpBinaryDownloaderOptions
{
    DefaultDirectory = Path.Combine(AppContext.BaseDirectory, "tools"),
});

// One-shot — grab everything you need in a single call:
var result = await downloader.DownloadAllAsync(
    new BinaryDownloadOptions
    {
        Directory       = "tools",
        SkipExisting    = true,
        DownloadYtDlp   = true,
        DownloadFfmpeg  = true,
        DownloadFfprobe = true,
        DownloadDeno    = false,
    },
    progress: new Progress<BinaryDownloadProgress>(p =>
        Console.WriteLine($"{p.Kind}: {p.BytesReceived}/{p.TotalBytes ?? 0}")));

Console.WriteLine($"yt-dlp:  {result.YtDlpPath}");
Console.WriteLine($"ffmpeg:  {result.FfmpegPath}");
Console.WriteLine($"ffprobe: {result.FfprobePath}");
```

Or fetch a single binary:

```csharp
var path = await downloader.DownloadYtDlpAsync(
    directory: "tools",
    progress:  new Progress<BinaryDownloadProgress>(/* … */));
```

Supported platforms (per binary) match what the upstream projects publish:

| Binary  | Source                                                    |
|---------|-----------------------------------------------------------|
| yt-dlp  | `https://github.com/yt-dlp/yt-dlp/releases/latest/...`    |
| ffmpeg  | `https://ffbinaries.com/api/v1/version/latest`            |
| ffprobe | `https://ffbinaries.com/api/v1/version/latest`            |
| Deno    | `https://dl.deno.land/release-latest.txt` + zip release   |

All URLs and the user-agent are overridable via `YtDlpBinaryDownloaderOptions`.

When registered via `AddYtDlpBinaryDownloader`, the downloader will prefer an `HttpClient`
provided by the container; otherwise it creates and disposes its own (subject to `HttpTimeout`).

---

## Exceptions

All exceptions thrown by the library derive from `YtDlpException`, which carries the (sanitized)
command line and exit code where applicable.

| Exception                          | When it is thrown                                                  |
|------------------------------------|--------------------------------------------------------------------|
| `YtDlpNotFoundException`           | The `yt-dlp` executable could not be located.                      |
| `YtDlpProcessException`            | The yt-dlp process exited with a non-zero exit code. Carries `LastStderrLines`. |
| `YtDlpUnavailableException`        | yt-dlp reported a known-bad video state (geo block, removed, …).   |
| `YtDlpValidationException`         | Bad option values detected before launching the process.           |
| `YtDlpParsingException`            | yt-dlp output (JSON metadata, progress) could not be parsed.       |
| `YtDlpBinaryDownloadException`     | A binary download/extract failed; exposes the offending `Url`.     |

```csharp
try
{
    await client.DownloadAsync(url, "downloads");
}
catch (YtDlpProcessException ex)
{
    Console.Error.WriteLine($"yt-dlp exited with {ex.ExitCode}: {ex.Message}");
    Console.Error.WriteLine(ex.LastStderrLines);
}
catch (YtDlpNotFoundException ex)
{
    Console.Error.WriteLine($"Install yt-dlp or set YtDlpExecutablePath. Tried: {ex.AttemptedPath}");
}
```

---

## Configuration reference (`YtDlpClientOptions`)

| Property                   | Default          | Purpose                                                                 |
|----------------------------|------------------|-------------------------------------------------------------------------|
| `YtDlpExecutablePath`      | `"yt-dlp"`       | Path to the yt-dlp binary (resolved against `PATH` if relative).        |
| `FfmpegExecutablePath`     | `"ffmpeg"`       | Path to ffmpeg, used by yt-dlp for merge/convert.                       |
| `DownloadConcurrency`      | `2`              | Max concurrent downloads for `IYtDlpExecutionScheduler`.                |
| `TerminationGracePeriod`   | `5s`             | Grace given to yt-dlp after a graceful kill before force-killing tree.  |
| `StderrTailLineCount`      | `100`            | Lines of stderr retained for non-zero-exit error reporting.             |
| `StdoutForwardingWriter`   | `null`           | Mirror yt-dlp stdout to this `TextWriter` (e.g. `Console.Out`).         |
| `StderrForwardingWriter`   | `null`           | Mirror yt-dlp stderr to this `TextWriter`.                              |
| `EnvironmentVariables`     | empty            | Extra env vars passed to the yt-dlp child process.                      |

For development, build, and contribution guidance — including the option-generator tooling — see
[`DEVELOPMENT.md`](./DEVELOPMENT.md).
