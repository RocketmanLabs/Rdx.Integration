using Rdx.Core.Exceptions;
using Rdx.Core.Interfaces;
using System.Diagnostics;
using System.Text;

namespace Rdx.Core;

[DebuggerDisplay($"{{{nameof(Info)},nq}}")]
public record SystemDefinition : ISystemDefinition, IEquatable<SystemDefinition>
{
    private RdVersion _version = RdVersion.Zero;
    private string _key = String.Empty;

    /// <summary>
    /// Ctor that generates Id from a random GUID.
    /// </summary>
    public SystemDefinition()
    {
        Id = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Ctor that all
    /// </summary>
    /// <param name="id"></param>
    public SystemDefinition(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Id = Guid.NewGuid().ToString();
        }
        else
        {
            Id = id;
        }
    }

    public const char DELIMITER = '|';
    private const int NUM_PARTS = 5;

    /// <summary>
    /// Id is set to the user's setting or a random GUID if it is still blank
    /// at the end of the ctor().
    /// </summary>
    public string Id { get; set; } = String.Empty;
    public string Key { get => _key; set => _key = value.ToUpperInvariant(); }
    public string ProductName { get; set; } = String.Empty;

    // this property is backed by an RdVersion instance to use its rules, but exposed as a string for simplicity
    public string Version { get => _version.FormattedVersion; set => _version = new RdVersion(value); }
    public ExternalSystemCode ExternalType { get; set; } = ExternalSystemCode.NOTHING;

    public bool IsEmpty => String.IsNullOrEmpty(Key) && String.IsNullOrEmpty(ProductName) && _version.IsZero;

    /// <summary>
    /// Returns SystemDefinition as KEY(v-v-v), where v = version major, minor, revision.
    /// </summary>
    public string ToSystemName() => string.Concat(Key,"(",_version.AltFormattedVersion, ")");

    //public static SystemDefinition ParseSystemDefinition(string line)
    //{
    //    var parts = line.Split(DELIMITER);
    //    if (parts.Length != NUM_PARTS) throw new RdxMissingItemException($"Cannot parse {line} into a system definition.");
    //    var sysDef = new SystemDefinition(parts[0]);
    //    sysDef.Key = parts[1];
    //    sysDef.ProductName = parts[2];
    //    sysDef.Version = parts[3];
    //    sysDef.ExternalType = (ExternalSystemCode)Enum.Parse(typeof(ExternalSystemCode), parts[4]);
    //    return sysDef;
    //}

    //public static string ToCfgFileFormat(SystemDefinition sysDef) {
    //    StringBuilder sb = new();
    //    sb.Append(sysDef.Id);
    //    sb.Append(DELIMITER);
    //    sb.Append(sysDef.Key);
    //    sb.Append(DELIMITER);
    //    sb.Append(sysDef.ProductName);
    //    sb.Append(DELIMITER);
    //    sb.Append(sysDef.Version);
    //    sb.Append(DELIMITER);
    //    sb.Append(((int)sysDef.ExternalType).ToString());
    //    return sb.ToString();
    //}

    private string Info => $"SysDef #{Id}: [{ExternalType}] {ProductName} v.{Version} ({Key})";

    public static SystemDefinition Empty => new();
}

