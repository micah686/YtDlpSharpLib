using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;

namespace YtDlpSharpLib.Options;

/// <summary>Helpers for composing and comparing <see cref="YtDlpOptions"/> instances.</summary>
public static class YtDlpOptionsExtensions
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<PropertyInfo>> PropertyCache = new();
    private static readonly IReadOnlyList<PropertyInfo> GroupProperties = GetOptionGroupProperties();

    /// <summary>
    /// Composes two option sets, letting explicitly set values from <paramref name="overrideOptions"/>
    /// replace values from <paramref name="baseOptions"/>.
    /// </summary>
    /// <remarks>
    /// When <paramref name="forceOverride"/> is false, unset values are ignored. For this options model,
    /// unset means <see langword="null"/> strings/nullable values, <see langword="false"/> booleans, and empty
    /// collections. When <paramref name="forceOverride"/> is true, all values from <paramref name="overrideOptions"/>
    /// are copied, including unset/default values.
    /// </remarks>
    public static YtDlpOptions OverrideOptions(
        this YtDlpOptions baseOptions,
        YtDlpOptions overrideOptions,
        bool forceOverride = false)
    {
        ArgumentNullException.ThrowIfNull(baseOptions);
        ArgumentNullException.ThrowIfNull(overrideOptions);

        var result = new YtDlpOptions();
        foreach (var groupProperty in GroupProperties)
        {
            var mergedGroup = MergeGroup(
                groupProperty.GetValue(baseOptions),
                groupProperty.GetValue(overrideOptions),
                groupProperty.PropertyType,
                forceOverride);
            groupProperty.SetValue(result, mergedGroup);
        }

        result = result with
        {
            AdvancedArguments = forceOverride || overrideOptions.AdvancedArguments.Count > 0
                ? overrideOptions.AdvancedArguments
                : baseOptions.AdvancedArguments
        };

        return result;
    }

    /// <summary>Adds a typed custom argument to <see cref="YtDlpOptions.AdvancedArguments"/>.</summary>
    public static YtDlpOptions AddCustomOption<T>(
        this YtDlpOptions options,
        string name,
        T value,
        bool isSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(options);

        return options with
        {
            AdvancedArguments = options.AdvancedArguments
                .Append(RawYtDlpArgument.Create(name, value, isSensitive))
                .ToArray()
        };
    }

    /// <summary>Sets a typed custom argument, replacing existing advanced arguments with the same name.</summary>
    public static YtDlpOptions SetCustomOption<T>(
        this YtDlpOptions options,
        string name,
        T value,
        bool isSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return options with
        {
            AdvancedArguments = options.AdvancedArguments
                .Where(argument => !string.Equals(argument.Name, name, StringComparison.Ordinal))
                .Append(RawYtDlpArgument.Create(name, value, isSensitive))
                .ToArray()
        };
    }

    /// <summary>Removes all advanced arguments with the supplied name.</summary>
    public static YtDlpOptions DeleteCustomOption(this YtDlpOptions options, string name)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return options with
        {
            AdvancedArguments = options.AdvancedArguments
                .Where(argument => !string.Equals(argument.Name, name, StringComparison.Ordinal))
                .ToArray()
        };
    }

    internal static bool ValueEquals(YtDlpOptions? left, YtDlpOptions? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        foreach (var groupProperty in GroupProperties)
        {
            if (!GroupValuesEqual(groupProperty.GetValue(left), groupProperty.GetValue(right), groupProperty.PropertyType))
            {
                return false;
            }
        }

        return ValuesEqual(left.AdvancedArguments, right.AdvancedArguments);
    }

    internal static int ValueHashCode(YtDlpOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var hash = new HashCode();
        foreach (var groupProperty in GroupProperties)
        {
            AddGroupHashCode(ref hash, groupProperty.GetValue(options), groupProperty.PropertyType);
        }

        AddValueHashCode(ref hash, options.AdvancedArguments);
        return hash.ToHashCode();
    }

    private static object MergeGroup(
        object? baseGroup,
        object? overrideGroup,
        Type groupType,
        bool forceOverride)
    {
        var result = Activator.CreateInstance(groupType)
            ?? throw new InvalidOperationException($"Could not create option group '{groupType.Name}'.");

        foreach (var property in GetProperties(groupType))
        {
            var overrideValue = overrideGroup is null ? null : property.GetValue(overrideGroup);
            var value = forceOverride || IsSetValue(overrideValue)
                ? overrideValue
                : property.GetValue(baseGroup);
            property.SetValue(result, value);
        }

        return result;
    }

    private static bool GroupValuesEqual(object? left, object? right, Type groupType)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        foreach (var property in GetProperties(groupType))
        {
            if (!ValuesEqual(property.GetValue(left), property.GetValue(right)))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsSetValue(object? value) =>
        value switch
        {
            null => false,
            string => true,
            bool flag => flag,
            IEnumerable sequence => sequence.Cast<object?>().Any(),
            _ => true
        };

    private static bool ValuesEqual(object? left, object? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        if (left is string || right is string)
        {
            return left.Equals(right);
        }

        if (left is IEnumerable leftSequence && right is IEnumerable rightSequence)
        {
            return leftSequence.Cast<object?>().SequenceEqual(
                rightSequence.Cast<object?>(),
                SequenceItemComparer.Instance);
        }

        return left.Equals(right);
    }

    private static void AddValueHashCode(ref HashCode hash, object? value)
    {
        if (value is null)
        {
            hash.Add(0);
            return;
        }

        if (value is string)
        {
            hash.Add(value);
            return;
        }

        if (value is IEnumerable sequence)
        {
            foreach (var item in sequence)
            {
                AddValueHashCode(ref hash, item);
            }

            return;
        }

        hash.Add(value);
    }

    private static void AddGroupHashCode(ref HashCode hash, object? group, Type groupType)
    {
        if (group is null)
        {
            hash.Add(0);
            return;
        }

        foreach (var property in GetProperties(groupType))
        {
            AddValueHashCode(ref hash, property.GetValue(group));
        }
    }

    private static IReadOnlyList<PropertyInfo> GetOptionGroupProperties() =>
        typeof(YtDlpOptions)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.GetCustomAttribute<YtDlpOptionGroupAttribute>() is not null)
            .OrderBy(property => property.GetCustomAttribute<YtDlpOptionGroupAttribute>()!.Order)
            .ThenBy(static property => property.MetadataToken)
            .ToArray();

    private static IReadOnlyList<PropertyInfo> GetProperties(Type type) =>
        PropertyCache.GetOrAdd(
            type,
            static key => key
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(static property => property.MetadataToken)
                .ToArray());

    private sealed class SequenceItemComparer : IEqualityComparer<object?>
    {
        public static readonly SequenceItemComparer Instance = new();

        public new bool Equals(object? x, object? y) =>
            ValuesEqual(x, y);

        public int GetHashCode(object? obj)
        {
            var hash = new HashCode();
            AddValueHashCode(ref hash, obj);
            return hash.ToHashCode();
        }
    }
}

/// <summary>Compares <see cref="YtDlpOptions"/> instances by option values, including sequence contents.</summary>
public sealed class OptionComparer : IEqualityComparer<YtDlpOptions>
{
    /// <summary>Shared comparer instance.</summary>
    public static OptionComparer Instance { get; } = new();

    /// <inheritdoc />
    public bool Equals(YtDlpOptions? x, YtDlpOptions? y) =>
        YtDlpOptionsExtensions.ValueEquals(x, y);

    /// <inheritdoc />
    public int GetHashCode(YtDlpOptions obj) =>
        YtDlpOptionsExtensions.ValueHashCode(obj);
}
