using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YtDlpSharpLib.Tests
{
    [TestClass]
    public class PrepBinariesForTests
    {      
        [AssemblyInitialize]
        public static void DownloadBinaries(TestContext context)
        {
            if (!File.Exists(Utils.YtDlpBinaryName()))
            {
                Utils.DownloadYtDlp();
            }
            if (!File.Exists(Utils.FfmpegBinaryName()))
            {
                Utils.DownloadFFmpeg();
            }
            if (!File.Exists(Utils.FfprobeBinaryName()))
            {
                Utils.DownloadFFprobe();
            }
        }
    }
}
