namespace Rdx.Core.Exceptions;

public class RdxCountMismatchException : _RdxException
{
    public RdxCountMismatchException() : base() { }
    public RdxCountMismatchException(string? msg) : base(msg) { }
    public RdxCountMismatchException(string? msg, Exception? inner) : base(msg, inner) { }
}
