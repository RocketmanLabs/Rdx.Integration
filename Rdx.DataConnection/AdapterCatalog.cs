namespace Rdx.DataConnection;

public class AdapterCatalog : Dictionary<string, AdapterFile>
{
    public void Add(AdapterFile filespec)
    {
        Add(filespec.Key, filespec);
    }
}
