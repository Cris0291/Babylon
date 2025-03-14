using System.Reflection;

namespace Babylon.Modules.Channels.Application;

public static class AssemblyReference
{
    public static Assembly Assembly { get; } = typeof(AssemblyReference).Assembly;
}
    

