using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YtDlpSharpLib.Tests
{
    [TestClass]
    public class MetadataTest
    {
        private static YtDlp _ydl = new YtDlp() { YtDlpPath = Utils.YtDlpBinaryName(), FFmpegPath = Utils.FfmpegBinaryName()};              

        [TestMethod]
        public async Task TestVideoInformationYoutube()
        {
            string url = "https://www.youtube.com/watch?v=C0DPdy98e4c&t=9s";
            RunResult<VideoInfo> result = await _ydl.GetVideoMetadata(url);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(MetadataType.Video, result.Data.ResultType);
            Assert.AreEqual("TEST VIDEO", result.Data.Title);
            Assert.AreEqual("Youtube", result.Data.ExtractorKey);
            Assert.AreEqual(new DateOnly(2007, 02, 21), result.Data.UploadDate);
            Assert.IsNotNull(result.Data.Formats);
            Assert.IsNotNull(result.Data.Tags);
            Assert.IsNull(result.Data.Entries);
        }

        [TestMethod]
        public async Task TestVideoInformationVimeo()
        {
            string url = "https://vimeo.com/23608259";
            RunResult<VideoInfo> result = await _ydl.GetVideoMetadata(url);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(MetadataType.Video, result.Data.ResultType);
            Assert.AreEqual("Cats in Tanks", result.Data.Title);
            Assert.AreEqual("Vimeo", result.Data.ExtractorKey);
            Assert.AreEqual("Whitehouse Post", result.Data.Uploader);
            Assert.IsNotNull(result.Data.Formats);
            Assert.IsNull(result.Data.Entries);
        }

        [TestMethod]
        public async Task TestPlaylistInformation()
        {
            string url = "https://www.youtube.com/playlist?list=PLD8804CB40CAB0EA5";
            RunResult<VideoInfo> result = await _ydl.GetVideoMetadata(url);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(MetadataType.Playlist, result.Data.ResultType);
            Assert.AreEqual("E.Grieg Peer Gynt Suite playlist", result.Data.Title);
            Assert.IsNotNull(result.Data.Entries);
        }
    }
}
