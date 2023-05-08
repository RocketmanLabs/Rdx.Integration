using Rdx.ActionInterface;
using Rdx.Core;

namespace Rdx.Adapters.Demonstrator.Greetings;

/*:::::::::::::::::::::::::::::::::::::
RDX Action: Goodbye 
-------------------
    Test action with no arguments, no communication with customer systems
Args:
    (none)
Return:
    ReturnValue = "Goodbye!!"
    Endpoint = \goodbye
 ::::::::::::::::::::::::::::::::::::::*/
public class Goodbye : _RdxAction
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

    public Goodbye() : base(
        "Goodbye",
        new Args())
    { }

    public override StatusCode Validate(Rq request, Rs response) => base.Validate(request, response);

    public override StatusCode EtlExtract(SystemConnection cs, Rq request, Rs response)
    {
        // No CS is involved in this "calculation only" demonstration.
        response.RawResponse = "Goodbye!!";
        return StatusCode.OK;
    }

    public override StatusCode EtlFormatReturn(SystemConnection rdProduct, Rq request, Rs response)
    {
        response.ReturnValue = response.RawResponse;
        return StatusCode.OK;
    }
}