using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YtDlpSharpLib.Options;

namespace YtDlpSharpLib.Tests
{
    [TestClass]
    public class OptionSetTests
    {
        [TestMethod]
        public void TestSimpleOptionFromString()
        {
            Option<string> stringOption = new Option<string>("-s");
            Option<bool> boolOption = new Option<bool>("--bool");
            Option<int> intOption = new Option<int>("--int", "-i");
            stringOption.SetFromString("-s someValue");
            Assert.AreEqual("someValue", stringOption.Value);
            boolOption.SetFromString("--bool");
            Assert.AreEqual(true, boolOption.Value);
            intOption.SetFromString("-i 42");
            Assert.AreEqual(42, intOption.Value);
        }

        [TestMethod]
        public void TestEnumOptionFromString()
        {
            Option<VideoRecodeFormat> videoOption = new Option<VideoRecodeFormat>("--vid");
            videoOption.SetFromString("--vid mp4");
            Assert.AreEqual(VideoRecodeFormat.Mp4, videoOption.Value);
        }

        [TestMethod]
        public void TestCustomOptionSetFromString()
        {
            void AssertCustomOption(IOption option)
            {
                Assert.IsTrue(option.OptionStrings.Any());
                Assert.IsTrue(option.IsCustom);
                Assert.IsTrue(option.IsSet);
                Assert.IsNotNull(option.ToString());
            }

            const string firstOption = "--my-option";
            const string secondOption = "--my-valued-option";
            string[] lines = {
                firstOption,
                $"{secondOption} value"
            };

            // Assert custom options parsing from string
            OptionSet opts = OptionSet.FromString(lines);
            AssertCustomOption(opts.CustomOptions.First(s => s.DefaultOptionString == firstOption));
            AssertCustomOption(opts.CustomOptions.First(s => s.DefaultOptionString == secondOption));

            // Assert custom options cloning
            var cloned = opts.OverrideOptions(new OptionSet());
            AssertCustomOption(cloned.CustomOptions.First(s => s.DefaultOptionString == firstOption));
            AssertCustomOption(cloned.CustomOptions.First(s => s.DefaultOptionString == secondOption));

            // Assert custom options override
            var overrideOpts = opts.OverrideOptions(OptionSet.FromString(new[] { firstOption }));
            CollectionAssert.AllItemsAreUnique(overrideOpts.CustomOptions);

            // Assert custom options to string conversion
            Assert.IsFalse(string.IsNullOrWhiteSpace(opts.ToString()));
        }

        [TestMethod]
        public void TestCustomOptionSetByMethod()
        {
            // Can create custom options (also multiple with same name)
            OptionSet options = new OptionSet();
            options.AddCustomOption("--custom-string-option", "hello");
            options.AddCustomOption("--custom-bool-option", true);
            options.AddCustomOption("--custom-string-option", "world");
            Assert.AreEqual("--custom-string-option \"hello\" --custom-bool-option --custom-string-option \"world\"", options.ToString().Trim());

            // Can set values of custom options
            options.SetCustomOption("--custom-string-option", "new");
            Assert.AreEqual("--custom-string-option \"new\" --custom-bool-option --custom-string-option \"new\"", options.ToString().Trim());

            // Can delete custom options
            options.DeleteCustomOption("--custom-string-option");
            Assert.AreEqual("--custom-bool-option", options.ToString().Trim());
        }

        [TestMethod]
        public void TestOptionSetOverrideOptions()
        {
            var originalOptions = new OptionSet()
            {
                MergeOutputFormat = DownloadMergeFormat.Mp4,
                ExtractAudio = true,
                AudioFormat = AudioConversionFormat.Wav,
                AudioQuality = "0",
                Username = "bob",
                Verbose = true
            };
            var overrideOptions = new OptionSet()
            {
                MergeOutputFormat = DownloadMergeFormat.Mkv,
                Password = "passw0rd"
            };
            var newOptions = originalOptions.OverrideOptions(overrideOptions);
            Assert.AreEqual(AudioConversionFormat.Wav, newOptions.AudioFormat);
            Assert.AreEqual(0.ToString(), newOptions.AudioQuality);
            Assert.IsTrue(newOptions.Verbose);
            Assert.AreEqual(DownloadMergeFormat.Mkv, newOptions.MergeOutputFormat);
            Assert.AreEqual("bob", newOptions.Username);
            Assert.AreEqual("passw0rd", newOptions.Password);
        }
    }
}
