using Rdx.Core.Exceptions;
using System.Text;

namespace Rdx.Core;

/// <summary>
/// The ExternalSystems class contains customer information and information
/// about the upstream RD Product and downstream Customer Systems.  The
/// ToCfgFileFormat() method produces Markdown file contents.
/// </summary>
public class ExternalSystems : Dictionary<string, SystemDefinition>
{
    public const string CUSTOMER_INFO = "# Customer Info";
    public const string EXTERNAL_SYSTEM = "# External System";

    public ExternalSystems() { }

    public string? FullPath { get; set; }
    public string? CustomerOrganizationName { get; set; }

    public Exception? Error { get; private set; }
    public bool HasError => Error is not null;

    public void Add(SystemDefinition sysdef) => Add(sysdef.Key, sysdef);

    //public StatusCode WriteCfgFile()
    //{
    //    try
    //    {
    //        if (FullPath is null) throw new RdxNullException("External Systems configuration file cannot be written, FullPath is null.");
    //        var s = ToCfgFileFormat(this);
    //        if (s is null) throw new RdxNullException("External Systems configuration file cannot be written, formatting failed.");
    //        File.WriteAllText(FullPath, s);
    //        return StatusCode.OK;
    //    }
    //    catch (Exception ex)
    //    {
    //        Error = ex;
    //        return StatusCode.ERROR;
    //    }
    //}

    //public static string ToCfgFileFormat(ExternalSystems xsys)
    //{
    //    StringBuilder sb = new(); 
    //    sb.AppendLine(CUSTOMER_INFO);
    //    sb.AppendLine(xsys.CustomerOrganizationName);
    //    int counter = 1;
    //    foreach (SystemDefinition sysDef in xsys.Values)
    //    {
    //        sb.AppendLine($"{EXTERNAL_SYSTEM} {counter}");
    //        sb.AppendLine(SystemDefinition.ToCfgFileFormat(sysDef));
    //    }
    //    return sb.ToString();
    //}

    //public static ExternalSystems ReadConfigurationFile(string filename)
    //{
    //    ExternalSystems xsys = new();
    //    try
    //    {
    //        var lines = File.ReadAllLines(filename);
    //        int lineMax = lines.Length;
    //        int targetIndex = GlobalConstants.ELEMENT_NOT_FOUND;
    //        for (int index = 0; index < lineMax; index++)
    //        {
    //            if (lines[index].StartsWith(CUSTOMER_INFO)) targetIndex = index + 1;
    //            if (index == targetIndex)
    //            {
    //                xsys.CustomerOrganizationName = lines[index].Trim();
    //                continue;
    //            }
    //            if (lines[index].StartsWith(EXTERNAL_SYSTEM)) targetIndex = index + 1;
    //            if (index == targetIndex)
    //            {
    //                SystemDefinition? sysDef = SystemDefinition.ParseSystemDefinition(lines[index]);
    //                if (sysDef is null) throw new RdxInvalidInstanceException($"Cannot parse SystemDefinition from '{lines[index]}'.");
    //                xsys.Add(sysDef);
    //                continue;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        xsys.Error = ex;
    //    }
    //    return xsys;
    //}

    public static ExternalSystems DevTestLoad()
    {
        ExternalSystems cfg = new();
        SystemDefinition rd = new("");  // generates ID
        rd.Key = "EMU";
        rd.ProductName = "Engagefully Emulator";
        rd.Version = "1.0.0.0";
        rd.ExternalType = ExternalSystemCode.RD | ExternalSystemCode.MOCK; // MOCK skips login, etc.

        cfg.Add(rd);

        SystemDefinition cs = new("");
        cs.Key = "SIM";
        cs.ProductName = "Customer System Simulator";
        cs.Version = "1.0.0.0";
        cs.ExternalType = ExternalSystemCode.CS | ExternalSystemCode.MOCK;

        cfg.Add(cs);

        cfg.CustomerOrganizationName = "Results Direct";
        cfg.FullPath = "";                                  // discourages file operations
        return cfg;
    }
}
