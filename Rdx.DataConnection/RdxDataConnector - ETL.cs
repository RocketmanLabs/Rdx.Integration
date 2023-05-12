using Rdx.ActionInterface;
using Rdx.Core.Exceptions;
using Rdx.Core;

namespace Rdx.DataConnection;


/// <summary>
/// This section of the class definition covers the ETL behaviors. ETL
/// includes processing incoming Requests, identifying and activating
/// the appropriate _RdxAction entry from the ActionCatalog, making 
/// calls to the customer systems, and assembling the returned values 
/// to match the desired data model.
/// </summary>
public partial class RdxDataConnector
{
    /// <summary>
    /// The Execute method performs the following:
    /// 
    /// 1. Checks the upstream and downstream external systems and ensures
    /// they have both authenticated this service.  
    /// 2. Looks up the Action class (type == _RdxAction) and calls its Validate, 
    /// EtlExtract, EtlFormatReturn methods.
    /// 3. Calls the CS 1..n times to gather the necessary data elements needed
    /// to fulfill the request.
    /// 4. Reformats the returned data to the requested data model.
    /// 5. Posts the return data to the RD Product.
    /// 6. Any errors encountered are stored in the response packet.
    /// </summary>
    public virtual StatusCode Execute(SystemConnection rd, SystemConnection cs, Rq request, Rs response)
    {
        // ensure we are linked to the RD Product ("originator") and the CS ("receiver") and both have authenticated this service
        _RdxAction? action = null;
        response.Errors.Clear();
        response.HttpStatusCode = GlobalConstants.INTERNAL_ERROR;  // pessimistic
        response.Message = "Encountered error while validating connections to external systems - attempting to query " +
            $"'{request.QueryEndpoint}'.";

        try
        {
            Originator = rd;
            Receiver = cs;

            if (rd.IsEmpty) throw new RdxInvalidInstanceException($"The connection to the originating system cannot be empty.");
            if (cs.IsEmpty) throw new RdxInvalidInstanceException($"The connection to the receiving system cannot be empty.");
            if (!rd.IsAuthenticated)
            {
                response.HttpStatusCode = GlobalConstants.AUTH_FAIL;
                throw new RdxInvalidInstanceException($"The RDX service has not been authenticated to the originating system.");
            }
            if (!cs.IsAuthenticated)
            {
                response.HttpStatusCode = GlobalConstants.AUTH_FAIL;
                throw new RdxInvalidInstanceException($"The RDX service has not been authenticated to the receiving system.");
            }

            // find action method in DLL
            action = _actions.Find(request.QueryEndpoint);
            if (action is null) throw new RdxInvalidOperationException($"Could not find a matching action for '{request.QueryEndpoint}'.");

            // E = Extract
            response.Message = $"Encountered error while extracting data.";
            if (action.EtlExtract(cs, request, response) != StatusCode.OK) return response.HasError
                    ? StatusCode.ERROR
                    : throw new RdxInvalidOperationException($"ETL halted in Adapter '{Key}' attempting to query '{request.QueryEndpoint}'.");

            // T = Transform
            response.Message = $"Encountered error while transforming data.";
            if (action.EtlFormatReturn(cs, request, response) != StatusCode.OK) return response.HasError
                    ? StatusCode.ERROR
                    : throw new RdxInvalidOperationException($"ETL halted in Adapter '{Key}' attempting to query '{request.QueryEndpoint}'.");

            // L = Load
            response.Message = $"Encountered error while loading data into the '{rd.Key}' system.";
            var httpResponse = rd.Link.PostAsync(request.RequestorEndpoint, new StringContent(response.ReturnValue ?? "")).GetAwaiter().GetResult();
            response.HttpStatusCode = httpResponse.StatusCode.ToString();

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new RdxInvalidInstanceException($"Failed to send response to '{request.RequestorEndpoint}'.");
            }
        }
        catch (Exception ex)
        {
            response.Errors.Push(ex);
            return StatusCode.ERROR;
        }
        response.Message = $"Queried '{cs.Key}' using '{Key}' to reach '{request.QueryEndpoint}'.";
        response.HttpStatusCode = GlobalConstants.OK;
        return StatusCode.OK;
    }
}
