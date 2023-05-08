using Rdx.ActionInterface;
using Rdx.Core;

namespace Rdx.Adapters;

/*:::::::::::::::::::::::::::::::::::::
RDX Action: Hello 
-------------------
    Test action using a single argument, no communication with customer
    systems
Args:
    username - name to include in greeting
Return:
    Value = "Hello, {username}"
    Endpoint = goodbye
 ::::::::::::::::::::::::::::::::::::::*/
public class Hello : _RdxAction
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

    public Hello() : base(
        "Hello",
        new Args(
            new Arg("username", null, null)
        ))
    { }

    public override StatusCode Validate(Rq request, Rs response) => base.Validate(request, response);

    public override StatusCode EtlExtract(SystemConnection cs, Rq request, Rs response)
    {
        // No CS is involved in this "calculation only" demonstration.
        response.RawResponse = $"Hello, {request.Criteria["username"]}";
        return StatusCode.OK;
    }

    public override StatusCode EtlFormatReturn(SystemConnection rdProduct, Rq request, Rs response)
    {
        response.ReturnValue = response.RawResponse;
        return StatusCode.OK;
    }
}