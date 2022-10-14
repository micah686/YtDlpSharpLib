using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YtDlpSharpLib.Options
{
    public partial class OptionSet
    {
        private Option<string> _sponsorBlockMark = new Option<string>("--sponsorblock-mark");
        private Option<string> _sponsorBlockRemove = new Option<string>("--sponsorblock-remove");
        private Option<string> _sponsorBlockChapterTitle = new Option<string>("--sponsorblock-chapter-title");
        private Option<bool> _noSponsorBlock = new Option<bool>("--no-sponsorblock");
        private Option<string> _sponsorBlockApi = new Option<string>("--sponsorblock-api");

        /// <summary>
        /// SponsorBlock categories to create chapters
        /// for, separated by commas. Available
        /// categories are sponsor, intro, outro,
        /// selfpromo, preview, filler, interaction,
        /// music_offtopic, poi_highlight, all and
        /// default (=all). You can prefix the category
        /// with a "-" to exclude it. See [1] for
        /// description of the categories. E.g.
        /// --sponsorblock-mark all,-preview
        /// </summary>
        public string SponsorBlockMark { get => _sponsorBlockMark.Value; set => _sponsorBlockMark.Value = value;}
        /// <summary>
        /// SponsorBlock categories to be removed from
        /// the video file, separated by commas. If a
        /// category is present in both mark and remove,
        /// remove takes precedence. The syntax and
        /// available categories are the same as for
        /// --sponsorblock-mark except that "default"
        /// refers to "all,-filler" and poi_highlight is
        /// not available
        /// </summary>
        public string SponsorBlockRemove { get => _sponsorBlockRemove.Value; set => _sponsorBlockRemove.Value = value;}
        /// <summary>
        /// An output template for the title of the
        /// SponsorBlock chapters created by
        /// --sponsorblock-mark. The only available
        /// fields are start_time, end_time, category,
        /// categories, name, category_names. Defaults
        /// to "[SponsorBlock]: %(category_names)l"
        /// </summary>
        public string SponsorBlockChapterTitle { get => _sponsorBlockChapterTitle.Value; set => _sponsorBlockChapterTitle.Value = value;}
        /// <summary>
        /// Disable both --sponsorblock-mark and
        /// --sponsorblock-remove
        /// </summary>
        public bool NoSponsorBlock { get => _noSponsorBlock.Value; set => _noSponsorBlock.Value = value; }
        /// <summary>
        /// SponsorBlock API location, defaults to
        /// https://sponsor.ajay.app
        /// </summary>
        public string SponsorBlockApi { get => _sponsorBlockApi.Value; set => _sponsorBlockApi.Value = value;}
    }
}
