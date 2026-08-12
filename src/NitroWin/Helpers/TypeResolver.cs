using Spectre.Console.Cli;

namespace NitroWin.Helpers;

internal sealed class TypeResolver(IServiceProvider provider) : ITypeResolver {
    public object? Resolve(Type? type) => type == null ? null : provider.GetService(type);
}
