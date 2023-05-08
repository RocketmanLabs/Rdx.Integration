﻿using Rdx.ActionInterface;
using Rdx.Core;
using Rdx.Core.Exceptions;

namespace Rdx.DataConnection;

/// <summary>
/// RdxAdapter acts as a container for ETL Actions.
/// </summary>
public class RdxAdapter
{
    private ActionCatalog _actions = new();

    protected RdxAdapter() { }

    public string? Key { get; set; }
    public SystemConnection? Originator { get; private set; }
    public SystemConnection? Receiver { get; private set; }

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
        // ensure we are linked to the RD Product ("originator") and the CS ("receiver") and both have authenticated this service
        try
        {
            response.Errors.Clear();

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

        // E = Extract
        try
        {
            // find action method in DLL
            _RdxAction? action = null;

            action = _actions.Find(request.Endpoint);
            if (action is null) throw new RdxInvalidOperationException($"Error encountered during retrieval of '{request.Endpoint}'.");

            if (action.EtlExtract(rd, request, response) != StatusCode.OK)
            {

            }
            if (response.HasError) throw new RdxInvalidOperationException($"Error encountered during extraction of data using '{request.Endpoint}'.");
        }
        catch (Exception ex)
        {
            response.Errors.Push(ex);
            response.Message = $"Etl halted in Adapter '{Key}', encountered error while extracting data using '{request.Endpoint}.";
            return StatusCode.ERROR;
        }

        // T = Transform
        try
        {
            if (receiver is null) throw new RdxNullException($"System Receiver must not be null in RdxAdapter.Etl()");
            if (action.EtlFormatReturn(receiver, request, response) != GlobalConstants.OK)
            {

            }
            if (response.HasError) throw new RdxInvalidOperationException($"Error encountered during transformation of data using '{request.Endpoint}'.");
        }
        catch (Exception ex)
        {
            response.Errors.Push(ex);
            response.Message = $"Etl halted in Adapter '{Key}', encountered error while transforming data using '{request.Endpoint}.";
            return StatusCode.ERROR;
        }

        // L = Load
        try
        {
            EtlLoadOriginator(request, response);
            if (response.HasError) throw new RdxInvalidOperationException($"Error encountered during transformation of data using '{request.Endpoint}'.");
        }
        catch (Exception ex)
        {
            response.Errors.Push(ex);
            response.Message = $"Etl halted in Adapter '{Key}', encountered error while loading data into Originating system using '{request.Endpoint}.";
            return StatusCode.ERROR;
        }
        return StatusCode.OK;
    }

    protected virtual void EtlLoadOriginator(Rq request, Rs response)
    {
        throw new NotImplementedException();
    }
}
