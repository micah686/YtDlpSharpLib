using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YtDlpSharpLib.Metadata
{
    //https://github.com/lesmiscore/yt-dlp/blob/9c53b9a1b6b8914e4322263c97c26999f2e5832e/yt_dlp/extractor/common.py#L105-L403
    public class FormatInfo
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("manifest_url")]
        public string ManifestUrl { get; set; }

        [JsonPropertyName("ext")]
        public string Extension { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("format_id")]
        public string FormatId { get; set; }

        [JsonPropertyName("format_note")]
        public string FormatNote { get; set; }

        [JsonPropertyName("width")]
        public int? Width { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [JsonPropertyName("resolution")]
        public string Resolution { get; set; }

        [JsonPropertyName("dynamic_range")]
        public string DynamicRange { get; set; }

        [JsonPropertyName("tbr")]
        public double? Bitrate { get; set; }

        [JsonPropertyName("abr")]
        public double? AudioBitrate { get; set; }

        [JsonPropertyName("acodec")]
        public string AudioCodec { get; set; }

        [JsonPropertyName("asr")]
        public double? AudioSamplingRate { get; set; }

        [JsonPropertyName("audio_channels")]
        public int? AudioChannels { get; set; }

        [JsonPropertyName("vbr")]
        public double? VideoBitrate { get; set; }

        [JsonPropertyName("fps")]
        public float? FrameRate { get; set; }

        [JsonPropertyName("vcodec")]
        public string VideoCodec { get; set; }

        [JsonPropertyName("container")]
        public string ContainerFormat { get; set; }

        [JsonPropertyName("filesize")]
        public long? FileSize { get; set; }

        [JsonPropertyName("player_url")]
        public string PlayerUrl { get; set; }

        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }       

        [JsonPropertyName("preference")]
        public int? Preference { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("stretched_ratio")]
        public float? StretchedRatio { get; set; }

        [JsonPropertyName("has_drm")]
        public bool HasDrm { get; set; }
        
        public override string ToString() => $"[{Extension}] {Format}";

        //Unused Fields (These are fields that were excluded, but documented for future use:
        //downloader_options (internal use only)
        //filesize_approx
        //fragments
        //fragment_base_url
        //http_headers
        //is_from_start
        //language_preference
        //manifest_stream_number (internal use only)
        //no_resume
        //quality
        //source_preference
    }
}
