using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using YtDlpSharpLib.Converters;

namespace YtDlpSharpLib.Metadata
{
    //https://github.com/yt-dlp/yt-dlp/blob/9c53b9a1b6b8914e4322263c97c26999f2e5832e/yt_dlp/extractor/common.py#L105-L403
    public class ChapterInfo
    {
        //[JsonConverter(typeof(LongToStringConverter))]
        [JsonPropertyName("start_time")]
        public double? StartTime { get; set; }

        [JsonPropertyName("end_time")]
        public double? EndTime { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
