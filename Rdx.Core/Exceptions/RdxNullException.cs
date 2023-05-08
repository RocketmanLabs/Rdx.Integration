namespace Rdx.Core.Exceptions;

public class RdxNullException : _RdxException
{
    public RdxNullException() : base() { }
    public RdxNullException(string? msg) : base(msg) { }
    public RdxNullException(string? msg, Exception? inner) : base(msg, inner) { }
}