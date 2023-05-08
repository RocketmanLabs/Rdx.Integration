using Rdx.ActionInterface;
using Rdx.DataConnection;

namespace Rdx.UnitTests;

public class Tests_for_ActionCatalog
{
    [Fact]
    public void Find_action_SUCCESS_using_request_endpoint()
    {
        ActionCatalog cat = Helpers.LoadCatalogWithExampleActions();
        _RdxAction? action = cat.Find("DeleteTreasure");
        Assert.NotNull(action);
        Assert.Equal("DeleteTreasure", action.Key);
    }

    [Fact]
    public void Find_action_FAIL_using_null_endpoint()
    {
        ActionCatalog cat = Helpers.LoadCatalogWithExampleActions();
        _RdxAction? action = cat.Find(null);
        Assert.Null(action);
    }
}
