using System.Reflection;

namespace Babylon.Modules.Users.Application;

public static class AssemblyReference
{
    public static Assembly Assembly { get; } = typeof(AssemblyReference).Assembly;
}
    

