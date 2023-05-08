namespace Rdx.Core.Exceptions;

public class RdxDuplicateException : _RdxException
{
    public RdxDuplicateException() : base() { }
    public RdxDuplicateException(string? msg) : base(msg) { }
    public RdxDuplicateException(string? msg, Exception? inner) : base(msg, inner) { }
}