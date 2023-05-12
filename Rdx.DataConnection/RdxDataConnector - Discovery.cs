using Rdx.ActionInterface;
using Rdx.Core;
using Rdx.Core.Exceptions;
using System;
using System.Reflection;
using System.Runtime.Loader;

namespace Rdx.DataConnection;

/// <summary>
/// This section of the class definition covers the Discovery behavior. Discovery
/// includes scanning the Adapters and Overrides directories, finding the DLLs,
/// and building the ActionCatalog.
/// </summary>
public partial class RdxDataConnector
{
    private readonly ExternalSystems _xs;
    private AdapterCatalog _adapters = new();
    private ActionCatalog _actions = new();

    public RdxDataConnector(ExternalSystems sysDefs)
    {
        _xs = sysDefs;
    }

    public string? Key { get; set; }
    public SystemConnection? Originator { get; private set; }
    public SystemConnection? Receiver { get; private set; }
    public ExternalSystems? SysConns { get; private set; }
    public Stack<Exception> Errors { get; private set; } = new();
    public string ErrorMessages => HasErrors
        ? String.Join("; ", Errors.Select(x=> x.GetType().Name + ": " + x.Message))
        : "";
    public bool HasErrors => Errors.Any();

    private const string DLLPATTERN = "*.dll";

    public StatusCode Discovery()
    {
        try
        {
            SysConns = _xs;

            // Create the AdapterCatalog - scan DLLs, creating an AdapterFile for each
            var current = Directory.GetCurrentDirectory();
            foreach (string file in Directory.GetFiles(current, DLLPATTERN))
            {
                AdapterFile adf = new(file, isOverride: false);
                _adapters.Add(adf);
            }

            // TODO: Test AdapterFiles against the ExternalSystems to see if they match

            // Load the DLLs
            foreach (AdapterFile adf in _adapters.Values)
            {
                var asm = LoadDll(adf.Key);
                if (asm is null) throw new RdxNullException($"Failed to load assembly for DLL located at '{adf.Key}'.");

                // Contribute to the ActionCatalog
                ExtractActions(asm, adf);
            }

            // TODO: Create HttpClient instances for each system
            // TODO: sign in using the endpoint and credentials
        }
        catch (Exception ex)
        {
            Errors.Push(ex);
            return StatusCode.ERROR;
        }
        return StatusCode.OK;
    }

    public Assembly? LoadDll(string path)
    {
        string pluginLocation = Path.GetFullPath(path.Replace('\\', Path.DirectorySeparatorChar));
        ExtensibilityLoadContext loadContext = new(pluginLocation);

        return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
    }

    /// <summary>
    /// Loads the ActionCatalog from the assembly.
    /// </summary>
    public StatusCode ExtractActions(Assembly asm, AdapterFile adf)
    {
        // examine all Types in the assembly
        foreach (Type type in asm.GetTypes())
        {
            // continue processing if a given type is (or inherits from) _RdxAction
            if (typeof(_RdxAction).IsAssignableFrom(type))
            {
                _RdxAction? action = Activator.CreateInstance(type) as _RdxAction;

                // If CreateInstance() was successful, add to ActionCatalog
                if (action is not null)
                {
                    // if ActionCatalog.Add() results in an error, capture it and return StatusCode.ERROR
                    if (_actions.Add(adf.Key, action, adf.IsOverride) != StatusCode.OK)
                    {
                        if (_actions.HasError)
                        {
                            throw _actions.Error!;
                        }
                        else
                        {
                            throw new RdxInvalidOperationException($"Error encountered while appending Action Catalog: {_actions.Error!.Message}");
                        }
                    }
                }
            }
        }
        return StatusCode.OK;
    }
}

public class ExtensibilityLoadContext : AssemblyLoadContext
{
    private AssemblyDependencyResolver _resolver;

    public ExtensibilityLoadContext(string pluginPath)
    {
        _resolver = new AssemblyDependencyResolver(pluginPath);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        if (assemblyPath != null)
        {
            return LoadFromAssemblyPath(assemblyPath);
        }

        return null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        string? libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (libraryPath != null)
        {
            return LoadUnmanagedDllFromPath(libraryPath);
        }

        return IntPtr.Zero;
    }
}