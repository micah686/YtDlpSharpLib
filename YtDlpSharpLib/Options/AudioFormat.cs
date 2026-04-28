namespace YtDlpSharpLib.Options;

/// <summary>
/// Enumerates audio formats supported by yt-dlp's <c>--audio-format</c> flag.
/// </summary>
public enum AudioFormat
{
    /// <summary>MPEG-4 audio container (.m4a).</summary>
    M4a,

    /// <summary>MP3 audio (.mp3).</summary>
    Mp3,

    /// <summary>Opus audio (.opus).</summary>
    Opus,

    /// <summary>FLAC lossless audio (.flac).</summary>
    Flac
}
