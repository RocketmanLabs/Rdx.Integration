namespace Rdx.Core.Exceptions;

// TODO: Add logging to the base exception class
public abstract class _RdxException : ApplicationException
{
    public _RdxException() : base() { }
    public _RdxException(string? msg) : base(msg) { }
    public _RdxException(string? msg, Exception? inner) : base(msg, inner) { }
}
