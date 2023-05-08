using Rdx.ActionInterface.Interfaces;
using Rdx.Core;
using Rdx.Core.Exceptions;

namespace Rdx.ActionInterface;

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

/// <summary>
/// Base class for all Action classes. An Action class represents a single ETL
/// transaction, identified by the "Key" property.
/// </summary>
public abstract class _RdxAction 
{
    protected _RdxAction(string key, Args expectedCriteria)
    {
        Key = key;
        ExpectedCriteria = new(expectedCriteria.Values.ToArray());
    }

    public string Key { get; private set; }
    public Args ExpectedCriteria { get; private set; } = new();

    /// <summary>
    /// Validation consists of:
    /// 
    /// 1. Identifying required Args that have not been supplied by comparing
    /// the 'submitted' collection with an 'expected' collection.
    /// 2. Setting values on uninitialized Args, acting as a 'default value'
    /// mechanism. 
    /// </summary>
    public virtual StatusCode Validate(Rq request, Rs response)
    {
        response.Errors.Clear();

        List<Exception> validationErrors = new();
        StatusCode retVal = _RdxAction.checkArgs(ExpectedCriteria, request.Criteria, validationErrors);
        response.Errors = new(validationErrors);
        return retVal;
    }

    /// <summary>
    /// Override and code the data gathering and RawResponse assembly here.
    /// </summary>
    public virtual StatusCode EtlExtract(SystemConnection cs, Rq request, Rs response)
    {
        response.Errors.Push(new NotImplementedException($"EtlExtract() has not been coded in the '{Key}' action."));
        return response.HasError ? StatusCode.ERROR : StatusCode.OK;
    }

    /// <summary>
    /// Override and code how RawResponse is formatted and returned via the
    /// response object's ReturnValue string.
    /// </summary>
    public virtual StatusCode EtlFormatReturn(SystemConnection rdProduct, Rq request, Rs response)
    {
        response.ReturnValue = response.RawResponse;
        return response.HasError ? StatusCode.ERROR : StatusCode.OK;
    }

    private static StatusCode checkArgs(Args expected, Args submitted, List<Exception> validationErrors)
    {
        try
        {
            // iterate through the Arg instances in the 'expected' collection
            foreach (Arg expArg in expected.Values)
            {
                if (expArg.Key is null) throw new RdxNullException("Validation halted - an expected argument contains a null key.");

                // error if there is no match in the 'submitted' collection
                if (!submitted.ContainsKey(expArg.Key))
                {
                    validationErrors.Add(new RdxMissingItemException($"Missing required argument: {expArg.Key}"));
                }
                else
                {
                    // if there is a match, check if subArg needs a value (which it will get from its matching expArg)
                    Arg subArg = submitted[expArg.Key];
                    if (!subArg.ValueSet)
                    {
                        subArg.Value = expArg.Value;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            validationErrors.Add(ex);
        }
        return validationErrors.Any() ? StatusCode.VALIDATION_ERROR : StatusCode.OK;
    }
}