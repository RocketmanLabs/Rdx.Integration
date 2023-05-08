using Rdx.Core;

namespace Rdx.UnitTests;

public class Tests_for_RdVersion
{
    // Rules:
    // 1. Incrementing major zeroes minor and revision
    // 2. Incrementing minor zeroes revision
    // 3. Incrementing revision has no other effects
    // 4. Incrementing build has not other effects
    // 5. When comparing two RdVersion instances, only major, minor, and revision are compared


    public const string VERSION = "3.27.83.105";
    public const string VERSION_PLUS_MAJOR = "4.0.0.105";
    public const string VERSION_PLUS_MINOR = "3.28.0.105";
    public const string VERSION_PLUS_REVISION = "3.27.84.105";
    public const string VERSION_PLUS_BUILD = "3.27.83.106";
    public const string VERSION_LT_MAJOR = "2.27.83.105";
    public const string VERSION_LT_MINOR = "3.26.83.105";
    public const string VERSION_LT_REVISION = "3.27.82.105";
    public const string VERSION_LT_BUILD = "3.27.83.104";
    public const string VERSION_GT_MAJOR = "4.27.83.105";
    public const string VERSION_GT_MINOR = "3.28.83.105";
    public const string VERSION_GT_REVISION = "3.27.84.105";
    public const string VERSION_GT_BUILD = "3.27.83.106";
    public const string VERSION_LTE_MAJOR = "3.27.83.105";
    public const string VERSION_LTE_MINOR = "3.27.83.105";
    public const string VERSION_LTE_REVISION = "3.27.83.105";
    public const string VERSION_LTE_BUILD = "3.27.83.105";
    public const string VERSION_GTE_MAJOR = "3.27.83.105";
    public const string VERSION_GTE_MINOR = "3.27.83.105";
    public const string VERSION_GTE_REVISION = "3.27.83.105";
    public const string VERSION_GTE_BUILD = "3.27.83.105";
    public const string VERSION_PARTIAL_UP_TO_MAJOR = "3";
    public const string VERSION_PARTIAL_UP_TO_MINOR = "3.27";
    public const string VERSION_PARTIAL_UP_TO_REVISION = "3.27.83";
    public const string VERSION_PARTIAL_UP_TO_MAJOR_RESULT = "3.0.0.0";
    public const string VERSION_PARTIAL_UP_TO_MINOR_RESULT = "3.27.0.0";
    public const string VERSION_PARTIAL_UP_TO_REVISION_RESULT = "3.27.83.0";

    [Fact]
    public void RdVersion_SUCCESS_Parse_version_string()
    {
        var version = new RdVersion(VERSION);
        Assert.Equal(VERSION, version.FormattedVersion);
    }

    [Fact]
    public void RdVersion_SUCCESS_IncrementMajor()
    {
        var version = new RdVersion(VERSION);
        version.IncrementMajor();
        Assert.Equal(VERSION_PLUS_MAJOR, version.FormattedVersion);
    }

    [Fact]
    public void RdVersion_SUCCESS_IncrementMinor()
    {
        var version = new RdVersion(VERSION);
        version.IncrementMinor();
        Assert.Equal(VERSION_PLUS_MINOR, version.FormattedVersion);
    }

    [Fact]
    public void RdVersion_SUCCESS_IncrementRevision()
    {
        var version = new RdVersion(VERSION);
        version.IncrementRevision();
        Assert.Equal(VERSION_PLUS_REVISION, version.FormattedVersion);
    }

    [Fact]
    public void RdVersion_FAIL_Parsing_null()
    {
        try
        {
            string? text = null;
            var version = new RdVersion(text!);
            Assert.True(false, "Did not throw on null");
        }
        catch
        {
            Assert.True(true, "Correctly handled null!");
        }
    }

    [Fact]
    public void RdVersion_FAIL_Parsing_empty()
    {
        try
        {
            string? text = "";
            var version = new RdVersion(text);
            Assert.True(false, "Did not throw on empty");
        }
        catch
        {
            Assert.True(true, "Correctly handled empty!");
        }
    }

    [Theory]
    [InlineData(VERSION, VERSION, "Versions are equal")]
    [InlineData(VERSION_PARTIAL_UP_TO_MAJOR, VERSION_PARTIAL_UP_TO_MAJOR_RESULT, "Major only - Versions are equal")]
    [InlineData(VERSION_PARTIAL_UP_TO_MINOR, VERSION_PARTIAL_UP_TO_MINOR_RESULT, "Major, minor - Versions are equal")]
    [InlineData(VERSION_PARTIAL_UP_TO_REVISION, VERSION_PARTIAL_UP_TO_REVISION_RESULT, "Major, minor, revision - Versions are equal")]
    public void RdVersion_SUCCESS_equality_and_partials(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.True(versionA == versionB, msg);
    }

    [Theory]
    [InlineData(VERSION, VERSION_PLUS_MAJOR, "Versions are unequal")]
    [InlineData(VERSION, VERSION_PLUS_MINOR, "Versions are unequal")]
    public void RdVersion_SUCCESS_inequality(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.True(versionA != versionB, msg);
    }

    [Theory]
    [InlineData(VERSION, VERSION_PLUS_BUILD, "Versions are unequal, but build is not tested for equality")]
    [InlineData(VERSION, VERSION_PLUS_REVISION, "Versions are unequal, but revision is not tested for equality")]
    public void RdVersion_FAIL_inequality(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.False(versionA != versionB, msg);
    }

    [Theory]
    [InlineData(VERSION, VERSION_LT_MAJOR, "Second entry less than")]
    [InlineData(VERSION, VERSION_LT_MINOR, "Second entry less than")]
    [InlineData(VERSION, VERSION_LT_REVISION, "Second entry less than, but revision is not tested for equality")]
    [InlineData(VERSION, VERSION_LT_BUILD, "Second entry less than, but build is not tested for equality")]
    public void RdVersion_SUCCESS_lt(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.True(versionA < versionB, msg);
    }

    [Theory]
    [InlineData(VERSION, VERSION_LT_MAJOR, "Second entry less than")]
    [InlineData(VERSION, VERSION_LT_MINOR, "Second entry less than")]
    [InlineData(VERSION, VERSION_LT_REVISION, "Second entry less than, but revision is not tested for equality")]
    [InlineData(VERSION, VERSION_LT_BUILD, "Second entry less than, but build is not tested for equality")]
    public void RdVersion_FAIL_lt(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.False(versionA < versionB, msg);
    }

    [Theory]
    [InlineData(VERSION, VERSION_GTE_MAJOR, "Versions are greater than or equal to")]
    //[InlineData(VERSION, VERSION_GTE_MINOR, "Versions are greater than or equal to")]
    [InlineData(VERSION, VERSION_GTE_REVISION, "Versions are greater than or equal to, but revision is not tested for equality")]
    [InlineData(VERSION, VERSION_GTE_BUILD, "Versions are greater than or equal to, but build is not tested for equality")]
    [InlineData(VERSION, VERSION_LTE_MAJOR, "Versions are less than or equal to")]
    //[InlineData(VERSION, VERSION_LTE_MINOR, "Versions are less than or equal to")]
    [InlineData(VERSION, VERSION_LTE_REVISION, "Versions are less than or equal to, but revision is not tested for equality")]
    [InlineData(VERSION, VERSION_LTE_BUILD, "Versions are less than or equal to, but build is not tested for equality")]
    public void RdVersion_SUCCESS_gte_lte(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.True(versionA >versionB, msg);
    }

    [Fact]
    public void RdVersion_SUCCESS_IncrementBuild()
    {
        var version = new RdVersion(VERSION);
        version.IncrementRevision();
        Assert.Equal(VERSION_PLUS_BUILD, version.FormattedVersion);
    }

    [Fact]
    public void RdVersion_SUCCESS_Create_and_identify_Zero()
    {
        var version = RdVersion.Zero;
        Assert.True(version.IsZero);
        version.IncrementMajor();
        Assert.False(version.IsZero);
    }

    [Theory]
    [InlineData(VERSION, VERSION_GT_MAJOR, 1, "Second entry is gt")]
    [InlineData(VERSION, VERSION_GT_MINOR, 1, "Second entry is gt")]
    [InlineData(VERSION, VERSION_GT_REVISION, 0, "Second entry is gt, but revision is not tested for equality")]
    [InlineData(VERSION, VERSION_GT_BUILD, 0, "Second entry is gt, but build is not tested for equality")]
    [InlineData(VERSION, VERSION_LT_MAJOR, -1, "Second entry is lt")]
    [InlineData(VERSION, VERSION_LT_MINOR, -1, "Second entry is lt")]
    [InlineData(VERSION, VERSION_LT_REVISION, 0, "Second entry is lt, but revision is not tested for equality")]
    [InlineData(VERSION, VERSION_LT_BUILD, 0, "Second entry is lt, but build is not tested for equality")]
    public void RdVersion_SUCCESS_compare(string a, string b, int expected, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.Equal(expected, versionA.CompareTo(versionB));
    }

    [Theory]
    [InlineData(VERSION, VERSION_GT_MAJOR, "Second entry greater than")]
    [InlineData(VERSION, VERSION_GT_MINOR, "Second entry greater than")]
    [InlineData(VERSION, VERSION_GT_REVISION, "Second entry greater than")]
    public void RdVersion_SUCCESS_gt(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.True(versionA >= versionB, msg);
        Assert.True(versionA <= versionB, msg);
    }

    [Theory]
    [InlineData(VERSION, VERSION_GT_BUILD, "Second entry greater than, but build is not tested for equality")]
    public void RdVersion_FAIL_gt(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.False(versionA > versionB, msg);
    }
}
