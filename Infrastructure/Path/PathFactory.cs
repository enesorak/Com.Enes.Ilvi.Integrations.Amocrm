using System.Reflection;
using Application.Abstractions.Path;

namespace Infrastructure.Path;

public class PathFactory : IPathFactory
{
    public string FilePath(string name)
    {
        return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
            name);
    }
}