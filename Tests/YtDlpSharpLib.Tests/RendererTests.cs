using System.Globalization;
using System.Reflection;
using YtDlpSharpLib.Options;
using YtDlpSharpLib.Rendering;

namespace YtDlpSharpLib.Tests;

public sealed class RendererTests
{
    [Fact]
    public void Render_HandlesGeneratedOptionsAndLegacyAliases()
    {
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            var renderer = new YtDlpArgumentRenderer();

#pragma warning disable CS0618
            var options = new YtDlpOptions
            {
                Format = new FormatOptions
                {
                    Format = new FormatSelector("best"),
                    ExtractAudio = true
                },
                General = new YtDlpGeneralOptions
                {
                    ConfigLocations = ["user.conf", "override.conf"]
                },
                Network = new YtDlpNetworkOptions
                {
                    SocketTimeout = 1.25
                },
                PostProcessing = new YtDlpPostProcessingOptions
                {
                    Fixup = YtDlpPostProcessingFixup.DetectOrWarn
                },
                AdvancedArguments =
                [
                    new RawYtDlpArgument
                    {
                        Name = "--custom",
                        Value = "first",
                        Values = ["second"]
                    }
                ]
            };
#pragma warning restore CS0618

            Assert.Equal(
                [
                    "--format",
                    "best",
                    "--extract-audio",
                    "--config-locations",
                    "user.conf",
                    "--config-locations",
                    "override.conf",
                    "--socket-timeout",
                    "1.25",
                    "--fixup",
                    "detect_or_warn",
                    "--custom",
                    "first",
                    "second"
                ],
                renderer.Render(options));
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Fact]
    public void Render_HandlesGeneratedMultiValueOptions()
    {
        var renderer = new YtDlpArgumentRenderer();
        var options = new YtDlpOptions
        {
            General = new YtDlpGeneralOptions
            {
                Alias =
                [
                    ["get-audio", "-x --audio-format mp3"]
                ]
            }
        };

        Assert.Equal(["--alias", "get-audio", "-x --audio-format mp3"], renderer.Render(options));
    }

    [Fact]
    public void LegacyAliasProperties_AreMarkedObsolete()
    {
        var attribute = typeof(FormatOptions)
            .GetProperty("ExtractAudio")?
            .GetCustomAttribute<ObsoleteAttribute>();

        Assert.NotNull(attribute);
        Assert.Contains("YtDlpOptions.PostProcessing.ExtractAudio", attribute.Message);
    }
}
