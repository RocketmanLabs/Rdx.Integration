using Rdx.Core;

namespace Rdx.UnitTests;

public class Tests_for_Rq
{
    [Theory]
    [InlineData("idfromemail", "idfromemail", 0, new string[0], new string[0])]
    [InlineData("idfromemail?", "idfromemail", 0, new string[0], new string[0])]
    [InlineData("idfromemail?email=tsm@rd.net", "idfromemail", 1, new[] { "email" }, new[] { "tsm@rd.net" })]
    [InlineData("plotpoint?x=1&y=2&z=3", "plotpoint", 3, new[] { "x", "y", "z" }, new[] { "1", "2", "3" })]
    [InlineData(@"https:\\foo.bar\zibbitz\idfromemail?email=tsm@rd.net", "idfromemail", 1, new[] { "email" }, new[] { "tsm@rd.net" })]
    [InlineData("plotpoint?x=1&y=2&z", "plotpoint", 3, new[] { "x", "y", "z" }, new[] { "1", "2", null })]
    public void Rq_SUCCESS_parse_query_url(string query, string endp, int count, string[] argNames, string[] argValues)
    {
        Rq rq = Rq.BuildFromQuery(query);
        Assert.Equal(endp, rq.QueryEndpoint);
        Assert.Equal(count, rq.Criteria.Count);
        var names = rq.Criteria.Values.Select(arg => arg.Key).ToArray();
        var values = rq.Criteria.Values.Select(arg => arg.Value).ToArray();
        Assert.Equal(argNames, names);
        Assert.Equal(argValues, values);
    }
}
