using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.ObjectFactories;

namespace NitroWin.Factories;

public sealed class ServiceProviderObjectFactory(IServiceProvider services) : IObjectFactory {
    private readonly DefaultObjectFactory _defaultFactory = new();

    public object Create(Type type) {
        try {
            return ActivatorUtilities.CreateInstance(services, type);
        } catch {
            return _defaultFactory.Create(type);
        }
    }

    public object? CreatePrimitive(Type type)
        => _defaultFactory.CreatePrimitive(type);

    public void ExecuteOnDeserialized(object value)
        => _defaultFactory.ExecuteOnDeserialized(value);

    public void ExecuteOnDeserializing(object value)
        => _defaultFactory.ExecuteOnDeserializing(value);

    public void ExecuteOnSerialized(object value)
        => _defaultFactory.ExecuteOnSerialized(value);

    public void ExecuteOnSerializing(object value)
        => _defaultFactory.ExecuteOnSerializing(value);

    public bool GetDictionary(
        IObjectDescriptor descriptor,
        out IDictionary? dictionary,
        out Type[]? genericArguments)
        => _defaultFactory.GetDictionary(
            descriptor,
            out dictionary,
            out genericArguments);

    public Type GetValueType(Type type)
        => _defaultFactory.GetValueType(type);
}
