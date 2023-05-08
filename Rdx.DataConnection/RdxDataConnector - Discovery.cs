using Rdx.ActionInterface;
using Rdx.Core;
using Rdx.Core.Exceptions;
using System;

namespace Rdx.DataConnection;

/// <summary>
/// This section of the class definition covers the Discovery behavior. Discovery
/// includes scanning the Adapters and Overrides directories, finding the DLLs,
/// and building the ActionCatalog.
/// </summary>
public partial class RdxDataConnector
{
    private ExternalSystems _sysDefs;
    private AdapterCatalog _adapters = new();  
    private ActionCatalog _actions = new();

    public RdxDataConnector(ExternalSystems sysDefs)
    {
        _sysDefs = sysDefs;
    }

    public string? Key { get; set; }
    public SystemConnection? Originator { get; private set; }
    public SystemConnection? Receiver { get; private set; }

    public static StatusCode Discovery()
    {
        try
        {
            // TODO: Read file into a new ExternalSystems instance
            // TODO: Scan DLLs, creating an AdapterFile for each
            // TODO: Test AdapterFiles against the ExternalSystems to see if they match
            // TODO: Load the DLLs 
            // TODO: Create the AdapterCatalog

            // TODO: Create the ActionCatalog
            // TODO: Create HttpClient instances for each system
            // TODO: sign in using the endpoint and credentials
        } catch (Exception ex)
        {
            response.Errors.Push(ex);
            response.Message = $"Etl halted in Adapter '{Key}', encounterd error will retrieving '{request.Endpoint}'.";
            return StatusCode.ERROR;
        }
        return StatusCode.OK;
    }

    /// <summary>
    /// The ETL method performs the following:
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
    protected virtual StatusCode Etl(SystemConnection rd, SystemConnection cs, Rq request, Rs response)
    {
        // Ensure RDX is linked to the RD Product ("originator") and the CS
        // ("receiver") and both have authenticated this service.
        response.Errors.Clear();

        try
        {
            Originator = rd;
            Receiver = cs;

            if (rd.IsEmpty) throw new RdxInvalidInstanceException($"The connection to the originating system cannot be empty.");
            if (cs.IsEmpty) throw new RdxInvalidInstanceException($"The connection to the receiving system cannot be empty.");
            if (!rd.IsAuthenticated) throw new RdxInvalidInstanceException($"This service has not been authenticated to the originating system.");
            if (!cs.IsAuthenticated) throw new RdxInvalidInstanceException($"This service has not been authenticated to the receiving system.");
        }
        catch (Exception ex)
        {
            response.Errors.Push(ex);
            response.Message = $"Etl halted in Adapter '{Key}', encounterd error will retrieving '{request.Endpoint}'.";
            return StatusCode.ERROR;
        }

        _RdxAction? action = null;

        // E = Extract: find and call the appropriate Action, which does the actual 
        // extraction in _RdxAction.EtlExtract() method.  Any exceptions that occur
        // are recorded in the response packet.
        try
        {
            action = _actions.Find(request.Endpoint);

            if (action is null) throw new RdxInvalidOperationException($"Error encountered during retrieval of '{request.Endpoint}'.");

            if (action.EtlExtract(rd, request, response) != StatusCode.OK)
            {
                response.Message = $"Etl halted in Adapter '{Key}', encountered error while extracting data using '{request.Endpoint}.";
                return StatusCode.ERROR;
            }
        }
        catch (Exception ex)
        {
            response.Errors.Push(ex);
            response.Message = $"Etl halted in Adapter '{Key}', encountered error while extracting data using '{request.Endpoint}.";
            return StatusCode.ERROR;
        }

        // T = Transform: reassembles responses from the customer system in _RdxAction.EtlFormatReturn().
        try
        {
            if (action.EtlFormatReturn(rd, request, response) != StatusCode.OK)
            {
                response.Message = $"Etl halted in Adapter '{Key}', encountered error while transforming data using '{request.Endpoint}.";
                return StatusCode.ERROR;
            }
        }
        catch (Exception ex)
        {
            response.Errors.Push(ex);
            response.Message = $"Etl halted in Adapter '{Key}', encountered error while transforming data using '{request.Endpoint}.";
            return StatusCode.ERROR;
        }

        // L = Load: calls the RD product and sends the reformatted response.
        try
        {
            // TODO: send request.ReturnValue to RD Product
            //if (???)
            //{
            //    response.Message = $"Error encountered during return of data using '{request.Endpoint}'.";
            //    return StatusCode.ERROR;
            //}
        }
        catch (Exception ex)
        {
            response.Errors.Push(ex);
            response.Message = $"Error encountered during return of data using '{request.Endpoint}'.";
            return StatusCode.ERROR;
        }
        return StatusCode.OK;
    }
}