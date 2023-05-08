using Rdx.Core;

namespace Rdx.ActionInterface.Interfaces;

public interface ISystemConnection
{
    bool IsAuthenticated { get; }
    HttpClient Link { get; }
}