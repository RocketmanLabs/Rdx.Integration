using System.Data.Common;

namespace Rdx.Core.Exceptions;

public class RdxInvalidInstanceException : _RdxException
{
    public RdxInvalidInstanceException() : base() { }
    public RdxInvalidInstanceException(string? msg) : base(msg) { }
    public RdxInvalidInstanceException(string? msg, Exception? inner) : base(msg, inner) { }
}
