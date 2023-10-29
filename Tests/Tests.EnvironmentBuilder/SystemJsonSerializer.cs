using System.Text.Json;
using Flurl.Http.Configuration;

namespace Tests.EnvironmentBuilder;

public class SystemJsonSerializer : ISerializer
{
    private readonly JsonSerializerOptions? _options;

    public SystemJsonSerializer(JsonSerializerOptions? options = null)
    {
        _options = options;
    }

    public T Deserialize<T>(string s) => JsonSerializer.Deserialize<T>(s, _options)!;

    public T Deserialize<T>(Stream stream)
    {
        using var reader = new StreamReader(stream);
        return Deserialize<T>(reader.ReadToEnd());
    }

    public string Serialize(object obj) => JsonSerializer.Serialize(obj, _options);
}