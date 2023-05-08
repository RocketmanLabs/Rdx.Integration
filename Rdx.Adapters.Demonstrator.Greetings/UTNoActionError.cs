using Rdx.ActionInterface;
using Rdx.Core;

namespace Rdx.Adapters;

/*:::::::::::::::::::::::::::::::::::::
RDX Action: UTNoAction 
-------------------
    When called with the stated Endpoint, demonstrates a 'missing Action' error 
    and should have been trapped in RdxAdapter. If the demo includes it, 
    this can be cured with an Override that adds UTNoAction to the Action stack.
Args:
    (none)
Return:
    ReturnValue = "This should never be returned"
    Endpoint = \utnoaction
 ::::::::::::::::::::::::::::::::::::::*/
public class UTNoActionError : _RdxAction
{
    /*
     _RdxAction: 
        string? Key { get; }
        Args ExpectedCriteria { get; }

        StatusCode Validate(Rq request, Rs response);
        string EtlExtract(SystemConnection cs, Rq request, Rs response);
        string EtlFormatReturn(SystemConnection rdProduct, Rq request, Rs response);

        bool HasError { get; }
        Stack<Exception> Errors { get; }
     */

    public UTNoActionError() : base(
        "UTNoActionError",
        new Args())
    { }

    public override StatusCode Validate(Rq request, Rs response) => base.Validate(request, response);

    public override StatusCode EtlExtract(SystemConnection cs, Rq request, Rs response)
    {
        // No CS is involved in this "calculation only" demonstration.
        response.RawResponse = "This should never be returned.";
        return StatusCode.OK;
    }

    public override StatusCode EtlFormatReturn(SystemConnection rdProduct, Rq request, Rs response)
    {
        response.ReturnValue = response.RawResponse;
        return StatusCode.OK;
    }
}