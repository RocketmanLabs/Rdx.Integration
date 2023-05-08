# Developing RDX Adapters
RDX Adapters bridge the gap between an RD Product and a customer system.  The DLL's name reflects
the product plus the product version, and the customer system, along with its version.

The Adapter is a container for Class files that represent RDX Actions, individual ETL operations
which are named for the endpoint they represent.  A query from the RD Product activates the Action,
which then makes 1..n queries of the customer system.  The response(s) are then assembled into a
return value, which is posted to the RD Product's API.

The first step is to construct the DLL's name based on the systems it supports.  Once that name is
built it is incorporated in the Visual Studio Project name.  All names in RDX that reference systems
will use their abbreviated names, such as ENG for Engagefully, and IMP for iMIS Professional.  These
names are recorded in the ExternalSystems.md catalog of system information.

1. To build the DLL name, start with the RD Product name, such as ENG.
2. Then add the current version in parenthesis, substituting '-' for '.'.  Only the first three values
in the version are needed (Major, Minor, and Revision).
3. Append an '=' to make "ENG(2-1-5)=".
4. Now add the CS abbreviated name and supported version.  In this example, this would yield "ENG(2-1-5)=IMP(100-3-0)"
5. This will be the Adapter's file name.
Note: if creating demonstration Adapters or Overrides, omit the version, but leave the parenthesis.

## The Visual Studio Project
6. Create a Visual Studio Class Library project called Rdx.Adapters.{name}.
7. Set up Project dependencies on Rdx.ActionInterface and Rdx.Core.
8. Change Project Properties to set Assembly name to the name developed above, such as "ENG(2-1-5)=IMP(100-3-0)".
9. Make sure the project file (.csproj) contains these settings:

<Project Sdk="Microsoft.NET.Sdk">
<PropertyGroup>
		<TargetFramework>net6.0</TargetFramework>
		<ImplicitUsings>enable</ImplicitUsings>
		<Nullable>enable</Nullable>
		<EnableDynamicLoading>true</EnableDynamicLoading>
		<AssemblyName>ENG(2-1-5)=IMP(100-3-0)</AssemblyName>
	</PropertyGroup>

	<ItemGroup>
		<ProjectReference Include="..\Rdx.ActionInterface\Rdx.ActionInterface.csproj">
			<Private>false</Private>
			<ExcludeAssets>runtime</ExcludeAssets>
		</ProjectReference>
		<ProjectReference Include="..\Rdx.Core\Rdx.Core.csproj" />
	</ItemGroup>
</PropertyGroup>
</Project>

