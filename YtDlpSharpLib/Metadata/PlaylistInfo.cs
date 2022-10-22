using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YtDlpSharpLib.Metadata
{
    public class PlaylistInfo
    {
        [JsonPropertyName("uploader")]
        public string Uploader { get; set; }

        [JsonPropertyName("uploader_id")]
        public string UploaderId { get; set; }

        [JsonPropertyName("uploader_url")]
        public string UploaderUrl { get; set; }

        [JsonPropertyName("thumbnails")]
        public List<ThumbnailInfo> Thumbnails { get; set; }

        [JsonPropertyName("tags")]
        public List<object> Tags { get; set; }

        [JsonPropertyName("view_count")]
        public int? ViewCount { get; set; }

        [JsonPropertyName("availability")]
        public string Availability { get; set; }

        [JsonPropertyName("modified_date")]
        public string ModifiedDate { get; set; }

        [JsonPropertyName("playlist_count")]
        public int? PlaylistCount { get; set; }

        [JsonPropertyName("channel_follower_count")]
        public object ChannelFollowerCount { get; set; }

        [JsonPropertyName("channel")]
        public string Channel { get; set; }

        [JsonPropertyName("channel_id")]
        public string ChannelId { get; set; }

        [JsonPropertyName("channel_url")]
        public string ChannelUrl { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("_type")]
        public string Type { get; set; }

        [JsonPropertyName("entries")]
        public List<VideoInfo> Entries { get; set; }

        [JsonPropertyName("webpage_url")]
        public string WebpageUrl { get; set; }

        [JsonPropertyName("original_url")]
        public string OriginalUrl { get; set; }

        [JsonPropertyName("webpage_url_basename")]
        public string WebpageUrlBasename { get; set; }

        [JsonPropertyName("webpage_url_domain")]
        public string WebpageUrlDomain { get; set; }

        [JsonPropertyName("extractor")]
        public string Extractor { get; set; }

        [JsonPropertyName("extractor_key")]
        public string ExtractorKey { get; set; }

        [JsonPropertyName("requested_entries")]
        public List<int> RequestedEntries { get; set; }

        [JsonPropertyName("epoch")]
        public int? Epoch { get; set; }        
    }
}
