﻿namespace YtDlpSharpLib.Options;

public partial class OptionSet
{
    private Option<string> _batchFile = new Option<string>("-a","--batch-file");
    private Option<bool> _noBatchFile = new Option<bool>("--no-batch-file");
    private Option<string> _paths = new Option<string>("-P","--paths");
    private Option<string> _output = new Option<string>("-o","--output");
    private Option<string> _outputNaPlaceholder = new Option<string>("--output-na-placeholder");
    private Option<bool> _restrictFilenames = new Option<bool>("--restrict-filenames");
    private Option<bool> _noRestrictFilenames = new Option<bool>("--no-restrict-filenames");
    private Option<bool> _windowsFilenames = new Option<bool>("--windows-filenames");
    private Option<bool> _noWindowsFilenames = new Option<bool>("--no-windows-filenames");
    private Option<bool> _trimFilenames = new Option<bool>("--trim-filenames");
    private Option<bool> _noOverwrites = new Option<bool>("-w","--no-overwrites");
    private Option<bool> _forceOverwrites = new Option<bool>("--force-overwrites");
    private Option<bool> _noForceOverwrites = new Option<bool>("--no-force-overwrites");
    private Option<bool> _continue = new Option<bool>("-C","--continue");
    private Option<bool> _noContinue = new Option<bool>("--no-continue");
    private Option<bool> _part = new Option<bool>("--part");
    private Option<bool> _noPart = new Option<bool>("--no-part");
    private Option<bool> _mtime = new Option<bool>("--mtime");
    private Option<bool> _noMtime = new Option<bool>("--no-mtime");
    private Option<bool> _writeDescription = new Option<bool>("--write-description");
    private Option<bool> _noWriteDescription = new Option<bool>("--no-write-description");
    private Option<bool> _writeInfoJson = new Option<bool>("--write-info-json");
    private Option<bool> _noWriteInfoJson = new Option<bool>("--no-write-info-json");
    private Option<bool> _writePlaylistMetafiles = new Option<bool>("--write-playlist-metafiles");
    private Option<bool> _noWritePlaylistMetafiles = new Option<bool>("--no-write-playlist-metafiles");
    private Option<bool> _cleanInfoJson = new Option<bool>("--clean-info-json");
    private Option<bool> _noCleanInfoJson = new Option<bool>("--no-clean-info-json");
    private Option<bool> _writeComments = new Option<bool>("--write-comments");
    private Option<bool> _noWriteComments = new Option<bool>("--no-write-comments");
    private Option<string> _loadInfoJson = new Option<string>("--load-info-json");
    private Option<bool> _cookies = new Option<bool>("--cookies");
    private Option<bool> _noCookies = new Option<bool>("--no-cookies");
    private Option<string> _cookiesFromBrowser = new Option<string>("--cookies-from-browser");
    private Option<bool> _noCookiesFromBrowser = new Option<bool>("--no-cookies-from-browser");
    private Option<string> _cacheDir = new Option<string>("--cache-dir");
    private Option<bool> _noCacheDir = new Option<bool>("--no-cache-dir");
    private Option<bool> _rmCacheDir = new Option<bool>("--rm-cache-dir");

    /// <summary>
    /// File containing URLs to download ("-" for
    /// stdin), one URL per line. Lines starting
    /// with "#", ";" or "]" are considered as
    /// comments and ignored
    /// </summary>
    public string BatchFile { get=> _batchFile.Value; set=> _batchFile.Value = value; }
    /// <summary>
    /// Do not read URLs from batch file (default)
    /// </summary>
    public bool NoBatchFile { get => _noBatchFile.Value; set => _noBatchFile.Value = value; }
    /// <summary>
    /// The paths where the files should be
    /// downloaded. Specify the type of file and the
    /// path separated by a colon ":". All the same
    /// TYPES as --output are supported.
    /// Additionally, you can also provide "home"
    /// (default) and "temp" paths. All intermediary
    /// files are first downloaded to the temp path
    /// and then the final files are moved over to
    /// the home path after download is finished.
    /// This option is ignored if --output is an
    /// absolute path
    /// </summary>
    public string Paths { get => _paths.Value; set => _paths.Value = value; }
    /// <summary>
    /// Output filename template; see "OUTPUT
    /// TEMPLATE" for details
    /// </summary>
    public string Output { get => _output.Value; set => _output.Value = value; }
    /// <summary>
    /// Placeholder for unavailable fields in
    /// "OUTPUT TEMPLATE" (default: "NA")
    /// </summary>
    public string OutputNaPlaceholder { get => _outputNaPlaceholder.Value; set => _outputNaPlaceholder.Value = value; }
    /// <summary>
    /// Restrict filenames to only ASCII characters,
    /// and avoid "&amp;" and spaces in filenames
    /// </summary>
    public bool RestrictFileNames { get => _restrictFilenames.Value; set => _restrictFilenames.Value = value; }
    /// <summary>
    /// Allow Unicode characters, "&amp;" and spaces in
    /// filenames (default)
    /// </summary>
    public bool NoRestrictFileNames { get => _noRestrictFilenames.Value; set => _noRestrictFilenames.Value = value; }
    /// <summary>
    /// Force filenames to be Windows-compatible
    /// </summary>
    public bool WindowsFileNames { get => _windowsFilenames.Value; set => _windowsFilenames.Value = value; }
    /// <summary>
    /// Make filenames Windows-compatible only if
    /// using Windows (default)
    /// </summary>
    public bool NoWindowsFileNames { get => _noWindowsFilenames.Value; set => _noWindowsFilenames.Value = value; }
    /// <summary>
    /// Limit the filename length (excluding
    /// extension) to the specified number of
    /// characters
    /// </summary>
    public bool TrimFilenames { get => _trimFilenames.Value; set => _trimFilenames.Value = value; }
    /// <summary>
    /// Do not overwrite any files
    /// </summary>
    public bool NoOverwrites { get => _noOverwrites.Value; set => _noOverwrites.Value = value; }
    /// <summary>
    /// Overwrite all video and metadata files. This
    /// option includes --no-continue
    /// </summary>
    public bool ForceOverwrites { get => _forceOverwrites.Value; set => _forceOverwrites.Value = value; }
    /// <summary>
    /// Do not overwrite the video, but overwrite
    /// related files (default)
    /// </summary>
    public bool NoForceOverwrites { get => _noForceOverwrites.Value; set => _noForceOverwrites.Value = value;}
    /// <summary>
    /// Resume partially downloaded files/fragments
    /// (default)
    /// </summary>
    public bool Continue { get => _continue.Value; set => _continue.Value = value; }
    /// <summary>
    /// Do not resume partially downloaded
    /// fragments. If the file is not fragmented,
    /// restart download of the entire file
    /// </summary>
    public bool NoContinue { get => _noContinue.Value; set => _noContinue.Value = value; }
    /// <summary>
    /// Use .part files instead of writing directly
    /// into output file (default)
    /// </summary>
    public bool Part { get => _part.Value; set => _part.Value = value; }
    /// <summary>
    /// Do not use .part files - write directly into
    /// output file
    /// </summary>
    public bool NoPart { get => _noPart.Value; set => _noPart.Value = value; }
    /// <summary>
    /// Use the Last-modified header to set the file
    /// modification time (default)
    /// </summary>
    public bool Mtime { get => _mtime.Value; set => _mtime.Value = value; }
    /// <summary>
    /// Do not use the Last-modified header to set
    /// the file modification time
    /// </summary>
    public bool NoMtime { get => _noMtime.Value; set => _noMtime.Value = value; }
    /// <summary>
    /// Write video description to a .description file
    /// </summary>
    public bool WriteDescription { get => _writeDescription.Value; set => _writeDescription.Value = value; }
    /// <summary>
    /// Do not write video description (default)
    /// </summary>
    public bool NoWriteDescription { get => _noWriteDescription.Value; set => _noWriteDescription.Value = value; }
    /// <summary>
    /// Write video metadata to a .info.json file
    /// (this may contain personal information)
    /// </summary>
    public bool WriteInfoJson { get => _writeInfoJson.Value; set => _writeInfoJson.Value = value; }
    /// <summary>
    /// Do not write video metadata (default)
    /// </summary>
    public bool NoWriteInfoJson { get => _noWriteInfoJson.Value; set => _noWriteInfoJson.Value = value; }
    /// <summary>
    /// Write playlist metadata in addition to the
    /// video metadata when using --write-info-json,
    /// --write-description etc. (default)
    /// </summary>
    public bool WritePlaylistMetafiles { get => _writePlaylistMetafiles.Value; set => _writePlaylistMetafiles.Value = value;}
    /// <summary>
    /// Do not write playlist metadata when using
    /// --write-info-json, --write-description etc.
    /// </summary>
    public bool NoWritePlaylistMetafiles { get => _noWritePlaylistMetafiles.Value; set => _noWritePlaylistMetafiles.Value = value; }
    /// <summary>
    /// Remove some private fields such as filenames
    /// from the infojson. Note that it could still
    /// contain some personal information (default)
    /// </summary>
    public bool CleanInfoJson { get => _cleanInfoJson.Value; set => _cleanInfoJson.Value = value; }
    /// <summary>
    /// Write all fields to the infojson
    /// </summary>
    public bool NoCleanInfoJson { get => _noCleanInfoJson.Value; set => _noCleanInfoJson.Value = value; }
    /// <summary>
    /// Retrieve video comments to be placed in the
    /// infojson. The comments are fetched even
    /// without this option if the extraction is
    /// known to be quick (Alias: --get-comments)
    /// </summary>
    public bool WriteComments { get => _writeComments.Value; set => _writeComments.Value = value; }
    /// <summary>
    /// Do not retrieve video comments unless the
    /// extraction is known to be quick (Alias:
    /// --no-get-comments)
    /// </summary>
    public bool NoWriteComments { get => _noWriteComments.Value; set => _noWriteComments.Value = value; }
    /// <summary>
    /// JSON file containing the video information
    /// (created with the "--write-info-json" option)
    /// </summary>
    public string LoadInfoJson { get => _loadInfoJson.Value; set => _loadInfoJson.Value = value; }
    /// <summary>
    /// Netscape formatted file to read cookies from
    /// and dump cookie jar in
    /// </summary>
    public bool Cookies { get => _cookies.Value; set => _cookies.Value = value; }
    /// <summary>
    /// Do not read/dump cookies from/to file
    /// (default)
    /// </summary>
    public bool NoCookies { get => _noCookies.Value; set => _noCookies.Value = value; }
    /// <summary>
    /// The name of the browser to load cookies
    /// from. Currently supported browsers are:
    /// brave, chrome, chromium, edge, firefox,
    /// opera, safari, vivaldi. Optionally, the
    /// KEYRING used for decrypting Chromium cookies
    /// on Linux, the name/path of the PROFILE to
    /// load cookies from, and the CONTAINER name
    /// (if Firefox) ("none" for no container) can
    /// be given with their respective seperators.
    /// By default, all containers of the most
    /// recently accessed profile are used.
    /// Currently supported keyrings are: basictext,
    /// gnomekeyring, kwallet
    /// </summary>
    public string CookiesFromBrowser { get => _cookiesFromBrowser.Value; set => _cookiesFromBrowser.Value = value; }
    /// <summary>
    /// Do not load cookies from browser (default)
    /// </summary>
    public bool NoCookiesFromBrowser { get => _noCookiesFromBrowser.Value; set => _noCookiesFromBrowser.Value = value; }
    /// <summary>
    /// Location in the filesystem where yt-dlp can
    /// store some downloaded information (such as
    /// client ids and signatures) permanently. By
    /// default ${XDG_CACHE_HOME}/yt-dlp
    /// </summary>
    public string CacheDir { get => _cacheDir.Value; set => _cacheDir.Value = value; }
    /// <summary>
    /// Disable filesystem caching
    /// </summary>
    public bool NoCacheDir { get => _noCacheDir.Value; set => _noCacheDir.Value = value; }
    /// <summary>
    /// Delete all filesystem cache files
    /// </summary>
    public bool RmCacheDir { get => _rmCacheDir.Value; set => _rmCacheDir.Value = value; }

}
