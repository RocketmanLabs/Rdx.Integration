using Rdx.Core;
using Rdx.Core.Exceptions;

namespace Rdx.UnitTests;

public class Tests_for_RdVersion
{
    // Rules:
    // 1. Incrementing major zeroes minor and revision
    // 2. Incrementing minor zeroes revision
    // 3. Incrementing revision has no other effects
    // 4. Incrementing build has no other effects
    // 5. When comparing two RdVersion instances, only major, minor, and revision are compared

    public const string VERSION = "3.27.83.105";
    public const string VERSION_3_ELEMENT = "3-27-83";
    public const string VERSION_PLUS_MAJOR = "4.27.83.105";
    public const string VERSION_PLUS_MINOR = "3.28.83.105";
    public const string VERSION_PLUS_REVISION = "3.27.84.105";
    public const string VERSION_PLUS_BUILD = "3.27.83.165";

    public const string VERSION_LT_MAJOR = "2.27.83.105";
    public const string VERSION_LT_MINOR = "3.26.83.105";
    public const string VERSION_LT_REVISION = "3.27.82.105";
    public const string VERSION_LT_BUILD = "3.27.83.104";

    public const string VERSION_GT_MAJOR = "4.27.83.105";
    public const string VERSION_GT_MINOR = "3.28.83.105";
    public const string VERSION_GT_REVISION = "3.27.84.105";
    public const string VERSION_GT_BUILD = "3.27.83.106";

    public const string VERSION_PARTIAL_UP_TO_MAJOR = "3";
    public const string VERSION_PARTIAL_UP_TO_MINOR = "3.27";
    public const string VERSION_PARTIAL_UP_TO_REVISION = "3.27.83";
    public const string VERSION_PARTIAL_UP_TO_MAJOR_RESULT = "3.0.0.0";
    public const string VERSION_PARTIAL_UP_TO_MINOR_RESULT = "3.27.0.0";
    public const string VERSION_PARTIAL_UP_TO_REVISION_RESULT = "3.27.83.0";
    public const string VERSION_4_ELEMENTS_SUFFIXED = "3.27.83.81056.suffix";
    public const string VERSION_4_ELEMENTS_EXTRA_STRING = "beta3.27.83.81056";

    [Fact]
    public void RdVersion_SUCCESS_Parse_version_string()
    {
        var version = new RdVersion(VERSION);
        Assert.Equal(VERSION, version.FormattedVersion);
    }

    [Fact]
    public void RdVersion_SUCCESS_AltFormattedVersion()
    {
        var version = new RdVersion(VERSION);
        Assert.Equal(VERSION, version.FormattedVersion);
    }

    [Fact]
    public void RdVersion_SUCCESS_IncrementMajor_changes_equality()
    {
        var versionA = new RdVersion(VERSION);
        var versionB = new RdVersion(VERSION);
        versionB.IncrementMajor();
        Assert.False(versionA == versionB);
        Assert.True(versionA != versionB);
    }

    [Fact]
    public void RdVersion_SUCCESS_IncrementMinor_changes_equality()
    {
        var versionA = new RdVersion(VERSION);
        var versionB = new RdVersion(VERSION);
        versionB.IncrementMinor();
        Assert.False(versionA == versionB);
        Assert.True(versionA != versionB);
    }

    [Fact]
    public void RdVersion_SUCCESS_IncrementRevision_changes_equality()
    {
        var versionA = new RdVersion(VERSION);
        var versionB = new RdVersion(VERSION);
        versionB.IncrementRevision();
        Assert.False(versionA == versionB);
        Assert.True(versionA != versionB);
    }

    [Fact]
    public void RdVersion_SUCCESS_IncrementBuild_does_not_change_equality()
    {
        var versionA = new RdVersion(VERSION);
        var versionB = new RdVersion(VERSION);
        versionB.IncrementBuild();
        Assert.True(versionA == versionB);
        Assert.False(versionA != versionB);
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

    [Fact]
    public void RdVersion_CompareTo_follows_NET_standard()
    {
        RdVersion vLower = new("1.2.3.4");
        RdVersion vUpper = new("2.3.4.5");
        string txtLower = "lower";
        string txtUpper = "upper";
        var txtLUCmp = txtLower.CompareTo(txtUpper);
        var txtULCmp = txtUpper.CompareTo(txtLower);
        var vLUCmp = vLower.CompareTo(vUpper);
        var vULCmp = vUpper.CompareTo(vLower);

        Assert.Equal(txtLUCmp, vLUCmp);
        Assert.Equal(txtULCmp, vULCmp);
    }

    [Theory]
    [InlineData(VERSION, VERSION, "Versions are equal")]
    [InlineData(VERSION_PARTIAL_UP_TO_MAJOR, VERSION_PARTIAL_UP_TO_MAJOR_RESULT, "Malformed Major only - Versions are equal")]
    [InlineData(VERSION_PARTIAL_UP_TO_MINOR, VERSION_PARTIAL_UP_TO_MINOR_RESULT, "Malformed Major, minor - Versions are equal")]
    [InlineData(VERSION_4_ELEMENTS_EXTRA_STRING, VERSION_4_ELEMENTS_EXTRA_STRING, "Well-formed Major, minor, revision, build with suffix - Versions are equal")]
    public void RdVersion_FAIL_throws_on_malformed_version(string a, string b, string msg)
    {
        try
        {
            var versionA = new RdVersion(a);
        }
        catch (RdxInvalidInstanceException)
        {
            Assert.True(true);
        }
    }

    [Theory]
    [InlineData(VERSION, VERSION, "Versions are equal")]
    [InlineData(VERSION_4_ELEMENTS_SUFFIXED, VERSION_4_ELEMENTS_SUFFIXED, "Major, minor, revision, build with suffix - Versions are equal")]
    [InlineData(VERSION, VERSION_PLUS_BUILD, "Just Build changed - Versions are equal")]
    public void RdVersion_SUCCESS_equality_and_partials(string a, string b, string msg)
    {
        try
        {
            var versionA = new RdVersion(a);
            var versionB = new RdVersion(b);
            Assert.True(versionA == versionB, msg);
        } catch(Exception)
        {
            Assert.False(true);  // should not throw
        }
    }

    [Theory]
    [InlineData(VERSION, VERSION_PLUS_MAJOR, "Major changed, Versions are unequal")]
    [InlineData(VERSION, VERSION_PLUS_MINOR, "Minor changed, Versions are unequal")]
    public void RdVersion_SUCCESS_inequality(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.True(versionA != versionB, msg);
    }

    [Theory]
    [InlineData(VERSION, VERSION_GT_MAJOR, "2nd entry Major gt")]
    [InlineData(VERSION, VERSION_GT_MINOR, "2nd entry Minor gt")]
    [InlineData(VERSION, VERSION_GT_REVISION, "2nd entry Revision gt")]
    public void RdVersion_SUCCESS_gt(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.True(versionA > versionB, msg);
        Assert.True(versionA >= versionB, msg);
    }

    [Theory]
    [InlineData(VERSION, VERSION_LT_MAJOR, "2nd entry Major lt")]
    [InlineData(VERSION, VERSION_LT_MINOR, "2nd entry Minor lt")]
    [InlineData(VERSION, VERSION_LT_REVISION, "2nd entry Revision lt")]
    public void RdVersion_FAIL_gt(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.False(versionA > versionB, msg);
    }

    [Theory]
    [InlineData(VERSION, VERSION_LT_MAJOR, "2nd entry Major lt")]
    [InlineData(VERSION, VERSION_LT_MINOR, "2nd entry Minor lt")]
    [InlineData(VERSION, VERSION_LT_REVISION, "2nd entry Revision lt")]
    public void RdVersion_SUCCESS_lt(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.True(versionA < versionB, msg);
        Assert.True(versionA <= versionB, msg);
    }

    [Theory]
    [InlineData(VERSION, VERSION_LT_BUILD, "2nd entry Build lt, appears equal")]
    public void RdVersion_FAIL_lt(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.False(versionA < versionB, msg);
    }

    [Theory]
    [InlineData(VERSION, VERSION_LT_BUILD, "2nd Version Build is less than, should appear equal")]
    public void RdVersion_SUCCESS_gt_and_eq(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.True(versionA >= versionB, msg);
    }

    [Theory]
    [InlineData(VERSION, VERSION, "Versions are equal")]
    [InlineData(VERSION, VERSION_LT_MAJOR, "2nd Version Major is less than")]
    [InlineData(VERSION, VERSION_LT_MINOR, "2nd Version Minor is less than")]
    [InlineData(VERSION, VERSION_LT_REVISION, "2nd Version Revision is less than, revision is tested for equality")]
    [InlineData(VERSION, VERSION_LT_BUILD, "2nd Version Build is less than, should appear equal")]
    public void RdVersion_SUCCESS_lte(string a, string b, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        Assert.True(versionA <= versionB, msg);
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
    [InlineData(VERSION, VERSION_GT_MAJOR, -1, "2nd entry is gt")]
    [InlineData(VERSION, VERSION_GT_MINOR, -1, "2nd entry is gt")]
    [InlineData(VERSION, VERSION_GT_REVISION, -1, "2nd entry is gt, revision is tested for equality")]
    [InlineData(VERSION, VERSION_GT_BUILD, 0, "2nd entry is gt, build is not tested for equality")]
    [InlineData(VERSION, VERSION_LT_MAJOR, 1, "2nd entry is lt")]
    [InlineData(VERSION, VERSION_LT_MINOR, 1, "2nd entry is lt")]
    [InlineData(VERSION, VERSION_LT_REVISION, 1, "2nd entry is lt, revision is tested for equality")]
    [InlineData(VERSION, VERSION_LT_BUILD, 0, "2nd entry is lt, build is not tested for equality")]
    public void RdVersion_SUCCESS_compare(string a, string b, int expected, string msg)
    {
        var versionA = new RdVersion(a);
        var versionB = new RdVersion(b);
        var cmp = versionA.CompareTo(versionB);
        Assert.Equal(expected, versionA.CompareTo(versionB));
    }
}
