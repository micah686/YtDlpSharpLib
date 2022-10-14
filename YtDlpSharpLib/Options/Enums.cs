
namespace YtDlpSharpLib.Options
{
    /// <summary>
    /// Possible video merging formats.
    /// </summary>
    public enum DownloadMergeFormat
    {
        Unspecified, Avi, Flv, Mkv, Mov, Mp4, Webm
    }

    /// <summary>
    /// Possible audio formats for audio conversion.
    /// </summary>
    public enum AudioConversionFormat
    {
        //Best, Aac, Flac, Mp3, M4a, Opus, Vorbis, Wav
        Best, Aac, Alac, Flac, M4a, Mp3, Opus, Vorbis, Wav
    }

    /// <summary>
    /// Possible video formats for video conversion.
    /// </summary>
    public enum VideoRecodeFormat
    {
        //None, Mp4, Mkv, Ogg, Webm, Flv, Avi
        None, Avi, Flv, Mkv, Mov, Mp4, Webm, Aac, Aiff, Alac, Flac, M4a, Mka, Mp3, Ogg, Opus, Vorbis, Wav
    }
}
