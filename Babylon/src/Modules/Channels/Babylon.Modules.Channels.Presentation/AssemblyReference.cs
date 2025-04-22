using System.Reflection;

namespace Babylon.Modules.Channels.Presentation;
public static class AssemblyReference
{
    public static Assembly Assembly { get; } = typeof(AssemblyReference).Assembly;
}
