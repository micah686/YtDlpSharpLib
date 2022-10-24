using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YtDlpSharpLib.Helpers;

namespace YtDlpSharpLib;

/// <summary>
/// Utility methods.
/// </summary>
public static class Utils
{

    public static string YtDlpBinaryName => GetYtDlpBinaryName();
    public static string FfmpegBinaryName => GetFfmpegBinaryName();
    public static string FfprobeBinaryName => GetFfprobeBinaryName();

    public static void DownloadBinaries(string directoryPath = "")
    {
        Task.Run(() => DownloadYtDlp(directoryPath));
        Task.Run(() => DownloadFFmpeg(directoryPath));
        Task.Run(() => DownloadFFprobe(directoryPath));        
    }

    private static string GetYtDlpBinaryName(bool fullPath = false)
    {
        const string BASE_GITHUB_URL = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp";

        string downloadUrl = OSHelper.GetOSVersion() switch
        {
            OSVersion.Windows => $"{BASE_GITHUB_URL}.exe",
            OSVersion.OSX => $"{BASE_GITHUB_URL}_macos",
            OSVersion.Linux => BASE_GITHUB_URL,
            _ => throw new Exception("Your OS isn't supported"),
        };
        return fullPath ? downloadUrl : Path.GetFileName(downloadUrl);
    }

    private static string GetFfmpegBinaryName()
    {
        return OSHelper.GetOSVersion() switch
        {
            OSVersion.Windows => "ffmpeg.exe",
            OSVersion.OSX or OSVersion.Linux => "ffmpeg",
            _ => throw new Exception("Your OS isn't supported"),
        };
    }

    private static string GetFfprobeBinaryName()
    {
        return OSHelper.GetOSVersion() switch
        {
            OSVersion.Windows => "ffprobe.exe",
            OSVersion.OSX or OSVersion.Linux => "ffprobe",
            _ => throw new Exception("Your OS isn't supported"),
        };
    }

    /// <summary>
    /// Downloads the YT-DLP binary depending on OS
    /// </summary>
    /// <param name="directoryPath">The optional directory of where it should be saved to</param>
    /// <exception cref="Exception"></exception>
    internal static void DownloadYtDlp(string directoryPath = "")
    {            
        string downloadUrl = GetYtDlpBinaryName(true);

        if (string.IsNullOrEmpty(directoryPath)) { directoryPath = Directory.GetCurrentDirectory(); }

        var downloadLocation = Path.Combine(directoryPath, Path.GetFileName(downloadUrl));
        var data = Task.Run(() => GetFileBytesAsync(downloadUrl)).Result;
        File.WriteAllBytes(downloadLocation, data);
    }


    /// <summary>
    /// Downloads the FFmpeg binary depending on OS
    /// </summary>
    /// <param name="directoryPath">The optional directory of where it should be saved to</param>
    /// <exception cref="Exception"></exception>
    internal static async Task DownloadFFmpeg(string directoryPath = "")
    {
        await InternalFFHelper(directoryPath, FFmpegApi.BinaryType.FFmpeg);
    }

    /// <summary>
    /// Downloads the FFprobe binary depending on OS
    /// </summary>
    /// <param name="directoryPath">The optional directory of where it should be saved to</param>
    /// <exception cref="Exception"></exception>
    internal static async Task DownloadFFprobe(string directoryPath = "")
    {
        await InternalFFHelper(directoryPath, FFmpegApi.BinaryType.FFprobe);
    }

    private static async Task InternalFFHelper(string directoryPath = "", FFmpegApi.BinaryType binary = FFmpegApi.BinaryType.FFmpeg)
    {
        if (string.IsNullOrEmpty(directoryPath)) { directoryPath = Directory.GetCurrentDirectory(); }
        const string ffmpegApiUrl = "https://ffbinaries.com/api/v1/version/latest";
        var ffmpegVersion = JsonSerializer.Deserialize<FFmpegApi.Root>(await (await new HttpClient().GetAsync(ffmpegApiUrl)).Content.ReadAsStringAsync());
        var ffContent = OSHelper.GetOSVersion() switch
        {
            OSVersion.Windows => ffmpegVersion?.Bin.Windows64,
            OSVersion.OSX => ffmpegVersion?.Bin.Osx64,
            OSVersion.Linux => ffmpegVersion?.Bin.Linux64,
            _ => throw new NotImplementedException("Your OS isn't supported")
        };
        string downloadUrl = binary == FFmpegApi.BinaryType.FFmpeg ? ffContent.Ffmpeg : ffContent.Ffprobe;        

        var dataBytes = await GetFileBytesAsync(downloadUrl);
        using var stream = new MemoryStream(dataBytes);
        using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        if (archive.Entries.Count > 0)
        {
            archive.Entries[0].ExtractToFile(Path.Combine(directoryPath, archive.Entries[0].FullName), true);
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

    internal class FFmpegApi
    {
        public class Root
        {
            [JsonPropertyName("version")]
            public string Version { get; set; }

            [JsonPropertyName("permalink")]
            public string Permalink { get; set; }

            [JsonPropertyName("bin")]
            public Bin Bin { get; set; }
        }

        public class Bin
        {
            [JsonPropertyName("windows-64")]
            public OsBinVersion Windows64 { get; set; }

            [JsonPropertyName("linux-64")]
            public OsBinVersion Linux64 { get; set; }

            [JsonPropertyName("osx-64")]
            public OsBinVersion Osx64 { get; set; }
        }

        public class OsBinVersion
        {
            [JsonPropertyName("ffmpeg")]
            public string Ffmpeg { get; set; }

            [JsonPropertyName("ffprobe")]
            public string Ffprobe { get; set; }
        }
        
        public enum BinaryType
        {
            [EnumMember(Value = "ffmpeg")]
            FFmpeg,
            [EnumMember(Value = "ffprobe")]
            FFprobe
        }        
    }

    
}
