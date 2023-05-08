using Rdx.Core.Interfaces;
using System.Text;

namespace Rdx.Core;

/// <summary>
/// The ExternalSystems class contains customer information and information
/// about the upstream RD Product and downstream Customer Systems.  The
/// ToConfigurationFile() method produces Markdown file contents.
/// </summary>
public class ExternalSystems : Dictionary<string, ISystemDefinition>
{
    public const string CUSTOMER_INFO = "# Customer Info";
    public const string EXTERNAL_SYSTEM = "# External System";

    public ExternalSystems() { }

    public string? Filename { get; set; }
    public string? CustomerOrganizationName { get; set; }

    public Exception? Error { get; private set; }
    public bool HasError => Error is not null;

    public void Add(ISystemDefinition sysdef) => Add(sysdef.Key, sysdef);


    public string ToConfigurationFile(string customerOrgName)
    {
        StringBuilder sb = new();
        sb.AppendLine($"# {CUSTOMER_INFO}");
        sb.AppendLine(CustomerOrganizationName);
        int counter = 1;
        foreach (ISystemDefinition sysDef in this.Values)
        {
            sb.AppendLine($"{EXTERNAL_SYSTEM} {counter}");
            sb.AppendLine($"{this.Values.ElementAt(counter - 1)}");
        }
        return sb.ToString();
    }

    public static ExternalSystems ReadConfigurationFile(string filename)
    {
        ExternalSystems xsys = new();
        try
        {
            var lines = File.ReadAllLines(filename);
            int lineMax = lines.Length;
            // TODO: Get orgname
            // TODO: load systems info into dictionary
        }
        catch (Exception ex)
        {
            xsys.Error = ex;
        }
    }
}
