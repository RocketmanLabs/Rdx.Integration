namespace Rdx.Core.Exceptions;

public class RdxNotFoundException : _RdxException
{
    public RdxNotFoundException() : base() { }
    public RdxNotFoundException(string? msg) : base(msg) { }
    public RdxNotFoundException(string? msg, Exception? inner) : base(msg, inner) { }
}