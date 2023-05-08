using Rdx.Core.Interfaces;
using System.Diagnostics;

namespace Rdx.Core;

[DebuggerDisplay($"{{{nameof(Info)},nq}}")]
public class SystemDefinition : ISystemDefinition
{
    private string _id = String.Empty;
    private RdVersion _version = RdVersion.Zero;

    public SystemDefinition()
    {
        Id = Guid.NewGuid().ToString();
    }

    public SystemDefinition(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            _id = Guid.NewGuid().ToString();
        }
        else
        {
            _id = id;
        }
    }

    /// <summary>
    /// Id is set to the user's setting or a random GUID if it is still blank
    /// at the end of the ctor().
    /// </summary>
    public string Id { get => _id; set => _id = value; }
    public string Key { get; set; } = String.Empty;
    public string ProductName { get; set; } = String.Empty;
    public string Version { get => _version.FormattedVersion; set => _version = new RdVersion(value); }
    public ExternalSystemCode ExternalType { get; set; } = ExternalSystemCode.NOTHING;

    public bool IsEmpty => String.IsNullOrEmpty(Key) && String.IsNullOrEmpty(ProductName) && _version.IsZero;

    public static SystemDefinition Empty => new();

    private string Info => $"SysDef #{Id}: [{ExternalType}] {ProductName} v.{Version} ({Key})";
}

