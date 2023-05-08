using Rdx.Core.Exceptions;

namespace Rdx.Core;


/*:::::::::::::::::::::::::::::::::::::
 Args - Collection of Arg instances.

:::::::::::::::::::::::::::::::::::::::*/
public class Args : Dictionary<string, Arg>
{
    public Args() { }

    public Args(params Arg[] args)
    {
        foreach(Arg arg in args)
        {
            Add(arg);
        }
    }

    new public IEnumerator<Arg> GetEnumerator() => this.Values.GetEnumerator(); 

    public static Stack<Exception> ValidationErrors { get; private set; } = new();
    public bool HasValidationError => ValidationErrors.Any();

    public void Add(Arg arg) => Add(arg.Key ?? throw new RdxNullException($"Add operation halted, Key cannot be null."), arg);
}
