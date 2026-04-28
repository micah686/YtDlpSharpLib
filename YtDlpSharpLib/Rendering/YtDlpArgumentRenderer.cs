using System.Collections;
using System.Globalization;
using System.Reflection;
using YtDlpSharpLib.Exceptions;
using YtDlpSharpLib.Options;

namespace YtDlpSharpLib.Rendering;

/// <summary>
/// Reflection-driven renderer that converts <see cref="YtDlpOptions"/> into
/// deterministic argv tokens.
/// </summary>
public sealed class YtDlpArgumentRenderer : IYtDlpArgumentRenderer
{
    /// <inheritdoc />
    public IReadOnlyList<string> Render(YtDlpOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var args = new List<string>();

        RenderGroup(args, options.Format);
        RenderGroup(args, options.Output);
        RenderGroup(args, options.Subtitles);
        RenderGroup(args, options.Metadata);
        RenderGroup(args, options.Playlist);
        RenderAdvanced(args, options.AdvancedArguments);

        return args;
    }

    private static void RenderGroup(List<string> args, object group)
    {
        foreach (var property in group.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var attribute = property.GetCustomAttribute<YtDlpArgumentAttribute>();
            if (attribute is null)
            {
                continue;
            }

            var value = property.GetValue(group);
            if (value is null)
            {
                continue;
            }

            if (attribute.ValueStyle == ArgumentValueStyle.Switch)
            {
                if (value is true)
                {
                    args.Add(attribute.Name);
                }

                continue;
            }

            if (value is IEnumerable sequence and not string)
            {
                foreach (var item in sequence)
                {
                    if (item is null)
                    {
                        continue;
                    }

                    args.Add(attribute.Name);
                    args.Add(RenderValue(item));
                }

                continue;
            }

            args.Add(attribute.Name);
            args.Add(RenderValue(value));
        }
    }

    private static void RenderAdvanced(List<string> args, IReadOnlyList<RawYtDlpArgument> advancedArguments)
    {
        foreach (var argument in advancedArguments)
        {
            if (!argument.Name.StartsWith("--", StringComparison.Ordinal))
            {
                throw new YtDlpValidationException(
                    $"Advanced argument '{argument.Name}' must be a long yt-dlp flag (begin with '--').");
            }

            args.Add(argument.Name);

            if (argument.Value is not null)
            {
                args.Add(argument.Value);
            }
        }
    }

    private static string RenderValue(object value) =>
        value switch
        {
            string text => text,
            FormatSelector selector => selector.Value,
            OutputTemplate template => template.Value,
            YtDlpPathMapping path => $"{RenderPathKind(path.Kind)}:{path.Path}",
            VideoContainer container => RenderEnum(container),
            AudioFormat format => RenderEnum(format),
            SubtitleFormat format => RenderEnum(format),
            Enum otherEnum => otherEnum.ToString().ToLowerInvariant(),
            IFormattable formattable => formattable.ToString(format: null, CultureInfo.InvariantCulture),
            _ => Convert.ToString(value, CultureInfo.InvariantCulture)
                ?? throw new YtDlpValidationException($"Could not render value '{value}'.")
        };

    private static string RenderPathKind(YtDlpPathKind kind) =>
        kind switch
        {
            YtDlpPathKind.Home => "home",
            YtDlpPathKind.Temp => "temp",
            YtDlpPathKind.Subtitle => "subtitle",
            YtDlpPathKind.Thumbnail => "thumbnail",
            YtDlpPathKind.InfoJson => "infojson",
            _ => throw new YtDlpValidationException($"Unsupported path kind '{kind}'.")
        };

    private static string RenderEnum<T>(T value)
        where T : struct, Enum =>
        value.ToString().ToLowerInvariant();
}
