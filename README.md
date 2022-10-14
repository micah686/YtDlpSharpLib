# YtDlpSharpLib

A simple .NET wrapper library for [yt-dlp](https://github.com/yt-dlp/yt-dlp).

## What is it?

YtDlpSharpLib is a wrapper for the popular command-line video downloader yt-dlp.
It allows you to use the extensive features of yt-dlp in a .NET project.
For more about the features of yt-dlp, supported websites and anything else, visit its project page at https://github.com/yt-dlp/yt-dlp.

## How do I use it?

Until nuget packages are made, you need to build it from source. YtDlpSharpLib is a Net Core 6+ project only.

Now, there are two ways to use YoutubeDLSharp: the class `YtDlp` provides high level methods for downloading and converting videos
while the class `YtDlpProcess` allows directer and flexibler access to the yt-dlp process.

### Convenient Approach

In the simplest case, initializing the downloader and downloading a video can be achieved like this:

```csharp
var ytdl = new YtDlp();
// set the path of the yt-dlp and FFmpeg if they're not in PATH or current directory
ytdl.YtDlpPath = "path\\to\\youtube-dl\\binary";
ytdl.FFmpegPath = "path\\to\\ffmpeg\\binary";
// optional: set a different download folder
ytdl.OutputFolder = "some\\directory\\for\\video\\downloads";
// download a video
var res = await ytdl.DownloadVideo("https://www.youtube.com/watch?v=_QdPW8JrYzQ");
// the path of the downloaded file
string path = res.Data;
```

Instead of only downloading a video, you can also directly extract the audio track ...

```csharp
var res = await ytdl.DownloadAudio(
    "https://www.youtube.com/watch?v=QUQsqBqxoR4",
    AudioConversionFormat.Mp3
);
```

... or selectively download videos from a playlist:

```csharp
var res = await ytdl.DownloadVideoPlaylist(
    "https://www.youtube.com/playlist?list=PLPfak9ofGSn9sWgKrHrXrxQXXxwhCblaT",
    start: 52, end: 76
);
```

All of the above methods also allow you to track the download progress or cancel an ongoing download:

```csharp
// a progress handler with a callback that updates a progress bar
var progress = new Progress<DownloadProgress>(p => progressBar.Value = p.Progress);
// a cancellation token source used for cancelling the download
// use `cts.Cancel();` to perform cancellation
var cts = new CancellationTokenSource();
// ...
await ytdl.DownloadVideo("https://www.youtube.com/watch?v=_QdPW8JrYzQ",
                            progress: progress, ct: cts.Token);
```

As yt-dlp also allows you to extract extensive metadata for videos, you can also fetch these (without downloading the video):

```csharp
var res = await ytdl.GetVideoMetadata("https://www.youtube.com/watch?v=_QdPW8JrYzQ");
// get some video information
VideoInfo video = res.Data;
string title = video.Title;
string uploader = video.Uploader;
long? views = video.ViewCount;
// all available download formats
FormatInfo[] formats = video.Formats;
// ...
```


## Working with options

YtDlpSharpLib uses the `OptionSet` class to model yt-dlp options.
The names of the option properties correspond to the names of yt-dlp, so defining a set of options can look like this:

```csharp
var options = new OptionSet()
{
    NoContinue = true,
    RestrictFileNames = true,
    Format = "best",
    RecodeVideo = VideoRecodeFormat.Mp4,
    Exec = "echo {}"
}
```

For documentation of all options supported by yt-dlp and their effects, visit https://github.com/yt-dlp/yt-dlp#usage-and-options.

Additionally, YtDlpSharpLib allows you to pass **custom options** to the downloader program.
This is especially useful when a forked/ modified version of yt-dlp is used.
Custom can be specified like this:

```csharp
// add
options.AddCustomOption<string>("--my-custom-option", "value");
// set
options.SetCustomOption<string>("--my-custom-option", "new value");
```

#### `YtDlpProcess`

To run a yt-dlp process with the defined options, you can use the `YtDlpProcess` class:

```csharp
var ytdlProc = new YtDlpProcess();
// capture the standard output and error output
ytdlProc.OutputReceived += (o, e) => Console.WriteLine(e.Data);
ytdlProc.ErrorReceived += (o, e) => Console.WriteLine("ERROR: " + e.Data);
// start running
string[] urls = new[] { "https://github.com/yt-dlp/yt-dlp#usage-and-options" };
await ytdlProc.RunAsync(urls, options);
```

## Loading/ Saving configuration

You can persist a yt-dlp configuration to a file and reload it:

```csharp
// Save to file
var saveOptions = new OptionSet();
saveOptions.WriteConfigFile("path\\to\\file");

// Reload configuration
OptionSet loadOptions = OptionSet.LoadConfigFile("path\\to\\file");
```

The file format is compatible with the format used by yt-dlp itself.
For more, read https://github.com/yt-dlp/yt-dlp#configuration.

## Issues & Contributing

You are very welcome to contribute by reporting issues, fixing bugs or resolving inconsistencies to yt-dlp.
If you want to contribute a new feature to the library, please open an issue with your suggestion before starting to implement it.

All issues related to downloading specific videos, support for websites or downloading/ conversion features should better be reported to https://github.com/yt-dlp/yt-dlp/issues.

## License

This project is licensed under [BSD-3-Clause license](LICENSE.txt).  
Forked from [ Bluegrams/YoutubeDLSharp](https://github.com/Bluegrams/YoutubeDLSharp)
