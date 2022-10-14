using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YtDlpSharpLib.Options;

namespace YtDlpSharpLib.Tests
{
    [TestClass]
    public class DownloadTests
    {
        private const string URL = "https://www.youtube.com/watch?v=C0DPdy98e4c";
        private static YtDlp _ydl = new YtDlp() { YtDlpPath = Utils.YtDlpBinaryName(), FFmpegPath = Utils.FfmpegBinaryName() };
        private static List<string> downloadedFiles = new List<string>();        

        [TestMethod]
        public async Task TestVideoDownloadSimple()
        {
            var result = await _ydl.DownloadVideo(URL, mergeFormat: Options.DownloadMergeFormat.Mkv);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(String.Empty, String.Join("", result.ErrorOutput));
            string file = result.Data;
            Assert.IsTrue(File.Exists(file));
            Assert.AreEqual(".mkv", Path.GetExtension(file));
            Assert.IsTrue(Path.GetFileNameWithoutExtension(file).Contains("TEST VIDEO"));
            downloadedFiles.Add(file);
        }

        [TestMethod]
        public async Task TestAudioDownloadSimple()
        {
            var result = await _ydl.DownloadAudio(URL, Options.AudioConversionFormat.Mp3);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(String.Empty, String.Join("", result.ErrorOutput));
            string file = result.Data;
            Assert.IsTrue(File.Exists(file));
            Assert.AreEqual(".mp3", Path.GetExtension(file));
            Assert.IsTrue(Path.GetFileNameWithoutExtension(file).Contains("TEST VIDEO"));
            downloadedFiles.Add(file);
        }

        [TestMethod]
        public async Task TestVideoDownloadWithOptions()
        {
            _ydl.OutputFolder = "Lib";
            _ydl.OutputFileTemplate = "%(extractor)s_%(title)s_%(upload_date)s.%(ext)s";
            _ydl.RestrictFilenames = true;
            var result = await _ydl.DownloadVideo(URL, format: "bestvideo", recodeFormat: Options.VideoRecodeFormat.Mp4);            
            Assert.IsTrue(result.Success);
            Assert.AreEqual(String.Empty, String.Join("", result.ErrorOutput));
            string file = result.Data;
            Assert.IsTrue(File.Exists(file));
            Assert.IsTrue(Path.GetDirectoryName(file).EndsWith("Lib"));
            Assert.AreEqual(".mp4", Path.GetExtension(file));
            Assert.AreEqual("youtube_TEST_VIDEO_20070221", Path.GetFileNameWithoutExtension(file));
            downloadedFiles.Add(file);
        }

        [TestMethod]
        public async Task TestVideoDownloadWithOverrideOptions()
        {
            var overrideOptions = new OptionSet()
            {
                Output = "%(extractor)s_%(title)s_%(upload_date)s.%(ext)s",
                RestrictFileNames = true,
                RecodeVideo = VideoRecodeFormat.Mp4
            };
            var result = await _ydl.DownloadVideo(URL, format: "bestvideo", overrideOptions: overrideOptions);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(String.Empty, String.Join("", result.ErrorOutput));
            string file = result.Data;
            Assert.IsTrue(File.Exists(file));
            Assert.AreEqual(".mp4", Path.GetExtension(file));
            Assert.AreEqual("youtube_TEST_VIDEO_20070221", Path.GetFileNameWithoutExtension(file));
            downloadedFiles.Add(file);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            foreach (var file in downloadedFiles)
            {
                File.Delete(file);
            }
        }
    }
}
