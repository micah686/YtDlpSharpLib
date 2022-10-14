using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YtDlpSharpLib.Helpers;

namespace YtDlpSharpLib
{
    /// <summary>
    /// Utility methods.
    /// </summary>
    public static class Utils
    {
        
        /// <summary>
        /// Returns the absolute path for the specified path string.
        /// Also searches the environment's PATH variable.
        /// </summary>
        /// <param name="fileName">The relative path string.</param>
        /// <returns>The absolute path or null if the file was not found.</returns>
        public static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var p in values.Split(Path.PathSeparator))
            {
                var fullPath = Path.Combine(p, fileName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return null;
        }

        public static void DownloadBinaries(string directoryPath = "")
        {
            DownloadYtDlp(directoryPath);
            DownloadFFmpeg(directoryPath);
            DownloadFFprobe(directoryPath);
        }

        public static string YtDlpBinaryName(bool fullPath = false)
        {
            const string BASE_GITHUB_URL = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp";

            string downloadUrl = "";
            switch (OSHelper.GetOSVersion())
            {
                case OSVersion.Windows:
                    downloadUrl = $"{BASE_GITHUB_URL}.exe";
                    break;
                case OSVersion.OSX:
                    downloadUrl = $"{BASE_GITHUB_URL}_macos";
                    break;
                case OSVersion.Linux:
                    downloadUrl = BASE_GITHUB_URL;
                    break;
                default:
                    throw new Exception("Your OS isn't supported");
            }

            if (fullPath)
            {
                return downloadUrl;
            }
            else
            {
                return Path.GetFileName(downloadUrl);
            }
        }

        public static string FfmpegBinaryName()
        {
            switch (OSHelper.GetOSVersion())
            {
                case OSVersion.Windows:
                    return "ffmpeg.exe";
                case OSVersion.OSX:
                case OSVersion.Linux:
                    return "ffmpeg";
                default:
                    throw new Exception("Your OS isn't supported");
            }
        }

        public static string FfprobeBinaryName()
        {
            switch (OSHelper.GetOSVersion())
            {
                case OSVersion.Windows:
                    return "ffprobe.exe";
                case OSVersion.OSX:
                case OSVersion.Linux:
                    return "ffprobe";
                default:
                    throw new Exception("Your OS isn't supported");
            }
        }

        /// <summary>
        /// Downloads the YT-DLP binary depending on OS
        /// </summary>
        /// <param name="directoryPath">The optional directory of where it should be saved to</param>
        /// <exception cref="Exception"></exception>
        public static void DownloadYtDlp(string directoryPath = "")
        {            
            string downloadUrl = YtDlpBinaryName(true);

            if (string.IsNullOrEmpty(directoryPath)) { directoryPath = Directory.GetCurrentDirectory(); }

            var downloadLocation = Path.Combine(directoryPath, Path.GetFileName(downloadUrl));
            var data = Task.Run(() => GetFileBytesAsync(downloadUrl)).Result;
            File.WriteAllBytes(downloadLocation, data);
        }

        /// <summary>
        /// Downloads the FFmpeg binary depending on the OS
        /// </summary>
        /// <param name="directoryPath">The optional directory of where it should be saved to</param>
        /// <exception cref="Exception"></exception>
        public static void DownloadFFmpeg(string directoryPath = "")
        {
            if (string.IsNullOrEmpty(directoryPath)) { directoryPath = Directory.GetCurrentDirectory(); }
            const string FFMPEG_API_URL = "https://ffbinaries.com/api/v1/version/latest";

            string jsonData = Task.Run(async () => await new HttpClient().GetStringAsync(FFMPEG_API_URL)).Result;
#nullable enable
            JsonObject? jsonObj = JsonSerializer.Deserialize<JsonObject>(jsonData);
#nullable disable

            if (jsonObj != null)
            {
                var ffmpegURL = OSHelper.GetOSVersion() switch
                {
                    OSVersion.Windows => JsonPeeker(jsonObj, new string[] { "bin", "windows-64", "ffmpeg" }),
                    OSVersion.OSX => JsonPeeker(jsonObj, new string[] { "bin", "osx-64", "ffmpeg" }),
                    OSVersion.Linux => JsonPeeker(jsonObj, new string[] { "bin", "linux-64", "ffmpeg" }),
                    _ => throw new Exception("Your OS isn't supported")
                };
                var dataBytes = Task.Run(async () => await GetFileBytesAsync(ffmpegURL)).Result;
                using (var stream = new MemoryStream(dataBytes))
                {
                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
                    {
                        if (archive.Entries.Count > 0)
                        {
                            archive.Entries[0].ExtractToFile(Path.Combine(directoryPath, archive.Entries[0].FullName), true);
                        }
                    }
                }
            }

        }

        public static void DownloadFFprobe(string directoryPath = "")
        {
            if (string.IsNullOrEmpty(directoryPath)) { directoryPath = Directory.GetCurrentDirectory(); }
            const string FFMPEG_API_URL = "https://ffbinaries.com/api/v1/version/latest";

            string jsonData = Task.Run(async () => await new HttpClient().GetStringAsync(FFMPEG_API_URL)).Result;
#nullable enable
            JsonObject? jsonObj = JsonSerializer.Deserialize<JsonObject>(jsonData);
#nullable disable

            if (jsonObj != null)
            {
                var ffmpegURL = OSHelper.GetOSVersion() switch
                {
                    OSVersion.Windows => JsonPeeker(jsonObj, new string[] { "bin", "windows-64", "ffprobe" }),
                    OSVersion.OSX => JsonPeeker(jsonObj, new string[] { "bin", "osx-64", "ffprobe" }),
                    OSVersion.Linux => JsonPeeker(jsonObj, new string[] { "bin", "linux-64", "ffprobe" }),
                    _ => throw new Exception("Your OS isn't supported")
                };
                var dataBytes = Task.Run(async () => await GetFileBytesAsync(ffmpegURL)).Result;
                using var stream = new MemoryStream(dataBytes);
                using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
                if (archive.Entries.Count > 0)
                {
                    archive.Entries[0].ExtractToFile(Path.Combine(directoryPath, archive.Entries[0].FullName), true);
                }
            }
        }

        /// <summary>
        /// Downloads a file from the specified URI
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>Returns a byte array of the file that was downloaded</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static async Task<byte[]> GetFileBytesAsync(string uri)
        {
            if (!Uri.TryCreate(uri, UriKind.Absolute, out _))
                throw new InvalidOperationException("URI is invalid.");

            var httpClient = new HttpClient();
            byte[] fileBytes = await httpClient.GetByteArrayAsync(uri);
            return fileBytes;
        }

        /// <summary>
        /// Checks sections of a <see cref="JsonNode"/>, stopping if it hits a null
        /// </summary>
        /// <param name="json">Json object you want to look through</param>
        /// <param name="keys">List of properties you want to check through</param>
        /// <returns>Returns the object if it could find the value, or an empty string if a result turned up null</returns>
        private static string JsonPeeker(JsonObject json, string[] keys)
        {
#nullable enable
            JsonNode? obj = null;
#nullable disable
            for (int i = 0; i < keys.Length; i++)
            {
                if (obj == null)
                {
                    obj = json[keys[i]] ?? null;
                    if (obj == null) { break; }
                }
                else
                {
                    obj = obj[keys[i]] ?? null;
                    if (obj == null) { break; }
                }
            }
            var result = obj != null ? obj.ToString() : string.Empty;
            return result;
        }
    }
}
