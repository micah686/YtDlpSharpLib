using YtDlpSharpLib.Downloads;
using YtDlpSharpLib.Options;
using YtDlpSharpLib.Process;
using YtDlpSharpLib.Provisioning;
using YtDlpSharpLib.Rendering;

namespace YtDlpSharpLib.IntegrationTests;

[Category("Integration")]
[NotInParallel]
public sealed class DownloadIntegrationTests
{
    private const string VimeoUrl = "https://vimeo.com/1084537";
    private const string YouTubeUrl = "https://www.youtube.com/watch?v=C0DPdy98e4c";

    [Test]
    public async Task GetVideoInfoAsync_ReturnsMetadataForVimeoVideo()
    {
        var environment = await IntegrationTestEnvironment.GetAsync();
        await using var workspace = TemporaryWorkspace.Create();

        var client = environment.CreateClient();
        var info = await client.GetVideoInfoAsync(VimeoUrl);

        await Assert.That(info.Id).IsEqualTo("1084537");
        await Assert.That(info.Title).IsNotEmpty();
        await Assert.That(info.Extractor).Contains("vimeo", StringComparison.OrdinalIgnoreCase);
        await Assert.That(info.WebpageUrl).IsEqualTo(VimeoUrl);
        await Assert.That(info.Formats).IsNotEmpty();
    }

    [Test]
    public async Task GetVideoInfoAsync_ReturnsMetadataForBluegramsYouTubeVideo()
    {
        var environment = await IntegrationTestEnvironment.GetAsync();
        await using var workspace = TemporaryWorkspace.Create();

        var client = environment.CreateClient();
        var info = await client.GetVideoInfoAsync(YouTubeUrl);

        await Assert.That(info.Id).IsEqualTo("C0DPdy98e4c");
        await Assert.That(info.Title).IsEqualTo("TEST VIDEO");
        await Assert.That(info.Extractor).Contains("youtube", StringComparison.OrdinalIgnoreCase);
        await Assert.That(info.UploadDate).IsEqualTo("20070221");
        await Assert.That(info.ParsedUploadDate).IsEqualTo(new DateOnly(2007, 2, 21));
        await Assert.That(info.Formats).IsNotEmpty();
    }

    [Test]
    public async Task DownloadAsync_DownloadsVideoFile()
    {
        var environment = await IntegrationTestEnvironment.GetAsync();
        await using var workspace = TemporaryWorkspace.Create();

        var client = environment.CreateClient();
        await client.DownloadAsync(
            VimeoUrl,
            workspace.Path,
            new DownloadOptions
            {
                YtDlp = environment.WithFfmpeg(new YtDlpOptions
                {
                    Filesystem = new YtDlpFilesystemOptions
                    {
                        Output = "%(id)s.%(ext)s"
                    },
                    Download = ShortSection,
                    VideoFormat = new YtDlpVideoFormatOptions
                    {
                        Format = "best[ext=mp4]/best",
                        MergeOutputFormat = DownloadMergeFormat.Mp4
                    }
                })
            });

        var file = await workspace.SingleMediaFileAsync();
        await Assert.That(File.Exists(file)).IsTrue();
        await Assert.That(Path.GetExtension(file)).IsEqualTo(".mp4");
        await Assert.That(Path.GetFileNameWithoutExtension(file)).IsEqualTo("1084537");
    }

    [Test]
    public async Task DownloadAudioAsync_ExtractsMp3()
    {
        var environment = await IntegrationTestEnvironment.GetAsync();
        await using var workspace = TemporaryWorkspace.Create();

        var client = environment.CreateClient();
        await client.DownloadAudioAsync(
            VimeoUrl,
            workspace.Path,
            new AudioDownloadOptions
            {
                AudioFormat = AudioConversionFormat.Mp3,
                YtDlp = environment.WithFfmpeg(new YtDlpOptions
                {
                    Filesystem = new YtDlpFilesystemOptions
                    {
                        Output = "%(id)s.%(ext)s"
                    },
                    Download = ShortSection
                })
            });

        var file = await workspace.SingleMediaFileAsync();
        await Assert.That(File.Exists(file)).IsTrue();
        await Assert.That(Path.GetExtension(file)).IsEqualTo(".mp3");
        await Assert.That(Path.GetFileNameWithoutExtension(file)).IsEqualTo("1084537");
    }

    [Test]
    public async Task DownloadAsync_UsesOutputTemplateRestrictFilenamesAndRecodeFormat()
    {
        var environment = await IntegrationTestEnvironment.GetAsync();
        await using var workspace = TemporaryWorkspace.Create();

        var client = environment.CreateClient();
        await client.DownloadAsync(
            VimeoUrl,
            workspace.Path,
            new DownloadOptions
            {
                YtDlp = environment.WithFfmpeg(new YtDlpOptions
                {
                    Filesystem = new YtDlpFilesystemOptions
                    {
                        Output = "%(extractor)s_%(id)s.%(ext)s",
                        RestrictFilenames = true
                    },
                    Download = ShortSection,
                    VideoFormat = new YtDlpVideoFormatOptions
                    {
                        Format = "bestvideo[ext=mp4]+bestaudio[ext=m4a]/best[ext=mp4]/best"
                    },
                    PostProcessing = new YtDlpPostProcessingOptions
                    {
                        RecodeVideo = VideoRecodeFormat.Mp4
                    }
                })
            });

        var file = await workspace.SingleMediaFileAsync();
        await Assert.That(File.Exists(file)).IsTrue();
        await Assert.That(Path.GetExtension(file)).IsEqualTo(".mp4");
        await Assert.That(Path.GetFileNameWithoutExtension(file)).IsEqualTo("vimeo_1084537");
    }

    [Test]
    public async Task DownloadMetadataAsync_WritesInfoJsonWithoutMediaFile()
    {
        var environment = await IntegrationTestEnvironment.GetAsync();
        await using var workspace = TemporaryWorkspace.Create();

        var client = environment.CreateClient();
        await client.DownloadMetadataAsync(
            VimeoUrl,
            workspace.Path,
            new MetadataDownloadOptions
            {
                YtDlp = new YtDlpOptions
                {
                    Filesystem = new YtDlpFilesystemOptions
                    {
                        Output = "%(id)s.%(ext)s"
                    }
                }
            },
            CancellationToken.None);

        var infoJson = Directory.GetFiles(workspace.Path, "*.info.json", SearchOption.TopDirectoryOnly);
        await Assert.That(infoJson).Count().IsEqualTo(1);
        await Assert.That(Directory.GetFiles(workspace.Path, "*.mp4", SearchOption.TopDirectoryOnly)).IsEmpty();
        await Assert.That(Directory.GetFiles(workspace.Path, "*.webm", SearchOption.TopDirectoryOnly)).IsEmpty();
    }

    [Test]
    public async Task DownloadAsync_DownloadsBluegramsYouTubeVideoToMkv()
    {
        var environment = await IntegrationTestEnvironment.GetAsync();
        await using var workspace = TemporaryWorkspace.Create();

        var client = environment.CreateClient();
        await client.DownloadAsync(
            YouTubeUrl,
            workspace.Path,
            new DownloadOptions
            {
                YtDlp = environment.WithFfmpeg(new YtDlpOptions
                {
                    Filesystem = new YtDlpFilesystemOptions
                    {
                        Output = "%(title)s.%(ext)s"
                    },
                    Download = ShortSection,
                    VideoFormat = new YtDlpVideoFormatOptions
                    {
                        Format = "bestvideo[ext=mp4]+bestaudio[ext=m4a]/best[ext=mp4]/best",
                        MergeOutputFormat = DownloadMergeFormat.Mkv
                    }
                })
            });

        var file = await workspace.SingleMediaFileAsync();
        await Assert.That(File.Exists(file)).IsTrue();
        await Assert.That(Path.GetExtension(file)).IsEqualTo(".mkv");
        await Assert.That(Path.GetFileNameWithoutExtension(file)).IsEqualTo("TEST VIDEO");
    }

    [Test]
    public async Task DownloadAudioAsync_ExtractsBluegramsYouTubeMp3()
    {
        var environment = await IntegrationTestEnvironment.GetAsync();
        await using var workspace = TemporaryWorkspace.Create();

        var client = environment.CreateClient();
        await client.DownloadAudioAsync(
            YouTubeUrl,
            workspace.Path,
            new AudioDownloadOptions
            {
                AudioFormat = AudioConversionFormat.Mp3,
                YtDlp = environment.WithFfmpeg(new YtDlpOptions
                {
                    Filesystem = new YtDlpFilesystemOptions
                    {
                        Output = "%(title)s.%(ext)s"
                    },
                    Download = ShortSection
                })
            });

        var file = await workspace.SingleMediaFileAsync();
        await Assert.That(File.Exists(file)).IsTrue();
        await Assert.That(Path.GetExtension(file)).IsEqualTo(".mp3");
        await Assert.That(Path.GetFileNameWithoutExtension(file)).IsEqualTo("TEST VIDEO");
    }

    [Test]
    public async Task DownloadAsync_UsesBluegramsYouTubeOutputTemplateAndRecodeFormat()
    {
        var environment = await IntegrationTestEnvironment.GetAsync();
        await using var workspace = TemporaryWorkspace.Create();

        var client = environment.CreateClient();
        await client.DownloadAsync(
            YouTubeUrl,
            workspace.Path,
            new DownloadOptions
            {
                YtDlp = environment.WithFfmpeg(new YtDlpOptions
                {
                    Filesystem = new YtDlpFilesystemOptions
                    {
                        Output = "%(extractor)s_%(title)s_%(upload_date)s.%(ext)s",
                        RestrictFilenames = true
                    },
                    Download = ShortSection,
                    VideoFormat = new YtDlpVideoFormatOptions
                    {
                        Format = "bestvideo[ext=mp4]+bestaudio[ext=m4a]/best[ext=mp4]/best"
                    },
                    PostProcessing = new YtDlpPostProcessingOptions
                    {
                        RecodeVideo = VideoRecodeFormat.Mp4
                    }
                })
            });

        var file = await workspace.SingleMediaFileAsync();
        await Assert.That(File.Exists(file)).IsTrue();
        await Assert.That(Path.GetExtension(file)).IsEqualTo(".mp4");
        await Assert.That(Path.GetFileNameWithoutExtension(file)).IsEqualTo("youtube_TEST_VIDEO_20070221");
    }

    private sealed class IntegrationTestEnvironment
    {
        private const string YtDlpPathEnvVar = "YTDLP_INTEGRATION_YTDLP_PATH";
        private const string FfmpegPathEnvVar = "YTDLP_INTEGRATION_FFMPEG_PATH";

        private static readonly SemaphoreSlim Gate = new(1, 1);
        private static IntegrationTestEnvironment? _current;

        private IntegrationTestEnvironment(string ytDlpPath, string? ffmpegLocation)
        {
            YtDlpPath = ytDlpPath;
            FfmpegLocation = ffmpegLocation;
        }

        public string YtDlpPath { get; }

        public string? FfmpegLocation { get; }

        public static async Task<IntegrationTestEnvironment> GetAsync()
        {
            if (_current is not null)
            {
                return _current;
            }

            await Gate.WaitAsync(CancellationToken.None);
            try
            {
                if (_current is not null)
                {
                    return _current;
                }

                _current = await ResolveAsync(CancellationToken.None);
                return _current;
            }
            finally
            {
                Gate.Release();
            }
        }

        public YtDlpClient CreateClient() =>
            new(
                new YtDlpClientOptions { YtDlpExecutablePath = YtDlpPath },
                new YtDlpProcessFactory(),
                new YtDlpArgumentRenderer(),
                TimeProvider.System);

        public YtDlpOptions WithFfmpeg(YtDlpOptions options)
        {
            if (string.IsNullOrWhiteSpace(FfmpegLocation))
            {
                return options;
            }

            return options with
            {
                PostProcessing = options.PostProcessing with
                {
                    FfmpegLocation = FfmpegLocation
                }
            };
        }

        private static async Task<IntegrationTestEnvironment> ResolveAsync(CancellationToken ct)
        {
            var configuredYtDlp = Environment.GetEnvironmentVariable(YtDlpPathEnvVar);
            var configuredFfmpeg = Environment.GetEnvironmentVariable(FfmpegPathEnvVar);
            var ffmpegPath = !string.IsNullOrWhiteSpace(configuredFfmpeg)
                ? configuredFfmpeg
                : FindExecutableOnPath("ffmpeg");
            if (!string.IsNullOrWhiteSpace(configuredYtDlp))
            {
                return new IntegrationTestEnvironment(
                    configuredYtDlp,
                    ResolveFfmpegLocation(ffmpegPath));
            }

            var pathYtDlp = FindExecutableOnPath("yt-dlp");
            if (!string.IsNullOrWhiteSpace(pathYtDlp))
            {
                return new IntegrationTestEnvironment(
                    pathYtDlp,
                    ResolveFfmpegLocation(ffmpegPath));
            }

            var binaryDirectory = Path.Combine(Path.GetTempPath(), "YtDlpSharpLib.IntegrationTests", "binaries");
            try
            {
                using var downloader = new YtDlpBinaryDownloader(new YtDlpBinaryDownloaderOptions());
                var result = await downloader.DownloadAllAsync(
                    new BinaryDownloadOptions
                    {
                        Directory = binaryDirectory,
                        DownloadDeno = false,
                        DownloadFfmpeg = ffmpegPath is null,
                        DownloadFfprobe = false,
                        SkipExisting = true
                    },
                    ct: ct);

                if (string.IsNullOrWhiteSpace(result.YtDlpPath))
                {
                    Skip.Test("yt-dlp could not be resolved or downloaded for integration tests.");
                }

                return new IntegrationTestEnvironment(
                    result.YtDlpPath!,
                    ResolveFfmpegLocation(ffmpegPath ?? result.FfmpegPath));
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                Skip.Test(
                    $"yt-dlp integration tests require network access or {YtDlpPathEnvVar}/{FfmpegPathEnvVar}. " +
                    $"Binary setup failed: {ex.Message}");
                throw;
            }
        }

        private static string? ResolveFfmpegLocation(string? ffmpegPath)
        {
            if (string.IsNullOrWhiteSpace(ffmpegPath))
            {
                return null;
            }

            return Path.GetDirectoryName(Path.GetFullPath(ffmpegPath));
        }

        private static string? FindExecutableOnPath(string fileName)
        {
            var path = Environment.GetEnvironmentVariable("PATH");
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            var extensions = OperatingSystem.IsWindows()
                ? (Environment.GetEnvironmentVariable("PATHEXT") ?? ".EXE;.CMD;.BAT;.COM")
                    .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                : [string.Empty];

            foreach (var directory in path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                foreach (var extension in extensions)
                {
                    var candidate = Path.Combine(directory, fileName + extension);
                    if (File.Exists(candidate))
                    {
                        return candidate;
                    }
                }
            }

            return null;
        }
    }

    private static YtDlpDownloadOptions ShortSection => new()
    {
        DownloadSections = ["*0-5"]
    };

    private sealed class TemporaryWorkspace : IAsyncDisposable
    {
        private static readonly HashSet<string> IgnoredExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".description",
            ".json",
            ".part",
            ".temp",
            ".tmp",
            ".ytdl"
        };

        private TemporaryWorkspace(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public static TemporaryWorkspace Create()
        {
            var path = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                "YtDlpSharpLib.IntegrationTests",
                Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(path);
            return new TemporaryWorkspace(path);
        }

        public async Task<string> SingleMediaFileAsync()
        {
            var files = Directory.GetFiles(Path, "*", SearchOption.TopDirectoryOnly)
                .Where(file => !IgnoredExtensions.Contains(System.IO.Path.GetExtension(file)))
                .Where(file => !file.EndsWith(".info.json", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            await Assert.That(files).Count().IsEqualTo(1);
            return files[0];
        }

        public ValueTask DisposeAsync()
        {
            if (Directory.Exists(Path))
            {
                Directory.Delete(Path, recursive: true);
            }

            return ValueTask.CompletedTask;
        }
    }
}
