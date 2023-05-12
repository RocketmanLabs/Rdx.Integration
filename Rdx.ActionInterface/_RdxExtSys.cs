using Rdx.Core;
using Rdx.Core.Exceptions;
using System.Diagnostics;

namespace Rdx.ActionInterface;

/*
     _RdxExtSys: 
        string? RdKey { get; set; }
        string? RdVersion { get; set; }
        string? CsKey { get; set; }
        string? CsVersion { get; set; }

        string ToString(); // returns DLL name, as in ENG(2-3-0)=IMP(100-3-0)

        Exception? Errors { get; }
        bool HasError { get; }
*/

/// <summary>
/// Each _RdxExtSys instance is associated with a single Adapter.  It is found 
/// and loaded during the Discovery() process.
/// 
/// _RdxExtSys contains the identities of the systems on either side of the RDX
/// middleware. 
/// </summary>
[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
public abstract class _RdxExtSys
{
    protected _RdxExtSys() { }

    protected _RdxExtSys(SystemDefinition sysDefRd, SystemDefinition sysDefCs)
    {
        Rd = sysDefRd;
        Cs = sysDefCs;
    }

    public SystemDefinition Rd { get; private set; } = new();
    public SystemDefinition Cs { get; private set; } = new();


    public Exception? Error { get; private set; }
    public bool HasError => Error is not null;

    /// <summary>
    /// Returns true if sysDef is in ExternalSystems
    /// </summary>
    public virtual bool Match(SystemDefinition sysDef)
    {
        if (sysDef.ExternalType.HasFlag(ExternalSystemCode.RD))
        {
            return String.Compare(sysDef.Key, Rd.Key, StringComparison.OrdinalIgnoreCase) == 0
                && sysDef.Version == Rd.Version;
        }
        else if (sysDef.ExternalType.HasFlag(ExternalSystemCode.CS))
        {
            return String.Compare(sysDef.Key, Cs.Key, StringComparison.OrdinalIgnoreCase) == 0
                && sysDef.Version == Cs.Version;
        }
        Error = new RdxNullException("Operation halted - Match() could not proceed since the 'xsys' argument does not define the external system.");
        return false;
    }

    public override string ToString() => $"{Rd.Key ?? "error"}({Rd.Version ?? "error"})={Cs.Key ?? "error"}({Cs.Version ?? "error"})";
}
