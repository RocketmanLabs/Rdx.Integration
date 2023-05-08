using System.Diagnostics;

namespace Rdx.Core;

/*:::::::::::::::::::::::::::::::::::::
Arg - Key=Value container to help build query strings in URLs.
In addition to passing values to Etl functions, Arg instances
can serve to indicate required Args for an action method, and
can supply default values for uninitialized Args.

The Arg.ValueSet property, when false, indicates an 
uninitialized Arg.
:::::::::::::::::::::::::::::::::::::::*/
[DebuggerDisplay($"{{{nameof(Info)},nq}}")]
public class Arg
{
    private string? _value;

    public Arg() { }

    public Arg(string key, string? value, string? description)
    {
        Key = key;
        Value = value;
        ValueSet = true;
        Description = description;
    }

    public string? Key { get; set; }
    public string? Value { get { return _value; } set { _value = value; ValueSet = true; } }
    public string? Description { get; set; }

    /// <summary>
    /// When true, indicates that a null value for Value was deliberately set 
    /// that way, when false, a null value is the instance default.
    /// </summary>
    public bool ValueSet { get; set; }

    private string Info => $"{Key} = '{valueInfo()}'{valueSetInfo()}";
    private string valueInfo() => Value is null ? "<null>" : Value == "" ? "<empty>" : Value;
    private string valueSetInfo() => ValueSet ? "" : " [VALUE NOT SET]";
}
