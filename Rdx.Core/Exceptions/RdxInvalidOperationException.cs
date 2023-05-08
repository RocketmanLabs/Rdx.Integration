namespace Rdx.Core.Exceptions;

public class RdxInvalidOperationException : _RdxException
{
    public RdxInvalidOperationException() : base() { }
    public RdxInvalidOperationException(string? msg) : base(msg) { }
    public RdxInvalidOperationException(string? msg, Exception? inner) : base(msg, inner) { }
}
