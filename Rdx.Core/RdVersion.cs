using Rdx.Core.Exceptions;
using System.Diagnostics;
using System.Xml.Linq;

namespace Rdx.Core;

/// <summary>
/// Implements RD's form of the version property.  This will be refined as
/// development progresses. The following rules are implemented:
/// 1. Incrementing major zeroes minor and revision
/// 2. Incrementing minor zeroes revision
/// 3. Incrementing revision has no other effects
/// 4. Incrementing build has no other effects
/// 5. When comparing two RdVersion instances, only major, minor, and revision are compared
/// 6. When testing for equality between two RdVersion instances, only major and minor must be equal
/// </summary>
[DebuggerDisplay($"{{{nameof(Info)},nq}}")]
public class RdVersion : IEquatable<RdVersion>, IComparable<RdVersion>
{
    // TODO: remove DELIMITER option and settle on '.'

    private const string DELIMITER = ".";
    private const string ALT_DELIMITER = "-";

    private int _major, _minor, _revision, _build;

    public RdVersion() { }

    public RdVersion(string version, string delimiter = ".")
    {
        _ = Parse(version, delimiter);
    }

    public RdVersion(int major, int minor, int revision, int build)
    {
        _major = major;
        _minor = minor;
        _revision = revision;
        _build = build;
    }

    public bool IsZero => _major == 0 && _minor == 0 && _revision == 0 && _build == 0;

    /// <summary>
    /// Converts a version string ("1.2.3" or "1.2.3.4") into 4 numeric parts.  Only
    /// the last part of a 4-element version is optional,  Throws an exception if
    /// there are any conversion problems.
    /// </summary>
    public virtual string Parse(string version, string delimiter = DELIMITER)
    {
        var parts = version.Split(delimiter);
        try
        {
            _major = int.Parse(parts[0]);
            _minor = int.Parse(parts[1]);
            _revision = int.Parse(parts[2]);
            if (parts.Length > 3)
            {
                _build = int.Parse(parts[3]);
            }
            return FormattedVersion;

        } catch (Exception)
        {
            throw new RdxInvalidInstanceException($"Cannot parse '{version}' into a meaningful Version.");
        }
    }

    /// <summary>
    /// Increment on releases where a breaking change is known or probable.
    /// </summary>
    public string IncrementMajor()
    {
        _major += 1;
        _minor = 0;
        _revision = 0;
        return FormattedVersion;
    }

    /// <summary>
    /// Increment on releases where there are no breaking changes.
    /// </summary>
    public string IncrementMinor()
    {
        _minor += 1;
        _revision = 0;
        return FormattedVersion;
    }

    /// <summary>
    /// Increment on releases where only non-functional changes have been made,
    /// such as appearance, spelling, or content changes.
    /// </summary>
    public string IncrementRevision()
    {
        _revision += 1;
        return FormattedVersion;
    }

    /// <summary>
    /// Increment each time the code is re-compiled.
    /// </summary>
    public string IncrementBuild()
    {
        _build += 1;
        return FormattedVersion;
    }

    /// <summary>
    /// Returns version as a four-valued string in the dotted form: 3.4.5.6
    /// </summary>
    public string FormattedVersion => $"{_major}{DELIMITER}{_minor}{DELIMITER}{_revision}{DELIMITER}{_build}";

    /// <summary>
    /// Returns version as a three-valued string in the dashed form: 3-4-5
    /// </summary>
    public string AltFormattedVersion => $"{_major}{ALT_DELIMITER}{_minor}{ALT_DELIMITER}{_revision}";

    /// <summary>
    /// Compares major, minor, and revision values - build is ignored.
    /// </summary>
    public int CompareTo(RdVersion? other)
    {
        if (other is null) return (int)StatusCode.COMPARES_AS_MORE;
        if (this._major > other._major) return (int)StatusCode.COMPARES_AS_MORE;
        if (this._minor > other._minor) return (int)StatusCode.COMPARES_AS_MORE;
        if (this._revision > other._revision) return (int)StatusCode.COMPARES_AS_MORE;

        if (this._major < other._major) return (int)StatusCode.COMPARES_AS_LESS;
        if (this._minor < other._minor) return (int)StatusCode.COMPARES_AS_LESS;
        if (this._revision < other._revision) return (int)StatusCode.COMPARES_AS_LESS;
        return (int)StatusCode.OK;  // equal
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        RdVersion? other = obj as RdVersion;
        return Equals(other);
    }

    public bool Equals(RdVersion? other)
    {
        return other is not null
            && _major == other._major
            && _minor == other._minor
            && _revision == other._revision;
    }

    public override int GetHashCode() => HashCode.Combine(_major, _minor, _revision);

    public static bool operator ==(RdVersion left, RdVersion right) => left is null ? right is null : left.Equals(right);
    public static bool operator !=(RdVersion left, RdVersion right) => left is null ? right is not null : !left.Equals(right);

    public static bool operator <(RdVersion left, RdVersion right) => left is null ? right is not null : left.CompareTo(right) > 0;
    public static bool operator <=(RdVersion left, RdVersion right) => left is null || left.CompareTo(right) >= 0;
    public static bool operator >(RdVersion left, RdVersion right) => left is not null && left.CompareTo(right) < 0;
    public static bool operator >=(RdVersion left, RdVersion right) => left is null ? right is null : left.CompareTo(right) <= 0;

    public static RdVersion Zero => new RdVersion("0.0.0.0");

    private string Info => $"{FormattedVersion} ({AltFormattedVersion})";
}
