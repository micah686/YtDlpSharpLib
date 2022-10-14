using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YtDlpSharpLib.Options
{
    public partial class OptionSet
    {
        private Option<string> _extractorRetries = new Option<string>("--extractor-retries");
        private Option<bool> _allowDynamicMpd = new Option<bool>("--allow-dynamic-mpd");
        private Option<bool> _ignoreDynamicMpd = new Option<bool>("--ignore-dynamic-mpd");
        private Option<bool> _hlsSplitDiscontinuity = new Option<bool>("--hls-split-discontinuity");
        private Option<bool> _noHlsSplitDiscontinuity = new Option<bool>("----no-hls-split-discontinuity");
        private Option<string> _extractorArgs = new Option<string>("--extractor-args");

        /// <summary>
        /// Number of retries for known extractor errors
        /// (default is 3), or "infinite"
        /// </summary>
        public string ExtractorRetries { get => _extractorRetries.Value; set => _extractorRetries.Value = value; }
        /// <summary>
        /// Process dynamic DASH manifests (default)
        /// (Alias: --no-ignore-dynamic-mpd)
        /// </summary>
        public bool AllowDynamicMpd { get => _allowDynamicMpd.Value; set => _allowDynamicMpd.Value = value;}
        /// <summary>
        /// Do not process dynamic DASH manifests
        /// (Alias: --no-allow-dynamic-mpd)
        /// </summary>
        public bool IgnoreDynamicMpd { get => _ignoreDynamicMpd.Value; set => _ignoreDynamicMpd.Value = value;}
        /// <summary>
        /// Split HLS playlists to different formats at
        /// discontinuities such as ad breaks
        /// </summary>
        public bool HlsSplitDiscontinuity { get => _hlsSplitDiscontinuity.Value; set => _hlsSplitDiscontinuity.Value = value; }
        /// <summary>
        /// Do not split HLS playlists to different
        /// formats at discontinuities such as ad breaks
        /// (default)
        /// </summary>
        public bool NoHlsSplitDiscontinuity { get => _noHlsSplitDiscontinuity.Value; set => _noHlsSplitDiscontinuity.Value = value;}
        /// <summary>
        /// Pass ARGS arguments to the IE_KEY extractor.
        /// See "EXTRACTOR ARGUMENTS" for details. You
        /// can use this option multiple times to give
        /// arguments for different extractors
        /// </summary>
        public string ExtractorArgs { get => _extractorArgs.Value; set => _extractorArgs.Value = value; }
    }
}
