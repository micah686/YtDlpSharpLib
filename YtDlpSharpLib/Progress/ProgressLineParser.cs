using System.Globalization;

namespace YtDlpSharpLib.Progress;

/// <summary>
/// Allocation-conscious parser for yt-dlp progress lines. Operates on
/// <see cref="ReadOnlySpan{T}"/> input so callers can avoid per-line string copies
/// for the percent/speed/ETA path.
/// </summary>
public static class ProgressLineParser
{
    private const string OfSentinel = " of ";
    private const string AtSentinel = " at ";
    private const string EtaSentinel = " ETA ";
    private const string InSentinel = " in ";

    /// <summary>
    /// Attempts to parse a single line of yt-dlp output into a <see cref="YtDlpProgress"/>.
    /// Returns <see langword="false"/> for lines that cannot be classified as progress.
    /// </summary>
    public static bool TryParse(ReadOnlySpan<char> line, out YtDlpProgress progress)
    {
        var trimmed = line.TrimStart();
        if (trimmed.IsEmpty || trimmed[0] != '[')
        {
            progress = null!;
            return false;
        }

        var closeBracket = trimmed.IndexOf(']');
        if (closeBracket < 0)
        {
            progress = null!;
            return false;
        }

        var tag = trimmed[1..closeBracket];
        var rest = trimmed[(closeBracket + 1)..].TrimStart();
        var phase = MapPhase(tag);

        if (phase == ProgressPhase.Downloading && TryParseDownload(rest, out var downloadProgress))
        {
            progress = downloadProgress with { RawLine = line.ToString() };
            return true;
        }

        progress = new YtDlpProgress
        {
            Phase = phase,
            AdditionalInfo = rest.IsEmpty ? null : rest.ToString(),
            RawLine = line.ToString()
        };
        return true;
    }

    private static ProgressPhase MapPhase(ReadOnlySpan<char> tag) =>
        tag switch
        {
            "download" => ProgressPhase.Downloading,
            "ffmpeg" => ProgressPhase.Merging,
            "Merger" => ProgressPhase.Merging,
            "ExtractAudio" => ProgressPhase.ExtractingAudio,
            "VideoConvertor" => ProgressPhase.Converting,
            "VideoConverter" => ProgressPhase.Converting,
            "EmbedThumbnail" => ProgressPhase.EmbeddingThumbnail,
            "Metadata" => ProgressPhase.PostProcessing,
            "Fixup" => ProgressPhase.PostProcessing,
            _ => ProgressPhase.PostProcessing
        };

    private static bool TryParseDownload(ReadOnlySpan<char> rest, out YtDlpProgress progress)
    {
        progress = null!;

        var percentEnd = rest.IndexOf('%');
        if (percentEnd <= 0)
        {
            return false;
        }

        var percentStart = percentEnd;
        while (percentStart > 0)
        {
            var ch = rest[percentStart - 1];
            if (!char.IsDigit(ch) && ch != '.')
            {
                break;
            }

            percentStart--;
        }

        var percentSpan = rest[percentStart..percentEnd];
        if (!double.TryParse(percentSpan, NumberStyles.Float, CultureInfo.InvariantCulture, out var percent))
        {
            return false;
        }

        long? totalBytes = null;
        string? speed = null;
        string? eta = null;

        var ofIndex = rest.IndexOf(OfSentinel.AsSpan());
        if (ofIndex >= 0)
        {
            var sizeStart = ofIndex + OfSentinel.Length;
            while (sizeStart < rest.Length && rest[sizeStart] == '~')
            {
                sizeStart++;
            }

            var sizeSpan = ExtractToken(rest, sizeStart);
            totalBytes = ParseSize(sizeSpan);
        }

        var atIndex = rest.IndexOf(AtSentinel.AsSpan());
        if (atIndex >= 0)
        {
            var speedSpan = ExtractToken(rest, atIndex + AtSentinel.Length);
            if (!speedSpan.IsEmpty && !speedSpan.SequenceEqual("Unknown".AsSpan()))
            {
                speed = speedSpan.ToString();
            }
        }

        var etaIndex = rest.IndexOf(EtaSentinel.AsSpan());
        if (etaIndex >= 0)
        {
            var etaSpan = ExtractToken(rest, etaIndex + EtaSentinel.Length);
            if (!etaSpan.IsEmpty && !etaSpan.SequenceEqual("Unknown".AsSpan()))
            {
                eta = etaSpan.ToString();
            }
        }
        else
        {
            var inIndex = rest.IndexOf(InSentinel.AsSpan());
            if (inIndex >= 0)
            {
                var elapsedSpan = ExtractToken(rest, inIndex + InSentinel.Length);
                if (!elapsedSpan.IsEmpty)
                {
                    eta = elapsedSpan.ToString();
                }
            }
        }

        long? downloadedBytes = null;
        if (totalBytes is { } total && percent >= 0)
        {
            downloadedBytes = (long)(total * (percent / 100.0));
        }

        progress = new YtDlpProgress
        {
            Phase = ProgressPhase.Downloading,
            Percent = percent,
            TotalBytes = totalBytes,
            DownloadedBytes = downloadedBytes,
            Speed = speed,
            Eta = eta
        };
        return true;
    }

    private static ReadOnlySpan<char> ExtractToken(ReadOnlySpan<char> source, int start)
    {
        if (start >= source.Length)
        {
            return ReadOnlySpan<char>.Empty;
        }

        var slice = source[start..];
        var end = slice.IndexOf(' ');
        return end < 0 ? slice : slice[..end];
    }

    private static long? ParseSize(ReadOnlySpan<char> sizeSpan)
    {
        if (sizeSpan.IsEmpty || sizeSpan.SequenceEqual("N/A".AsSpan()))
        {
            return null;
        }

        var splitIdx = 0;
        while (splitIdx < sizeSpan.Length && (char.IsDigit(sizeSpan[splitIdx]) || sizeSpan[splitIdx] == '.'))
        {
            splitIdx++;
        }

        if (splitIdx == 0)
        {
            return null;
        }

        var numSpan = sizeSpan[..splitIdx];
        var unitSpan = sizeSpan[splitIdx..];

        if (!double.TryParse(numSpan, NumberStyles.Float, CultureInfo.InvariantCulture, out var num))
        {
            return null;
        }

        var multiplier = unitSpan switch
        {
            "" or "B" => 1L,
            "KiB" => 1L << 10,
            "MiB" => 1L << 20,
            "GiB" => 1L << 30,
            "TiB" => 1L << 40,
            "K" or "KB" => 1_000L,
            "M" or "MB" => 1_000_000L,
            "G" or "GB" => 1_000_000_000L,
            "T" or "TB" => 1_000_000_000_000L,
            _ => 0L
        };

        return multiplier == 0 ? null : (long)(num * multiplier);
    }
}
