using Rdx.ActionInterface;
using Rdx.Core;

namespace Rdx.Adapters.Demonstrator.Greetings;

/*:::::::::::::::::::::::::::::::::::::
RDX Action: UTMissingArg
-------------------
    Test action that causes a "missing argument" error in the Demonstrator Adapter.
Args:
    x - a fake arg value
    y - a fake arg value
    z - a missing fake arg value to demonstrate error handling
Return:
    ReturnValue = "Success (UTMissingArg)"
    Endpoint = utmissingarg

    MissingArg causes a 400 Bad Request error, and Rs.Message should have the details.
 ::::::::::::::::::::::::::::::::::::::*/
public class UTMissingArg : _RdxAction
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

    public UTMissingArg() : base(
        "UTMissingArg",
        new Args(
            new Arg("x", "1", "fake arg"),
            new Arg("y", "2", "fake arg")
            //new Arg("z", "3", "fake arg")
            ))
    { }

    public override StatusCode Validate(Rq request, Rs response) => base.Validate(request, response);

    public override StatusCode EtlExtract(SystemConnection cs, Rq request, Rs response)
    {
        // No CS is involved in this "calculation only" demonstration.
        response.RawResponse = "Success (UTMissingArg)";
        return StatusCode.OK;
    }

    public override StatusCode EtlFormatReturn(SystemConnection rdProduct, Rq request, Rs response)
    {
        response.ReturnValue = response.RawResponse;
        return StatusCode.OK;
    }
}

