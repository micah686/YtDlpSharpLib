namespace YtDlpSharpLib.Options;

/// <summary>
/// Maps a typed option property to the exact yt-dlp command-line flag.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class YtDlpArgumentAttribute(string name) : Attribute
{
    /// <summary>The yt-dlp flag (long form, including leading double-dashes).</summary>
    public string Name { get; } = name;

    /// <summary>How the value is rendered. Defaults to <see cref="ArgumentValueStyle.SeparateToken"/>.</summary>
    public ArgumentValueStyle ValueStyle { get; init; } = ArgumentValueStyle.SeparateToken;

    /// <summary>Indicates whether the property is a collection that may emit the flag multiple times.</summary>
    public bool AllowMultiple { get; init; }
}
