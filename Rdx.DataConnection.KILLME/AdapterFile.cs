namespace Rdx.DataConnection;

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
