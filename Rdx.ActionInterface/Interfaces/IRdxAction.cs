using Rdx.Core;

namespace Rdx.ActionInterface.Interfaces
{
    public interface IRdxAction
    {
        string? Key { get; }
        Args ExpectedCriteria { get; }

        bool HasError { get; }
        Stack<Exception> Errors { get; }

        StatusCode EtlExtract(SystemConnection cs, Rq request, Rs response);
        StatusCode EtlFormatReturn(SystemConnection rdProduct, Rq request, Rs response);
        StatusCode Validate(Rq request, Rs response);
    }
}