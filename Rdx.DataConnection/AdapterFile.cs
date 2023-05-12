using Rdx.ActionInterface;
using Rdx.Core;

namespace Rdx.DataConnection;


/// <summary>
/// The Adapter file is personalized with an AdapterFile instance.  This class
/// is responsible for confirming the AdapterFile represents a type expected
/// by the ExternalSystems catalog.  If so, it them loads the DLL and activates
/// each _RdxAction.
/// </summary>
public class Adapter
{

    public Adapter(ExternalSystems xsys, AdapterFile dll)
    {
        Dll = dll;
    }

    public AdapterFile Dll { get; private set; }

    public Exception? Error { get; set; }
    public bool HasError => Error is not null;

    /// <summary>
    /// Loads ActionCatalog and 
    /// </summary>
    //public StatusCode LoadDll(ExternalSystems xsys)
    //{
    //    _dll = dll;
    //    if (dll.Key is null)
    //}
}

public class AdapterFile
{
    public AdapterFile(string fullPath, bool isOverride = false)
    {
        Url = new(fullPath);
        IsOverride = isOverride;
    }

    public Uri? Url { get; set; }
    public string Key => Url?.AbsolutePath ?? string.Empty;
    public bool IsOverride { get; set; }
}
