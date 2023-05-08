namespace Rdx.Core.Exceptions;

public class RdxMissingItemException : _RdxException
{
    public RdxMissingItemException() : base() { }
    public RdxMissingItemException(string? msg) : base(msg) { }
    public RdxMissingItemException(string? msg, Exception? inner) : base(msg, inner) { }
}