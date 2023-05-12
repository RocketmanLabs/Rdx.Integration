using Rdx.ActionInterface;
using Rdx.Core;
using Rdx.Core.Exceptions;

namespace Rdx.DataConnection;

public class ActionCatalog : Dictionary<string, _RdxAction>
{
    public ActionCatalog() { }

    public Exception? Error { get; private set; }
    public bool HasError => Error is null;

    /// <summary>
    /// Adds actions to the catalog if they are not already there. Duplicates that
    /// are encountered while loading an Override will be replaced, otherwise an
    /// error is thrown and you are advised to rebuild the adapter to remove the
    /// conflict.
    /// </summary>
    public StatusCode Add(string dllPath, _RdxAction action, bool isOverride = false)
    {
        try
        {
            if (this.ContainsKey(action.Key))
            {
                if (!isOverride)
                {
                    throw new RdxDuplicateException($"The '{action.Key}' action is already in the Action Catalog.  Rework the '{dllPath}' adapter and reload.");
                }
                else
                {
                    this[action.Key] = action;  // overwrite existing action
                }
            }
            else
            {
                base.Add(action.Key, action);
            }
            return StatusCode.OK;

        }
        catch (Exception ex)
        {
            Error = ex;
            return StatusCode.ERROR;
        }
    }

    public _RdxAction? Find(string? queryEndpoint) =>
        this.Values.FirstOrDefault(x => queryEndpoint is not null
            && queryEndpoint.EndsWith(
                x.Key,
                StringComparison.InvariantCultureIgnoreCase)
        );
}
