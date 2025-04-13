using Newtonsoft.Json;

namespace Babylon.Common.Infrastructure.Serialization;
public static class SerializationSettings
{
    public readonly static JsonSerializerSettings Instance = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
    };
}
