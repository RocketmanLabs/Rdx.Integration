using Rdx.Core.Exceptions;

namespace Rdx.Core;

public class CfgFile
{
    private string? _filename;

    public Exception? Error { get; private set; }
    public bool HasError => Error is not null;

    public CfgFile(string filename)
    {
        _filename = filename;
    }

    public string[] ReadText()
    {
        string[] lines = new string[0];
        try
        {
            if (_filename is null) throw new RdxNullException($"Configuration file name cannot be null.");
            lines = File.ReadAllLines(_filename);
        }
        catch (Exception ex)
        {
            Error = ex;
        }
        return lines;
    }
}
